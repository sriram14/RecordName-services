using PartnerApplicationServices.DataAccess;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PartnerApplicationServices.Models;
using PartnerApplicationServices;

namespace PartnerApplicationServices.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly IUserRepo _userRepo;
        public LoginServices(IUserRepo getUserRepo)
        {
            _userRepo = getUserRepo;
        }
        public async Task<string> GenerateJSONWebToken(LoginRequest userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.ConnectionStrings.GetSection("SigningKey").Value));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var encryptionkey = Encoding.UTF8.GetBytes(Startup.ConnectionStrings.GetSection("EncryptionKey").Value);
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);


            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.userid),
                new Claim("IsAdmin", "true")
            };

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = Startup.ConnectionStrings.GetSection("Issuer").Value,
                Audience = Startup.ConnectionStrings.GetSection("Audience").Value,
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var jwt = tokenHandler.WriteToken(securityToken);
            return await Task.FromResult(jwt);
        }
    }
}
