using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Npgsql;
using Microsoft.Extensions.Configuration;
using PartnerUserLoginService.Models;
using PartnerUserLoginService.DataAccess;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using PartnerUserLoginService;
using System.Threading.Tasks;

namespace RecordYourName_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PartnerUserController : ControllerBase
    {
        private readonly IPartnerUserRepo _recordRepo;

        public PartnerUserController(IPartnerUserRepo recordRepo)
        {
            _recordRepo = recordRepo;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> PartnerUserLoginAsync(PartnerUserLoginRequest partnerUserLoginRequest)
        {
            try
            {
                long login_status =  _recordRepo.PartnerUserLogin(partnerUserLoginRequest);
                if (login_status==0)
                {
                    return Unauthorized();
                }
                else
                {
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Startup.ConnectionStrings.GetSection("SigningKey").Value));
                        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        var encryptionkey = Encoding.UTF8.GetBytes(Startup.ConnectionStrings.GetSection("EncryptionKey").Value);
                        var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);


                        var claims = new[] {
                            new Claim("userid", partnerUserLoginRequest.userid)
                        };

                        var descriptor = new SecurityTokenDescriptor
                        {
                            Issuer = Startup.ConnectionStrings.GetSection("Issuer").Value,
                            Audience = Startup.ConnectionStrings.GetSection("Audience").Value,
                            IssuedAt = DateTime.Now,
                            Expires = DateTime.Now.AddMinutes(15),
                            SigningCredentials = signingCredentials,
                            EncryptingCredentials = encryptingCredentials,
                            Subject = new ClaimsIdentity(claims)
                        };

                        var tokenHandler = new JwtSecurityTokenHandler();
                        var securityToken = tokenHandler.CreateToken(descriptor);
                        var jwt = tokenHandler.WriteToken(securityToken);
                        return Ok(new { token = jwt });
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
           
        }


    }
}
