using System;

namespace Homeworks.Domain
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public bool IsAdmin {get; set;} = false;

        public string Password { get; set; }

        public User() { }

        public bool IsValid() {
            return !String.IsNullOrEmpty(Name) || 
                !String.IsNullOrEmpty(UserName) || 
                !String.IsNullOrEmpty(Password);
        }

        public User Update(User user) {
            Name = user.Name;
            UserName = user.UserName;
            Password = user.Password;
            IsAdmin = user.IsAdmin;
            return this;
        }
    }
}
