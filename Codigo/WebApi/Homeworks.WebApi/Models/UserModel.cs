using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.Domain;
using Newtonsoft.Json;

namespace Homeworks.WebApi.Models
{
    public class UserModel : Model<User, UserModel>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; } = false;

        public UserModel() { }

        public UserModel(User entity)
        {
            SetModel(entity);
        }

        public override User ToEntity() => new User()
        {
            Id = this.Id,
            Name = this.Name,
            UserName = this.UserName,
            Password = this.Password,
            IsAdmin = this.IsAdmin
        };

        protected override UserModel SetModel(User entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            UserName = entity.UserName;
            IsAdmin = entity.IsAdmin;
            return this;
        }

    }
}
