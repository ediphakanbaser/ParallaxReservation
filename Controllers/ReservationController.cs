using Parallax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace Parallax.Controllers
{
    public class ReservationController : Controller
    {
        private ParallaxContext context;

        public ReservationController()
        {
            context = new ParallaxContext();

        }
        public ActionResult Reservation()
        {

            List<ServiceType> serviceType = context.ServiceTypes.ToList();
            List<TBLSERVICE> tblServices = context.TBLSERVICEs.ToList();
            List<EmpService> empService = context.EmpServices.ToList();

            ReservationViewModel reservationModel = new ReservationViewModel
            {
                ServiceTypes = serviceType,
                TBLSERVICEs = tblServices,
                EmpServices = empService

            };
            return View(reservationModel);
        }
        
    }
}
