using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Parallax.Areas.Admin.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                return false; // Kullanıcı giriş yapmamışsa false döner
            }

            // Diğer yetkilendirme kontrollerini burada gerçekleştirebilirsiniz
            // Örneğin, kullanıcının belirli bir rolde olup olmadığını kontrol edebilirsiniz

            return true; // Eğer diğer kontrollerden geçiyorsa true döner
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Kullanıcı giriş yapmamışsa, standart olarak belirlenen Login sayfasına yönlendir
                filterContext.Result = new RedirectResult("~/Admin/Login");
            }
            else
            {
                // Kullanıcı giriş yapmışsa ancak erişim izni yoksa, yine Login sayfasına yönlendir
                filterContext.Result = new RedirectResult("~/Admin/Login");
            }
        }
    }


}