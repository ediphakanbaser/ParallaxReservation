using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class ReservationViewModel
    {
        public List<ServiceType> ServiceTypes  { get; set; }
        public List<TBLSERVICE> TBLSERVICEs { get; set; }
        public List<EmpService> EmpServices { get; set; }
        public TBLPAGE PageModel { get; set; }
        public TimeViewModel TimeModel { get; set; }
        public List<TBLRESERVATION> ReservationModel { get; set; }

    }
}