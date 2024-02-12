using Parallax.Helpers;
using Parallax.Models;
using System;
using System.Collections.Generic;
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
            List<ServiceType> serviceType = context.ServiceTypes.ToList();
            List<TBLSERVICE> tblServices = context.TBLSERVICEs.ToList();
            List<EmpService> empService = context.EmpServices.ToList();
            List<string> timeSlots = GenerateTimeSlots();
            ViewBag.TimeSlots = timeSlots;
            TimeViewModel timeModel = TimeModelHelper.GetTimeModel(pageModel);


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
        public ActionResult Reservation(string selectedDateTime, int selectedServiceId)
        {
            try
            {
                if (!DateTime.TryParseExact(selectedDateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                {
                    return Json(new { success = false, message = "Geçersiz tarih formatı. Lütfen yyyy-MM-dd formatında giriniz." });
                }

                // Kullanıcının seçtiği tarih ve hizmete uygun olan çalışanları bulmak için sorgu yapılır.
                var result = (from e in context.TBLEMPLOYEEs
                              join s in context.SKILLS on e.EmployeeID equals s.EmployeeID
                              where s.ServiceID == selectedServiceId
                              select new
                              {
                                  e.EmpName,
                                  e.EmpSurname,
                                  e.EmpImageURL
                              }).ToList();


                //// Rezerve edilmiş çalışanları kontrol etmek için bir sorgu daha yapılır.
                //var reservedEmployees = (from r in context.TBLRESERVATIONs
                //                         join e in context.TBLEMPLOYEEs on e.EmployeeID equals r.EmployeeID
                //    .Where(reservation => reservation.ReserveDateTime == parsedDate)
                //    .Select(reservation => reservation.SKILL.TBLEMPLOYEE)
                //    ).ToList();

                //// Rezerve edilmemiş çalışanlar elde edilir.
                //var finalAvailableEmployees = result.Except(reservedEmployees).ToList();

                // AJAX isteği sonucunu JSON olarak döner.
                return Json(new { success = true, employees = result });
            }
            catch (Exception ex)
            {
                // Hata durumunda JSON olarak hata mesajını döner.
                return Json(new { success = false, message = ex.Message });
            }
        }
        private List<string> GenerateTimeSlots()
        {
            List<string> timeSlots = new List<string>();
            DateTime startTime = DateTime.Parse("08:00");
            DateTime endTime = DateTime.Parse("17:00");

            while (startTime < endTime)
            {
                timeSlots.Add(startTime.ToString("HH:mm") + " - " + startTime.AddMinutes(15).ToString("HH:mm"));
                startTime = startTime.AddMinutes(15);
            }

            return timeSlots;
        }


    }
}
