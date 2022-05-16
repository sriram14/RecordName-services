﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Npgsql;
using PartnerApplicationServices.Models;
using PartnerApplicationServices.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace PartnerApplicationServices.Controllers
{
    [Route("")]
    [ApiController]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly IFriendRepo _friendRepo;

        public FriendController(IFriendRepo friendRepo)
        {
            _friendRepo = friendRepo;
        }

        [HttpGet("GetFriends")]
        public async Task<IActionResult> GetFriends()
        {
            List<GetFriendResponse> list = new List<GetFriendResponse>();
            list = _friendRepo.GetFriend(Startup.UserClaims.FirstOrDefault(x => x.Type == "userid")?.Value);
            if(list.Count == 0)
            {
                return NoContent();
            }
            return Ok(list);
        }


        [HttpPost("AddFriend")]
        public IActionResult AddFriend(Friend friend)
        {
            try
            {
                string errors = _friendRepo.AddFriend(friend);
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

        [HttpPost("RemoveFriend")]
        public IActionResult RemoveFriend(Friend friend)
        {
            try
            {
                string errors = _friendRepo.RemoveFriend(friend);
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
