using Parallax.Areas.Admin.Attributes;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI.WebControls;



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
            DateTime currentDate = DateTime.UtcNow.AddHours(3);
            var model = new AdminIndexModel
            {
                YorumSayisi = context.TBLREVIEWs.Count(),
                MemnunSayisi = context.TBLREVIEWs.Count(r => r.Rating >= 3.5),
                MemnunOlmayanSayisi = context.TBLREVIEWs.Count(r => r.Rating < 2.5),
                MusteriSayisi = context.TBLUSERs.Count(r => r.RoleID == 2),
                CalisanSayisi = context.TBLEMPLOYEEs.Where(item => item.EmpDismissalDate == null || currentDate < item.EmpDismissalDate || item.EmpDismissalDate == DateTime.MinValue).Count(),
                CalisanAylıkMaasToplam = context.TBLEMPLOYEEs.Where(item => item.EmpDismissalDate == null || currentDate < item.EmpDismissalDate || item.EmpDismissalDate == DateTime.MinValue).Sum(c => c.EmpSalary),
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
        public ActionResult AddEmployee(EmployeeViewModel employee)
        {
            try
            {

                if (employee.EmpImage == null)
                {
                    // Eğer resim seçilmemişse default bir değer atayabilirsiniz.
                    employee.EmpImageURL = "/Content/Images/womanavatar.jpg";
                }
                else
                {
                    // Resmi kaydetme işlemi
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(employee.EmpImage.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/Images"), uniqueFileName);

                    // Dosyayı kaydetme işlemi
                    employee.EmpImage.SaveAs(filePath);
                    employee.EmpImageURL = "/Content/Images/" + uniqueFileName;
                }

                // EmployeeViewModel'i TBLEMPLOYEE'ye dönüştürme
                TBLEMPLOYEE employeeEntity = new TBLEMPLOYEE
                {
                    EmpName = employee.EmpName,
                    EmpSurname = employee.EmpSurname,
                    EmpImageURL = employee.EmpImageURL,
                    EmpPhone = employee.EmpPhone,
                    EmpMail = employee.EmpMail,
                    EmpSalary = employee.EmpSalary,
                    EmpRecordDateTime = DateTime.Now,
                    EmpStardDate = employee.EmpStart,
                    EmpDismissalDate = employee.EmpEnd,
                    // Diğer özellikleri ekleyin
                };

                using (context) // YourDbContext sınıfını kullanarak bir bağlantı oluşturun
                {
                    // Veritabanına ekleme işlemi
                    context.TBLEMPLOYEEs.Add(employeeEntity);
                    context.SaveChanges();
                }
                return Json(new { success = true, message = "Çalışan başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ekleme sırasında bir hata oluştu." });
            }
        }

        [HttpGet]
        public ActionResult GetEmployeeInfo(int employeeID)
        {
            // employeeID parametresine göre veritabanından çalışan bilgilerini çek
            var employee = context.TBLEMPLOYEEs.FirstOrDefault(e => e.EmployeeID == employeeID);

            if (employee != null)
            {
                // Ajax isteğine çalışan bilgilerini JSON formatında geri döndür
                return Json(new
                {
                    EmpID = employee.EmployeeID,
                    EmpName = employee.EmpName,
                    EmpSurname = employee.EmpSurname,
                    EmpPhone = employee.EmpPhone,
                    EmpMail = employee.EmpMail,
                    EmpStardDate = employee.EmpStardDate,
                    EmpDismissalDate = employee.EmpDismissalDate,
                    EmpSalary = employee.EmpSalary,
                    EmpImageURL = employee.EmpImageURL
                    // Diğer alanları da ekleyebilirsiniz
                }, JsonRequestBehavior.AllowGet);
            }

            // Eğer çalışan bulunamazsa hata durumu
            return HttpNotFound("Belirtilen ID'ye sahip çalışan bulunamadı.");
        }

        [HttpPost]
        public ActionResult UpdateEmployee(EmployeeViewModel updatedEmployee)
        {
            try
            {
                // Veritabanından güncellenecek çalışanı bul
                var existingEmployee = context.TBLEMPLOYEEs.FirstOrDefault(e => e.EmployeeID == updatedEmployee.EmpID);

                if (existingEmployee != null)
                {
                    // Güncelleme işlemlerini gerçekleştir
                    existingEmployee.EmpName = updatedEmployee.EmpName;
                    existingEmployee.EmpSurname = updatedEmployee.EmpSurname;
                    existingEmployee.EmpPhone = updatedEmployee.EmpPhone;
                    existingEmployee.EmpMail = updatedEmployee.EmpMail;
                    existingEmployee.EmpStardDate = updatedEmployee.EmpStart;

                    // Güncelleme tarihi kontrolü
                    if (updatedEmployee.EmpEnd != null)
                        existingEmployee.EmpDismissalDate = updatedEmployee.EmpEnd;

                    // Resim güncelleme kontrolü
                    if (updatedEmployee.EmpImage != null)
                    {
                        // Yeni resim seçildiyse
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(updatedEmployee.EmpImage.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Content/Images"), uniqueFileName);
                        updatedEmployee.EmpImage.SaveAs(filePath);
                        existingEmployee.EmpImageURL = "/Content/Images/" + uniqueFileName;
                    }

                    // Diğer alanları güncelle

                    // Veritabanına değişiklikleri kaydet
                    context.SaveChanges();

                    return Json(new { success = true, message = "Çalışan başarıyla güncellendi." });
                }

                return Json(new { success = false, message = "Güncellenmek istenen çalışan bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu." });
            }
        }


        [CustomAuthorize]
        public ActionResult Service()
        {
            List<TBLSERVICE> serviceModel = context.TBLSERVICEs.ToList();
            return View(serviceModel);

        }
        [HttpPost]
        public ActionResult AddService(ServiceViewModel service)
        {
            try
            {
                if (service.SrvImage == null)
                {
                    // Eğer resim seçilmemişse default bir değer atayabilirsiniz.
                    service.SrvImageURL = "/Content/Images/womanavatar.jpg";
                }
                else
                {
                    // Resmi kaydetme işlemi
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(service.SrvImage.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Content/Images"), uniqueFileName);

                    // Dosyayı kaydetme işlemi
                    service.SrvImage.SaveAs(filePath);
                    service.SrvImageURL = "/Content/Images/" + uniqueFileName;
                }

                // ServiceViewModel'i TBLSERVICE'ye dönüştürme
                TBLSERVICE serviceEntity = new TBLSERVICE
                {
                    ServiceName = service.SrvName,
                    TypeID = service.SrvType,
                    ServiceImageURL = service.SrvImageURL,
                    TimeSpent = TimeSpan.Parse(service.SrvSpent),
                    ServicePrice = service.SrvPrice,
                    DiscountedPrice = service.SrvDisc,
                    ServiceParagraph = service.SrvPrg,
                    // Diğer özellikleri ekleyin
                };

                using (context) // YourDbContext sınıfını kullanarak bir bağlantı oluşturun
                {
                    // Veritabanına ekleme işlemi
                    context.TBLSERVICEs.Add(serviceEntity);
                    context.SaveChanges();
                }

                return Json(new { success = true, message = "Servis başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ekleme sırasında bir hata oluştu." });
            }
        }

        [HttpGet]
        public ActionResult GetServiceInfo(int serviceID)
        {
            var service = context.TBLSERVICEs.FirstOrDefault(e => e.ServiceID == serviceID);

            if (service != null)
            {
                // Ajax isteğine çalışan bilgilerini JSON formatında geri döndür
                return Json(new
                {
                    SrvID = service.ServiceID,
                    SrvName = service.ServiceName,
                    SrvType = service.TypeID,
                    SrvSpent = service.TimeSpent,
                    SrvPrice = service.ServicePrice,
                    SrvDisc = service.DiscountedPrice,
                    SrvPrg = service.ServiceParagraph,
                    SrvImageURL = service.ServiceImageURL


                }, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound("Belirtilen ID'ye sahip çalışan bulunamadı.");
        }

        [HttpPost]
        public ActionResult ChangeServiceStatus(int serviceID)
        {
            try
            {
                var status = context.TBLSERVICEs.FirstOrDefault(e => e.ServiceID == serviceID);
                if (status != null)
                {
                    status.ServiceStatus = !status.ServiceStatus;
                    context.SaveChanges();
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hata durumunda uygun loglama işlemlerini gerçekleştirin
                return Json(new { success = false, message = "Durum değiştirme sırasında bir hata oluştu." });
            }
        }

        [HttpPost]
        public ActionResult UpdateService(ServiceViewModel updatedService) 
        {
            try
            {
                // Veritabanından güncellenecek çalışanı bul
                var existingService = context.TBLSERVICEs.FirstOrDefault(e => e.ServiceID == updatedService.SrvID);

                if (existingService != null)
                {
                    // Güncelleme işlemlerini gerçekleştir
                    existingService.ServiceName = updatedService.SrvName;
                    existingService.TypeID = updatedService.SrvType;
                    existingService.TimeSpent = TimeSpan.Parse(updatedService.SrvSpent);
                    existingService.ServicePrice = updatedService.SrvPrice;
                    existingService.DiscountedPrice = updatedService.SrvDisc;
                    existingService.ServiceParagraph = updatedService.SrvPrg;
                                       
                   
                    // Resim güncelleme kontrolü
                    if (updatedService.SrvImage != null)
                    {
                        // Yeni resim seçildiyse
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(updatedService.SrvImage.FileName);
                        string filePath = Path.Combine(Server.MapPath("~/Content/Images"), uniqueFileName);
                        updatedService.SrvImage.SaveAs(filePath);
                        existingService.ServiceImageURL = "/Content/Images/" + uniqueFileName;
                    }

                    // Diğer alanları güncelle

                    // Veritabanına değişiklikleri kaydet
                    context.SaveChanges();

                    return Json(new { success = true, message = "Çalışan başarıyla güncellendi." });
                }

                return Json(new { success = false, message = "Güncellenmek istenen çalışan bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu." });
            }
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