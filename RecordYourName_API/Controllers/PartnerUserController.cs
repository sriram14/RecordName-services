using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecordYourName_API.DataAccess;
using RecordYourName_API.Models;
using Newtonsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace RecordYourName_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerUserController : ControllerBase
    {
        private readonly IPartnerUserRepo _recordRepo;
        private readonly string Database;

        public PartnerUserController(IPartnerUserRepo recordRepo, IConfiguration configuration)
        {
            _recordRepo = recordRepo;
            Database = configuration["ConnectionStrings:RecordYourNameDBString"];
        }

        [HttpPost("Register")]
        public IActionResult RegisterPartnerUser(RegisterPartnerUserRequest registerPartnerUserRequest)
        {
            try
            {
                string errors =  _recordRepo.RegisterPartnerUser(registerPartnerUserRequest);
                if (string.IsNullOrEmpty(errors))
                {
                    return Ok("SUCCESS");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
          
           
           
        }


    }
}
