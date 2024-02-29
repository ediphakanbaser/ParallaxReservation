using Parallax.Helpers;
using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
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

            string usernameFromSession = (string)Session["UsernameSS"];

            // Çift tırnakları kaldırmak için Replace fonksiyonu kullanılıyor
            string cleanedUsername = usernameFromSession.Replace("\"", "");
            List<TBLRESERVATION> tblReservation = context.TBLRESERVATIONs
       .Where(r => r.TBLUSER.Username == cleanedUsername)
       .ToList();

            ReservationViewModel reservationModel = new ReservationViewModel
            {
                ServiceTypes = serviceType,
                TBLSERVICEs = tblServices,
                EmpServices = empService,
                TimeModel = timeModel,
                ReservationModel = tblReservation
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

                TimeViewModel timeModel = TimeModelHelper.GetTimeModel(context.TBLPAGEs.FirstOrDefault());

                var result = (from e in context.TBLEMPLOYEEs
                              join s in context.SKILLS on e.EmployeeID equals s.EmployeeID
                              where s.ServiceID == selectedServiceId
                              select new
                              {
                                  s.EmployeeServiceID,
                                  e.EmployeeID,
                                  e.EmpName,
                                  e.EmpSurname,
                                  e.EmpImageURL
                              }).ToList();

                List<EmployeeTimeSlots> employeeTimeSlotsList = new List<EmployeeTimeSlots>();
                List<int> finalTimeSlotsCounts = new List<int>();
                foreach (var employee in result)
                {
                    Console.WriteLine($"EmployeeID: {employee.EmployeeID}, EmpName: {employee.EmpName}, EmpSurname: {employee.EmpSurname}, EmpImageURL: {employee.EmpImageURL}");
                    EmployeeTimeSlots employeeTimeSlots = new EmployeeTimeSlots

                    {
                        EmployeeID = employee.EmployeeID,
                        EmployeeServiceID = employee.EmployeeServiceID,
                        ReservedTimeSlots = new List<string>(),
                        FinalTimeSlots = new List<string>(),
                    };

                    var reservations = context.TBLRESERVATIONs
                        .Where(r => r.SKILL.EmployeeID == employee.EmployeeID && DbFunctions.TruncateTime(r.ReserveDateTime.Value) == parsedDate.Date)
                        .OrderBy(r => r.ReserveDateTime)
                        .Select(r => new { r.ReserveDateTime, r.ServiceEndDateTime })
                        .ToList();

                    foreach (var reservation in reservations)
                    {
                        string reservationString = reservation.ReserveDateTime.Value.ToString("HH:mm") + " - " + reservation.ServiceEndDateTime.Value.ToString("HH:mm");
                        employeeTimeSlots.ReservedTimeSlots.Add(reservationString);
                    }

                    List<string> timeSlots = GenerateTimeSlots(
                        DateTime.Parse(timeModel.FormattedWorkStartTime),
                        DateTime.Parse(timeModel.FormattedBreakStartTime),
                        DateTime.Parse(timeModel.FormattedBreakEndTime),
                        DateTime.Parse(timeModel.FormattedWorkEndTime),
                        TimeSpan.Parse(selectedTimeSpent),
                        employeeTimeSlots.ReservedTimeSlots);

                    employeeTimeSlots.FinalTimeSlots.AddRange(timeSlots);
                    finalTimeSlotsCounts.Add(employeeTimeSlots.FinalTimeSlots.Count);
                    employeeTimeSlotsList.Add(employeeTimeSlots);
                }

                return Json(new { success = true, employees = result, timeSlotsList = employeeTimeSlotsList, finalTimeSlotsCounts, timeModel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.ToString() });
            }
        }

        private List<string> GenerateTimeSlots(DateTime workStartTime, DateTime breakStartTime, DateTime breakEndTime, DateTime workEndTime, TimeSpan selectedTimeSpent, List<string> reservedTimeSlots)
        {
            List<string> timeSlots = new List<string>();
            DateTime slotStartTime = workStartTime;
            DateTime slotEndTime = slotStartTime.Add(selectedTimeSpent);

            while (IsWorkTime(workStartTime, workEndTime, slotStartTime, slotEndTime))
            {
                if (IsOnBreak(breakStartTime, breakEndTime, slotStartTime, slotEndTime))
                {
                    slotStartTime = breakEndTime;
                    slotEndTime = slotStartTime.Add(selectedTimeSpent);
                }
                else
                {
                    if (reservedTimeSlots.Count != 0)
                    {
                        foreach (string reservedTimeSlot in reservedTimeSlots)
                        {
                            string[] reservedTimeParts = reservedTimeSlot.Split('-');
                            DateTime reservedStartTime = DateTime.ParseExact(reservedTimeParts[0].Trim(), "HH:mm", CultureInfo.InvariantCulture);
                            DateTime reservedEndTime = DateTime.ParseExact(reservedTimeParts[1].Trim(), "HH:mm", CultureInfo.InvariantCulture);

                            if (IsConflict(reservedStartTime, slotStartTime, slotEndTime, reservedEndTime))
                            {
                                slotStartTime = reservedEndTime;
                                slotEndTime = slotStartTime.Add(selectedTimeSpent);
                            }
                        }
                    }
                    if (IsWorkTime(workStartTime, workEndTime, slotStartTime, slotEndTime))
                    {
                        string timeSlot = slotStartTime.ToString("HH:mm") + " - " + slotEndTime.ToString("HH:mm");
                        timeSlots.Add(timeSlot);
                        slotStartTime = slotStartTime.AddMinutes(15);
                        slotEndTime = slotEndTime.AddMinutes(15);
                    }
                }
            }
            return timeSlots;
        }

        private bool IsConflict(DateTime reservedStartTime, DateTime slotStartTime, DateTime slotEndTime, DateTime reservedEndTime)
        {
            return reservedStartTime < slotEndTime && slotEndTime <= reservedEndTime ||
                   reservedStartTime <= slotStartTime && slotStartTime < reservedEndTime ||
                   slotStartTime <= reservedStartTime && reservedEndTime <= slotEndTime ||
                   slotStartTime <= reservedStartTime && reservedEndTime <= slotEndTime;
        }

        private bool IsOnBreak(DateTime breakStartTime, DateTime breakEndTime, DateTime slotStartTime, DateTime slotEndTime)
        {
            return (breakStartTime < slotEndTime && slotEndTime <= breakEndTime) ||
                   (breakStartTime <= slotStartTime && slotStartTime < breakEndTime) ||
                   (slotStartTime >= breakStartTime && slotEndTime <= breakEndTime) ||
                   (slotStartTime <= breakStartTime && slotEndTime >= breakEndTime);
        }

        private bool IsWorkTime(DateTime workStartTime, DateTime workEndTime, DateTime slotStartTime, DateTime slotEndTime)
        {
            return workStartTime <= slotStartTime && slotEndTime <= workEndTime;
        }

        [HttpPost]
        public JsonResult SaveReservation(string username, string reserveDate, int employeeServiceId, string reserveTime, string serviceTime)
        {
            try
            {
                int userId = GetUserId(username);
                bool isUserId = userId > 0 ? true : false;

                if (isUserId)
                {
                    var serverTime = GetServerTime();
                    DateTime reserveDateTime = DateTime.ParseExact(reserveDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    string reserverTimeTrimmed = reserveTime.Trim();
                    string combinedReserveTime = $"{reserveDate} {reserverTimeTrimmed}";
                    string serviceTimeTrimmed = serviceTime.Trim();
                    string combinedServiceTime = $"{reserveDate} {serviceTimeTrimmed}";

                    DateTime serviceTimeParsed = DateTime.ParseExact(combinedServiceTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    DateTime reserveTimeParsed = DateTime.ParseExact(combinedReserveTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

                    var reservation = new TBLRESERVATION
                    {
                        UserID = userId,
                        EmployeeServiceID = employeeServiceId,
                        RecordDateTime = serverTime,
                        ReserveDateTime = reserveTimeParsed,
                        ServiceEndDateTime = serviceTimeParsed
                    };

                    context.TBLRESERVATIONs.Add(reservation);
                    context.SaveChanges();

                    return Json(new { success = true, message = "Rezervasyon başarıyla kaydedildi." });
                }
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bir hata oluştu: " + ex.Message });
            }
        }

        public int GetUserId(string username)
        {
            try
            {
                var user = context.TBLUSERs.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    return user.UserID;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
                return -1;
            }
        }

        public DateTime GetServerTime()
        {
            DateTime serverTime = DateTime.UtcNow.AddHours(3);
            return serverTime;
        }

        [HttpPost]
        public ActionResult DeleteReservation(int reservationID)
        {
            try
            {
                var reservation = context.TBLRESERVATIONs.FirstOrDefault(e => e.ReservationID == reservationID);
                if (reservation != null)
                {
                    context.TBLRESERVATIONs.Remove(reservation);
                    context.SaveChanges();
                    return Json(new { success = true, message = "Rezervasyon başarıyla iptal edildi." });
                }
                // Örneğin, rezervasyonu veritabanından silmek gibi

                // Başarılı bir şekilde işlem yapıldığını belirten bir mesaj dönebilirsiniz
                return Json(new { success = false, message = "Rezervasyon bulunamadı." });
            }
            catch (Exception ex)
            {
                // Hata durumunda kullanıcıya bilgi verin
                return Json(new { success = false, message = "Rezervasyon iptali sırasında bir hata oluştu." });
            }
        }
    }
}