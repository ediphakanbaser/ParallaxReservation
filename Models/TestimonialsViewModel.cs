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

    }
}