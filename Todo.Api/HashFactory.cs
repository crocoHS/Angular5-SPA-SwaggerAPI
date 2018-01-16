using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Todo.Model;

namespace Todo.Api
{
    public static class HashFactory
    {
        public static string GenerateToken(TokenRequestModel model)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(model.Issuer, // issued by
                model.Audience, // issued for
                claims, // payload
                expires: DateTime.Now.AddMinutes(30), // valid for
                signingCredentials: creds); // signature

            var tokenEncoded = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenEncoded;
        }

        public static string GetHash(TodoItem item)
        {
            var itemText = $"{item.Id}|{item.IsComplete}|{item.Name}";

            using (var md5 = MD5.Create())
            {
                byte[] retVal = md5.ComputeHash(Encoding.Unicode.GetBytes(itemText));
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }
    }
}
