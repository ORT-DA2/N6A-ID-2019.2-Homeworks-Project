using System;
using Homeworks.WebApi.Models;
using Homeworks.BusinessLogic.Interface;
using Homeworks.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Homeworks.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private ILogic<User> users;

        public UsersController(ILogic<User> users) : base()
        {
            this.users = users;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(UserModel.ToModel(users.GetAll()));
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var user = users.Get(id);
            if (user == null) {
                return NotFound();
            }
            return Ok(UserModel.ToModel(user));
        }

        [HttpPost]
        public IActionResult Post([FromBody]UserModel model)
        {
            try {
                var user = users.Create(UserModel.ToEntity(model));
                return CreatedAtRoute("Get", new { id = user.Id }, UserModel.ToModel(user));
            } catch(ArgumentException e) {
                return BadRequest(e.Message);
            }
        }

    }
}
