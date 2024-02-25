using Parallax.Areas.Admin.Attributes;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Parallax.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (IsValidatedUser(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, false);

                return Json(new { success = true, redirectUrl = Url.Action("Index", "Dashboard") });
            }

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
            return Json(new { success = false, message = "Geçersiz kullanıcı adı veya şifre." });
        }

        private bool IsValidatedUser(string username, string password)
        {
            using (var dbContext = new ParallaxContext())
            {

                var user = dbContext.TBLUSERs.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user != null && user.RoleID == 1)
                {
                    var identity = HttpContext.User.Identity;
                    return true;
                }

                // Kullanıcı var ve admin değilse veya kullanıcı bulunamazsa false döner
                return false;
            }


            // Kullanıcı var ve admin ise true döner
        }
    }
}
