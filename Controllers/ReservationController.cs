using Parallax.Helpers;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Parallax.Controllers
{
    [Route("Reservation")]
    public class ReservationController : Controller
    {
        private readonly ParallaxContext context;

        public ReservationController()
        {
            context = new ParallaxContext();
        }

        public ActionResult Reservation()
        {
            TBLPAGE pageModel = context.TBLPAGEs.FirstOrDefault();
            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(pageModel);
            List<ServiceType> serviceType = context.ServiceTypes.ToList();
            List<TBLSERVICE> tblServices = context.TBLSERVICEs.ToList();
            List<EmpService> empService = context.EmpServices.ToList();
            
            

            // RemoveReservedTimeSlots fonksiyonunu çağırarak rezerve edilen zaman dilimlerini çıkar
            
            
            ReservationViewModel reservationModel = new ReservationViewModel
            {
                ServiceTypes = serviceType,
                TBLSERVICEs = tblServices,
                EmpServices = empService,
                TimeModel = timeModel
            };

            return View(reservationModel);
        }
        
        [HttpPost]
        public ActionResult Reservation(string selectedDateTime, int selectedServiceId, string selectedTimeSpent)
        {
            try
            {
                if (!DateTime.TryParseExact(selectedDateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return Json(new { success = false, message = "Geçersiz tarih formatı. Lütfen yyyy-MM-dd formatında giriniz." });
                }

                // Zaman modelini al
                TimeViewModel timeModel = TimeModelHelper.GetTimeModel(context.TBLPAGEs.FirstOrDefault());

                // Dönüşümü gerçekleştir
                
               

                // Kullanıcının seçtiği tarih ve hizmete uygun olan çalışanları bulmak için sorgu yapılır.
                var result = (from e in context.TBLEMPLOYEEs
                              join s in context.SKILLS on e.EmployeeID equals s.EmployeeID
                              where s.ServiceID == selectedServiceId
                              select new
                              {
                                  e.EmployeeID,
                                  e.EmpName,
                                  e.EmpSurname,
                                  e.EmpImageURL
                              }).ToList();


                List<string> reservedTimeSlots = new List<string>();
                // Her bir employe için rezervasyonları kontrol et
                foreach (var employee in result)
                {
                    var reservations = context.TBLRESERVATIONs
                        .Where(r => r.EmployeeServiceID == employee.EmployeeID && DbFunctions.TruncateTime(r.ReserveDateTime.Value) == parsedDate.Date)
                        .Select(r => new { r.ReserveDateTime, r.ServiceEndDateTime })
                        .ToList();

                    foreach (var reservation in reservations)
                    {
                        // Rezervasyon başlangıç ve bitiş saatlerini liste halinde ekleyin
                        if (reservation.ReserveDateTime != null && reservation.ServiceEndDateTime != null)
                        {
                            reservedTimeSlots.Add(reservation.ReserveDateTime.Value.ToString("HH:mm") + " - " + reservation.ServiceEndDateTime.Value.ToString("HH:mm"));
                        }
                    }
                }
                
                    List<string> timeSlots = GenerateTimeSlots(
                    timeModel,
                    DateTime.Parse(timeModel.FormattedWorkStartTime),
                    DateTime.Parse(timeModel.FormattedBreakStartTime),
                    DateTime.Parse(timeModel.FormattedBreakEndTime),
                    DateTime.Parse(timeModel.FormattedWorkEndTime),
                    TimeSpan.Parse(selectedTimeSpent),
                    reservedTimeSlots
                );
                
                
                // GenerateTimeSlots fonksiyonunu çağırarak zaman dilimlerini oluştur
                
                

                // RemoveReservedTimeSlots fonksiyonunu çağırarak rezerve edilen zaman dilimlerini çıkar
                List<string> finalTimeSlots = RemoveReservedTimeSlots(timeSlots, reservedTimeSlots);

               
                // AJAX isteği sonucunu JSON olarak döner.
                return Json(new { success = true, employees = result, reservedTimeSlots, finalTimeSlots, timeModel});
            }
            catch (Exception ex)
            {
                // Hata durumunda JSON olarak hata mesajını döner.
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GenerateTimeSlots fonksiyonunu TimeViewModel parametresi ile güncelle
        private List<string> GenerateTimeSlots(TimeViewModel timeModel, DateTime workStartTime, DateTime breakStartTime, DateTime breakEndTime, DateTime workEndTime, TimeSpan selectedTimeSpent, List<string> reservedTimeSlots)
        {
            List<string> timeSlots = new List<string>();
            DateTime currentTime = workStartTime;
            TimeSpan timeSpent = selectedTimeSpent; // ViewModelTimeSpent'i burada al

            while (currentTime < workEndTime)
            {
                // Belirli aralıklarla zaman dilimlerini oluştur
                string timeSlot = currentTime.ToString("HH:mm") + " - " + currentTime.Add(timeSpent).ToString("HH:mm");

                // Eğer oluşturulan zaman dilimi rezerve edilmemişse ve çalışma saatleri içerisinde ise listeye ekle
                if (!reservedTimeSlots.Contains(timeSlot) && currentTime >= workStartTime && currentTime.Add(timeSpent) <= workEndTime)
                {
                    timeSlots.Add(timeSlot);
                    currentTime = currentTime.AddMinutes(15);
                }
                else
                {
                    // Mola başlangıcı ve bitiş arasında ise mola zamanını atla
                    if (currentTime >= breakStartTime && currentTime.Add(timeSpent) <= breakEndTime)
                    {
                        currentTime = currentTime.AddMinutes(60); // Mola süresini 15 dakika olarak kabul ettim, bu süreyi istediğiniz gibi değiştirebilirsiniz.
                    }
                    else
                    {
                        currentTime = currentTime.AddMinutes(15);
                    }
                }
            }

            return timeSlots;
        }



        private List<string> RemoveReservedTimeSlots(List<string> timeSlots, List<string> reservedTimeSlots)
        {
            return timeSlots.Except(reservedTimeSlots).ToList();
        }
    }
}
