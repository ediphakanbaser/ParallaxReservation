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

                TimeViewModel timeModel = TimeModelHelper.GetTimeModel(context.TBLPAGEs.FirstOrDefault());

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

                List<EmployeeTimeSlots> employeeTimeSlotsList = new List<EmployeeTimeSlots>();
                List<int> finalTimeSlotsCounts = new List<int>();
                foreach (var employee in result)
                {
                    Console.WriteLine($"EmployeeID: {employee.EmployeeID}, EmpName: {employee.EmpName}, EmpSurname: {employee.EmpSurname}, EmpImageURL: {employee.EmpImageURL}");
                    EmployeeTimeSlots employeeTimeSlots = new EmployeeTimeSlots
                    {
                        EmployeeID = employee.EmployeeID,
                        ReservedTimeSlots = new List<string>(),
                        FinalTimeSlots = new List<string>(),
                        
                    };

                    var reservations = context.TBLRESERVATIONs
                        .Where(r => r.SKILL.EmployeeID == employee.EmployeeID && DbFunctions.TruncateTime(r.ReserveDateTime.Value) == parsedDate.Date)
                        .Select(r => new { r.ReserveDateTime, r.ServiceEndDateTime })
                        .ToList();

                    foreach (var reservation in reservations)
                    {
                        string reservationString = reservation.ReserveDateTime.Value.ToString("HH:mm") + " - " + reservation.ServiceEndDateTime.Value.ToString("HH:mm");
                        employeeTimeSlots.ReservedTimeSlots.Add(reservationString);
                    }

                    List<string> timeSlots = GenerateTimeSlots(
                        timeModel,
                        DateTime.Parse(timeModel.FormattedWorkStartTime),
                        DateTime.Parse(timeModel.FormattedBreakStartTime),
                        DateTime.Parse(timeModel.FormattedBreakEndTime),
                        DateTime.Parse(timeModel.FormattedWorkEndTime),
                        TimeSpan.Parse(selectedTimeSpent),
                        employeeTimeSlots.ReservedTimeSlots
                    );

                    List<string> finalTimeSlots = RemoveReservedTimeSlots(timeSlots, employeeTimeSlots.ReservedTimeSlots);
                    employeeTimeSlots.FinalTimeSlots.AddRange(finalTimeSlots);
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


        // GenerateTimeSlots fonksiyonunu TimeViewModel parametresi ile güncelle
        private List<string> GenerateTimeSlots(TimeViewModel timeModel, DateTime workStartTime, DateTime breakStartTime, DateTime breakEndTime, DateTime workEndTime, TimeSpan selectedTimeSpent, List<string> reservedTimeSlots)
        {
            List<string> timeSlots = new List<string>();
            TimeSpan breakTime = breakEndTime - breakStartTime;
            int breakT = (int)breakTime.TotalMinutes;
            DateTime currentTime = workStartTime;
            TimeSpan timeSpent = selectedTimeSpent; // ViewModelTimeSpent'i burada al
            DateTime currentTimeSpent = currentTime.Add(timeSpent);

            while (currentTime < workEndTime)
            {
                while ((currentTime < breakStartTime && currentTimeSpent > breakStartTime) || (currentTime >= breakStartTime && currentTimeSpent < breakEndTime) || (currentTime < breakEndTime && currentTimeSpent >= breakEndTime))

                {
                    currentTime = breakEndTime;
                    currentTimeSpent = currentTime.Add(timeSpent);
                }
                if (currentTimeSpent <= breakStartTime || (currentTimeSpent > breakEndTime && currentTimeSpent <= workEndTime))
                {
                    if (reservedTimeSlots.Count != 0)
                    {
                        foreach (string reservedTimeSlot in reservedTimeSlots)
                        {
                            string[] reservedTimeParts = reservedTimeSlot.Split('-');
                            DateTime reservedStartTime = DateTime.Parse(reservedTimeParts[0].Trim());
                            DateTime reservedEndTime = DateTime.Parse(reservedTimeParts[1].Trim());

                            if ((currentTime <= reservedStartTime && reservedStartTime < currentTimeSpent) || (currentTime < reservedEndTime && reservedEndTime <= currentTimeSpent))
                            {
                                currentTime = currentTime.AddMinutes(15);
                                currentTimeSpent = currentTimeSpent.AddMinutes(15);
                            }
                            else
                            {
                                string timeSlot = currentTime.ToString("HH:mm") + " - " + currentTimeSpent.ToString("HH:mm");
                                timeSlots.Add(timeSlot);
                                currentTime = currentTime.AddMinutes(15);
                                currentTimeSpent = currentTimeSpent.AddMinutes(15);
                            }
                        }

                    }
                    else
                    {
                        string timeSlot = currentTime.ToString("HH:mm") + " - " + currentTimeSpent.ToString("HH:mm");
                        timeSlots.Add(timeSlot);
                        currentTime = currentTime.AddMinutes(15);
                        currentTimeSpent = currentTimeSpent.AddMinutes(15);
                    }
                }
                else
                {
                    currentTime = currentTime.AddMinutes(15);
                    currentTimeSpent = currentTimeSpent.AddMinutes(15);
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