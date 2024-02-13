using Parallax.Helpers;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Parallax.Controllers
{
    public class HomeController : Controller
    {

        private readonly ParallaxContext context;


        public HomeController()
        {
            context = new ParallaxContext();

        }
        public ActionResult Index()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();            
            List<TekilHizmetler> serviceModels = context.TekilHizmetlers.ToList();
            List<TBLEMPLOYEE> employeeModels = context.TBLEMPLOYEEs.ToList();
            List<PaketHizmet> packageModels = context.PaketHizmets.ToList();

            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(pageModel);

            IndexViewModel indexModel = new IndexViewModel
            {
                PageModel = pageModel,
                ServiceModels = serviceModels,
                EmployeeModels = employeeModels,
                PackageModels = packageModels,
                TimeModel = timeModel
            };         

            

            return View(indexModel);
        }
        
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(TBLUSER tbluser)
        {
            if (ModelState.IsValid) // ModelState kontrolü eklenmiştir
            {
                if (context.TBLUSERs.Any(x => x.Username == tbluser.Username))
                {
                    ViewBag.NotificationUsername = "Kullanıcı ismi mevcut. Farklı bir kullanıcı ismi deneyiniz.";
                    return View(tbluser);

                }

                else
                {
                    tbluser.CreationDate = DateTime.Now;
                    tbluser.RoleID = 2;
                    tbluser.UserImageURL = null;
                    context.TBLUSERs.Add(tbluser);
                    context.SaveChanges();

                    Session["UserIDSS"] = tbluser.UserID.ToString();
                    Session["UsernameSS"] = tbluser.Username.ToString();
                    ViewBag.Success = "Kayıt Başarılı!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                // ModelState geçerli değilse, validation hatası var demektir.
                // Hata mesajları kullanıcıya gösterilebilir veya istediğiniz şekilde ele alınabilir.
                return View(tbluser);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBLUSER tbluser)
        {
            var checkLogin = context.TBLUSERs.Where(x => x.Username.Equals(tbluser.Username) && x.Password.Equals(tbluser.Password) && x.RoleID == 2).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["UsernameSS"] = tbluser.Username.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
            }
            return View();
        }

        public ActionResult About()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();
            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(pageModel);

            IndexViewModel aboutModel = new IndexViewModel
            {
                PageModel = pageModel,
                TimeModel = timeModel
            };

            
            return View(aboutModel);
        }

        public ActionResult Service()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();
            List<TekilHizmetler> serviceModels = context.TekilHizmetlers.ToList();
            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(pageModel);
            IndexViewModel serviceModel = new IndexViewModel
            {
                PageModel = pageModel,
                ServiceModels = serviceModels,
                TimeModel = timeModel
            };

            return View(serviceModel);
        }

        public ActionResult Employee()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();
            List<TBLEMPLOYEE> employeeModels = context.TBLEMPLOYEEs.ToList();
            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(pageModel);
            IndexViewModel employeeModel = new IndexViewModel
            {
                PageModel = pageModel,
                EmployeeModels = employeeModels,
                TimeModel = timeModel
            };

       
            return View(employeeModel);
        }

        
    }
}