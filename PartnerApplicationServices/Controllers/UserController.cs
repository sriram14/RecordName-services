using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PartnerApplicationServices.DataAccess;
using PartnerApplicationServices.Models;
using PartnerApplicationServices.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly ILoginServices _loginServices;

        public UserController(IUserRepo userRepo, ILoginServices loginServices)
        {
            _userRepo = userRepo;
            _loginServices = loginServices;
        }

        [HttpGet("AllUsers")]
        public IActionResult AllUsers()
        {
            return Ok(_userRepo.GetAllUsers(Startup.UserClaims.FirstOrDefault(x => x.Type == "userid")?.Value));
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult RegisterUser(UserRegistrationRequest userRegistrationRequest)
        {
            try
            {
                string errors = _userRepo.RegisterUser(userRegistrationRequest);
                if (string.IsNullOrEmpty(errors))
                {
                    return Ok("SUCCESS");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> VerifyUserAsync(LoginRequest userLoginRequest)
        {
            try
            {
                bool login_status = _userRepo.VerifyUser(userLoginRequest);
                if (login_status)
                {
                    var tokenString = await _loginServices.GenerateJSONWebToken(userLoginRequest);
                    return Ok(new { token = tokenString });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("CreateAdmin")]
        public IActionResult CreateAdmin(string userid)
        {

            string result = _userRepo.CreateAdmin(Startup.UserClaims.FirstOrDefault(x => x.Type == "userid")?.Value, userid);
            if (result.Equals("SUCCESS"))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }


        }

        [AllowAnonymous]
        [HttpPost("ValidateToken")]
        public IActionResult ValidateToken([FromBody] TokenValidationRequest tokenValidationRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {

                var claimsPrincipal = tokenHandler.ValidateToken(tokenValidationRequest.token, Startup.tokenValidationParameters, out SecurityToken validatedToken);
                IEnumerable<Claim> claims = claimsPrincipal.Claims;
                var userid = claims.FirstOrDefault(x => x.Type == "userid")?.Value;
                bool isAdmin = _userRepo.isAdmin(userid);
                bool userIdMatch = userid  == tokenValidationRequest.userid;
                if(isAdmin || userIdMatch)
                {
                    return Ok();
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return Unauthorized();
            }
        }

        [HttpPost("DeleteAdmin")]
        public IActionResult DeleteAdmin(string userid)
        {

            string result = _userRepo.DeleteAdmin(Startup.UserClaims.FirstOrDefault(x => x.Type == "userid")?.Value, userid);
            if (result.Equals("SUCCESS"))
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }


        }


        [HttpGet("GetUserDetail")]
        public IActionResult GetUserDetail(string userid)
        {
            GetUserDetailResponse result = _userRepo.GetUserDetail(userid);
            return Ok(result);

        }

        [HttpGet("GetMyProfile")]
        public IActionResult GetMyProfile()
        {
            GetUserDetailResponse result = _userRepo.GetUserDetail(Startup.UserClaims.FirstOrDefault(x => x.Type == "userid")?.Value);
            return Ok(result);

        }





        [HttpPost("UpdateUserDetail")]
        public IActionResult UpdateUserDetail(UpdateUserDetailRequest updateUserDetailRequest)
        {
            string errors = _userRepo.UpdateUserDetail(updateUserDetailRequest);
            return Ok();

        }
    }

}

