using AutoMapper;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Text;
using Todo.Api.Services;
using Todo.Api.Services.Interfaces;
using Todo.Library;
using Todo.Model;

namespace Todo.Api
{
    public class Startup
    {
        public static IConfigurationRoot Configuration { get; set; }

        static Startup()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("TodoContext");
            string issuer = $"https://{Configuration["Tokens:Issuer"]}/";

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = issuer;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // verify signature to avoid tampering
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Tokens:Issuer"], // site that makes the token
                    ValidateAudience = true,
                    ValidAudience = Configuration["Tokens:Audience"], // site that consumes the token
                    ValidateLifetime = true, // validate the expiration 
                    ClockSkew = System.TimeSpan.FromMinutes(5) // tolerance for the expiration date
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:customers", policy => policy.Requirements.Add(new HasScopeRequirement("read:customers", issuer)));
                options.AddPolicy("add:technology", policy => policy.Requirements.Add(new HasScopeRequirement("add:technology", issuer)));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Todo API", Version = "v1" });
            });

            services.AddMvc();
            services.AddAutoMapper();
            services.AddSingleton<IGlobalService, GlobalService>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton(_ => Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseCors("CorsPolicy");
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseAuthentication();
            app.UseMvc();

            BL.Initialize(Configuration);
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder()
              .ConfigureAppConfiguration((ctx, cfg) =>
              {
                  cfg.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", true) // Require the json file!
                  .AddEnvironmentVariables();
              })
              .ConfigureLogging((ctx, logging) => { }) // No logging
              .UseStartup<Startup>()
              .UseSetting("DesignTime", "true")
              .Build();
        }
    }
}
