using System;
using Microsoft.AspNetCore.Mvc;
using Homeworks.BusinessLogic.Interface;
using Homeworks.WebApi.Filters;
using Homeworks.WebApi.Models;

namespace Homeworks.WebApi.Controllers
{
    [ProtectFilter("User")]
    [Route("api/[controller]")]
    public class ProtectedController : ControllerBase
    { 
        private ISessionLogic sessions;

        public ProtectedController(ISessionLogic sessions) : base()
        {
            this.sessions = sessions;
        }

        [ProtectFilter("Admin")]
        [HttpGet("CheckAdmin")]
        public IActionResult CheckLoginAdmin() {
            return Ok(new UserModel(sessions.GetUser(Request.Headers["Authorization"])));
        }

        [ProtectFilter("User")]
        [HttpGet("CheckUser")]
        public IActionResult CheckLoginUser() {
            return Ok(new UserModel(sessions.GetUser(Request.Headers["Authorization"])));
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok("The token is valid");
        }
    }
}