using Parallax.Areas.Admin.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Parallax.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Dashboard
        [CustomAuthorize]
        public ActionResult Index()
        {
            // Admin rolüne sahip kullanıcılar bu sayfaya erişebilir
            return View();
        }

        [CustomAuthorize] // Sadece giriş yapmış kullanıcılar bu action'a erişebilir
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut(); // Kullanıcıyı çıkış yapmış olarak işaretle

            return RedirectToAction("Login", "Login"); // Çıkış yaptıktan sonra bir sayfaya yönlendirin (Örneğin, login sayfasına)
        }
    }
}