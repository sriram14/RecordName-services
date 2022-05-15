using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PartnerApplicationServices.DataAccess;
using PartnerApplicationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PartnerApplicationServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepo _userRepo;

        public UserController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

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

        [HttpPost("Login")]
        public IActionResult VerifyUser(UserLoginRequest userLoginRequest)
        {
            try
            {
                bool login_status = _userRepo.VerifyUser(userLoginRequest);
                if (!login_status)
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(userLoginRequest.userid);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

    }
}
