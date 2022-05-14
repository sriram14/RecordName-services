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
                string errors = _partnerAppRepo.PartnerAppRegistration(partnerAppRegistrationRequest);
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

    }
}
