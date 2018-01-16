using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Todo.Model;
using Todo.Model.AccountViewModels;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpPost]
        public IActionResult GetToken([FromBody] TokenRequestModel model)
        {
            var tokenEncoded = HashFactory.GenerateToken(model);

            return new OkObjectResult(new { token = tokenEncoded });
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return Ok(new { message = "Invalid username or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!result.Succeeded)
            {
                return Ok(new { message = "Invalid username or password" });
            }

            var tokenEncoded = HashFactory.GenerateToken(new TokenRequestModel()
            {
                Email = model.Email,
                Key = _config["Tokens:Key"],
                Issuer = _config["Tokens:Issuer"],
                Audience = _config["Tokens:Audience"]
            });

            return new OkObjectResult(new { token = tokenEncoded, userId = user.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            var msg = "User logged out.";
            _logger.LogInformation(msg);

            return Ok(msg);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var msg = "User successfully created.";
                    _logger.LogInformation(msg);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(msg);
                }
            }

            return BadRequest("Something went wrong");
        }
    }
}