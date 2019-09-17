using System;
using System.Collections.Generic;
using System.Linq;
using Homeworks.BusinessLogic.Interface;
using Homeworks.DataAccess.Interface;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic
{
    public class SessionLogic : ISessionLogic
    {
        private IRepository<User> repository;

        // TENDRÍA QUE SER UN SESSION REPOSITORY
        // SESSION = {
        //      token: Guid,  
        //      user: User
        // }
        // CUIDADO CON LAS VARIABLES ESTÁTICAS EN LA BUISSNESSLOGIC
        private static IDictionary<string, Guid?> TokenRepository = null;

        public SessionLogic(IRepository<User> repository) {
            this.repository = repository;
            if (TokenRepository == null) {
                TokenRepository = new Dictionary<string, Guid?>();
            }
        }

        public bool IsValidToken(string token)
        {
            // SI EL TOKEN EXISTE EN BD RETORNA TRUE
            return TokenRepository.ContainsKey(token);
        }

        public Guid? CreateToken(string userName, string password)
        {
            // SI EL USUARIO EXISTE Y LA PASS Y EL USERNAME SON EL MISMO
            // RETORNAR GUID
            var users = repository.GetAll();
            var user = users.FirstOrDefault(x => x.UserName == userName && x.Password == password);
            if (user == null) 
            {
                return null;
            }
            var token = Guid.NewGuid();
            TokenRepository.Add(token.ToString(), user.Id);
            return token;
        }

        public bool HasLevel(string token, string role)
        {  
            var user = GetUser(token);
            if (user == null) {
                return false;
            }
            if (role == "Admin") {
                return user.IsAdmin;
            }
            return true;
        }

        public User GetUser(string token) 
        {
            Guid? userId = null;
            if (TokenRepository.TryGetValue(token, out userId)) 
            {
                return repository.Get(userId.GetValueOrDefault());
            }
            return null;
        }

    }
}
