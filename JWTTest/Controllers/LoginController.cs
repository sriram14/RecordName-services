using JWTTest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PartnerApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace JWTTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        
        private IConfiguration _config;
        private readonly ILoginServices _loginServices;

        public LoginController(IConfiguration config, ILoginServices loginServices)    
        {    
            _config = config;
            _loginServices = loginServices;
        }    
        [AllowAnonymous]    
        [HttpPost("Logins")]    
        public async Task<IActionResult> Login([FromBody]LoginRequest loginReq)    
        {    
            IActionResult response = Unauthorized();    
            var user = _loginServices.AuthenticateUser(loginReq);   
            if (user)    
            {    
                var tokenString = await _loginServices.GenerateJSONWebToken(loginReq);    
                response = Ok(new { token = tokenString });    
            }    
            return Unauthorized();    
        }      
        
        [Authorize]
        [HttpGet("Test")]
        public IActionResult Test()
        {
            var claimsList = Startup.UserClaims.FirstOrDefault(x => x.Type == "IsAdmin")?.Value;
            return Ok(claimsList);
        }
    }
}
