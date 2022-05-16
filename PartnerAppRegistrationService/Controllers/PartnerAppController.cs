using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Npgsql;
using PartnerAppRegistrationService.Models;
using PartnerAppRegistrationService.DataAccess;

namespace PartnerAppRegistrationService.Controllers
{
    [Route("")]
    [ApiController]
    public class PartnerAppController : ControllerBase
    {
        private readonly IPartnerAppRepo _partnerAppRepo;

        public PartnerAppController(IPartnerAppRepo partnerAppRepo)
        {
            _partnerAppRepo = partnerAppRepo;
        }

        [HttpPost("Register")]
        public IActionResult PartnerAppRegistration(PartnerAppRegistrationRequest partnerAppRegistrationRequest)
        {
            try
            {
                PartnerAppRegistrationDetails details = new PartnerAppRegistrationDetails();
                details.guid = Guid.NewGuid().ToString();
                details.createdby = partnerAppRegistrationRequest.userid;
                details.updatedby = partnerAppRegistrationRequest.userid;
                details.createdtime = DateTime.Now;
                details.updatedtime = DateTime.Now;
                details.description = partnerAppRegistrationRequest.description;
                details.hosturl = partnerAppRegistrationRequest.hosturl;
                string errors = _partnerAppRepo.PartnerAppRegistration(details);
                if (string.IsNullOrEmpty(errors))
                {
                    return Ok(details.guid);
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

        [HttpGet("GetAllPartnerApps")]
        public IActionResult GetAllPartnerApps()
        {


            return Ok(_partnerAppRepo.GetAllPartnerApps());



        }

    }
}
