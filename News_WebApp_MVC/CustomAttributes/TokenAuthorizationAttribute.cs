using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace News_WebApp_MVC.CustomAttributes
{

    public class TokenAuthorizationAttribute : TypeFilterAttribute
    {
        public TokenAuthorizationAttribute() : base(typeof(TokenAuthorizationFilter))
        {
        }
    }

    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
 
            if (!context.HttpContext.Request.Cookies.TryGetValue("AdminToken", out var tokenValue))
            {
                string returnUrl = context.HttpContext.Request.Path;

                context.Result = new RedirectToActionResult("Login", "AdminAccount", new { returnUrl });
                return;
            }

        }
    }
}