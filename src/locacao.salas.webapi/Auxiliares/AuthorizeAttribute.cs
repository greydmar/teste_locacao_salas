using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace mtgroup.locacao.Auxiliares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MtGroupAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.HasAuthorizedUser())
                context.Result = new JsonResult(
                    new {message = "Unauthorized"})
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
        }
    }
}