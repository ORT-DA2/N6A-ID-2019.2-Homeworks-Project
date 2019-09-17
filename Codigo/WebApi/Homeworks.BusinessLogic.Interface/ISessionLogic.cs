using System;
using Homeworks.Domain;

namespace Homeworks.BusinessLogic.Interface
{
    public interface ISessionLogic
    {
        bool IsValidToken(string token);

        Guid? CreateToken(string userName, string password);

        bool HasLevel(string token, string role);

        User GetUser(string token);
    }
}
