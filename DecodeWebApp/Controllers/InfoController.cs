using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DecodeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DecodeWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Bearer")]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var jwt = Request.Headers["Authorization"].ToString();
            jwt = jwt.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);
            return Ok(token.Payload.Claims);
        }

        [HttpGet("Identity")]
        public IActionResult GetIdentity()
        {
            var user = new User()
            {
                Email = User.FindFirst(ClaimTypes.Email).Value,
                Nome = User.Identity.Name,
                Id = Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value)
            };


            return Ok(user);
        }
    }
}