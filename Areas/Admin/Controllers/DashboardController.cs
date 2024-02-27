using Parallax.Areas.Admin.Attributes;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace Parallax.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ParallaxContext context;

        public DashboardController()
        {
            context = new ParallaxContext();
        }
        // GET: Admin/Dashboard
        [CustomAuthorize]
        public ActionResult Index()
        {

            var model = new AdminIndexModel
            {
                YorumSayisi = context.TBLREVIEWs.Count(),
                MemnunSayisi = context.TBLREVIEWs.Count(r => r.Rating >= 3.5),
                MemnunOlmayanSayisi = context.TBLREVIEWs.Count(r => r.Rating < 2.5),
                MusteriSayisi = context.TBLUSERs.Count(r => r.RoleID == 2),
                CalisanSayisi = context.TBLEMPLOYEEs.Count(),
                CalisanAylıkMaasToplam = context.TBLEMPLOYEEs.Sum(c => c.EmpSalary),
                HizmetSayisi = context.TBLSERVICEs.Count(),
                TekilHizmetSayisi = context.TBLSERVICEs.Count(h => h.TYPE.TypeID == 1),
                PaketHizmetSayisi = context.TBLSERVICEs.Count(h => h.TYPE.TypeID == 2)
            };
            return View(model);
        }

        [CustomAuthorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Login");
        }

        [CustomAuthorize]
        public ActionResult ParallaxPanel()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();
            return View(pageModel);
        }

        [CustomAuthorize]
        [HttpPost]
        public ActionResult UpdateAboutText(string aboutText)
        {
            try
            {
                var pageModel = context.TBLPAGEs.FirstOrDefault();

                if (pageModel != null)
                {
                    pageModel.AboutText = aboutText;
                    context.SaveChanges();

                    return Json(new { success = true, message = "AboutText güncellendi" });
                }
                else
                {
                    return Json(new { success = false, message = "AboutText güncellenirken hata oluştu" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"AboutText güncelleme hatası: {ex.Message}" });
            }
        }

        [HttpPost]
        public ActionResult UpdateShift(string field, string timeSpanValue)
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();

            try
            {
                if (TimeSpan.TryParse(timeSpanValue, out TimeSpan timeSpan))
                {
                    // Burada field değerine göre hangi alanın güncelleneceğini belirleyebilir ve value değeri ile güncelleme işlemini gerçekleştirebilirsin
                    switch (field)
                    {
                        case "mesaiBaslangic":
                            pageModel.WorkStartTime = timeSpan;
                            break;
                        case "molaBaslangic":
                            pageModel.BreakStartTime = timeSpan;
                            break;
                        case "molaBitis":
                            pageModel.BreakEndTime = timeSpan;
                            break;
                        case "mesaiBitis":
                            pageModel.WorkEndTime = timeSpan;
                            break;
                        default:
                            return Json(new { success = false, message = "Geçersiz alan" });
                    }

                    // Değişiklikleri kaydet
                    context.SaveChanges();

                    // Başarı durumunda JSON yanıtı döndür
                    return Json(new { success = true, message = $"{field} güncellendi" });
                }
                else
                {
                    return Json(new { success = false, message = "Geçersiz zaman formatı" });
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda JSON yanıtı döndür
                return Json(new { success = false, message = $"Shift güncelleme hatası: {ex.Message}" });
            }
        }



        [HttpPost]
        public ActionResult UpdateOverlayImage(HttpPostedFileBase image)
        {
            return UpdateImage("bg_1.png", image);
        }

        [HttpPost]
        public ActionResult UpdateLogoImage(HttpPostedFileBase image)
        {
            return UpdateImage("parallax.svg", image);
        }

        [HttpPost]
        public ActionResult UpdateDiscountImage(HttpPostedFileBase image)
        {
            return UpdateImage("bg_7.png", image);
        }

        private ActionResult UpdateImage(string oldFileName, HttpPostedFileBase image)
        {
            try
            {
                if (image != null && image.ContentLength > 0)
                {
                    // Eski dosyanın yolunu belirle
                    var oldImagePath = Path.Combine(Server.MapPath("~/Content/images"), oldFileName);

                    // Eski dosyayı oldimages klasörüne taşı
                    var oldImagesPath = Path.Combine(Server.MapPath("~/Content/images/oldimages"), $"{Guid.NewGuid()}-{oldFileName}");
                    System.IO.File.Move(oldImagePath, oldImagesPath);

                    // Yeni bir dosya adı oluştur
                    var uniqueFileName = oldFileName;

                    // Dosya yolunu belirle
                    var imagePath = Path.Combine(Server.MapPath("~/Content/images"), uniqueFileName);

                    // Görseli kaydet
                    image.SaveAs(imagePath);

                    // Görselin URL'sini döndür
                    var imageUrl = Url.Content($"~/Content/Images/{uniqueFileName}");

                    // Bitiş logu
                    System.Diagnostics.Debug.WriteLine($"UpdateImage metodu başarıyla tamamlandı: {uniqueFileName}");

                    return Json(new { success = true, message = $"{oldFileName} güncellendi", imageUrl });
                }
                else
                {
                    return Json(new { success = false, message = "Lütfen geçerli bir görsel seçin" });
                }
            }
            catch (Exception ex)
            {
                // Hata logu
                System.Diagnostics.Debug.WriteLine($"UpdateImage metodu hata: {ex.Message}");

                return Json(new { success = false, message = $"{oldFileName} güncelleme hatası: {ex.Message}" });
            }
        }


        [HttpPost]
        public ActionResult UpdateDiscount(string field, string value)
        {
            try
            {
                var pageModel = context.TBLPAGEs.FirstOrDefault();

                if (pageModel != null)
                {
                    switch (field)
                    {
                        case "title":
                            pageModel.DiscountTitle = value;
                            break;
                        case "paragraph":
                            pageModel.DiscountParagraph = value;
                            break;
                        case "amount":
                            pageModel.DiscountAmount = value;
                            break;
                        default:
                            return Json(new { success = false, message = "Geçersiz alan" });
                    }

                    context.SaveChanges();

                    return Json(new { success = true, message = "Discount alanı güncellendi" });
                }
                else
                {
                    return Json(new { success = false, message = "Sayfa modeli bulunamadı" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Discount güncelleme hatası: {ex.Message}" });
            }
        }


        [CustomAuthorize]
        public ActionResult Employee()
        {
            List<TBLEMPLOYEE> employeeModel = context.TBLEMPLOYEEs.ToList();
            return View(employeeModel);
        }

        [HttpPost]
        public ActionResult AddEmployee(TBLEMPLOYEE model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employee = new TBLEMPLOYEE()
                    {
                        EmpName = model.Name,
                        EmpSurname = model.Surname,
                        EmpPhone = model.Phone,
                        EmpMail = model.Email,
                        EmpStardDate = model.StartDate,
                        EmpDismissalDate = model.EndDate,
                        EmpSalary = model.Salary
                        EmpRecordDateTime =
                        // Diğer özellikleri ekle
                    };

                    _context.Employees.Add(employee);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Çalışan başarıyla eklendi." });
                }
                else
                {
                    return Json(new { success = false, message = "Form doğrulama hatası." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Çalışan eklerken bir hata oluştu: {ex.Message}" });
            }
        }

        [CustomAuthorize]
        public ActionResult Service()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult EmployeeService()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Testimonial()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult Earnings()
        {
            return View();
        }





    }
}