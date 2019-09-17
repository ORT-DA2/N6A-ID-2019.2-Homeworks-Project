using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Homeworks.BusinessLogic.Interface;

namespace Homeworks.WebApi.Filters {

    public class ProtectFilter : Attribute, IActionFilter
    {
        private readonly string _role;

        public ProtectFilter(string role) 
        {
            _role = role;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];
            if (token == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "Token is required",
                };
                return;
            }
            var sessions = GetSessions(context);
            if (!sessions.IsValidToken(token))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "Invalid Token",
                };
                return;
            }
            if (!sessions.HasLevel(token, _role))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 400,
                    Content = "The user isen't " + _role,
                };
                return;
            }
        }

        private static ISessionLogic GetSessions(ActionExecutingContext context)
        {
            return (ISessionLogic)context.HttpContext.RequestServices.GetService(typeof(ISessionLogic));
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do something after the action executes
        }
    }
}