using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Npgsql;
using Microsoft.Extensions.Configuration;
using PartnerUserLoginService.Models;
using PartnerUserLoginService.DataAccess;

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
        public IActionResult PartnerUserLogin(PartnerUserLoginRequest partnerUserLoginRequest)
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
                    return Ok(partnerUserLoginRequest.userid);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
           
        }


    }
}
