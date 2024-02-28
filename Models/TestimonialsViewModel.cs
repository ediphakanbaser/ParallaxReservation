using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class TestimonialsViewModel : LayoutViewModel
    {
        public TBLPAGE PageModel { get; set; }
        public List<TBLRESERVATION> ReservationModels { get; set; }
        public List<TBLREVIEW> ReviewModels { get; set; }
        public List<TBLUSER> UserModels { get; set; }
        public List<TBLSERVICE> ServiceModels { get; set; }

        public List<TBLSERVICE> EmployeeModels { get; set; }

    }
}