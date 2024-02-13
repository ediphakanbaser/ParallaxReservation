using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class EmployeeTimeSlots
    {
        public int EmployeeID { get; set; }
        public List<string> ReservedTimeSlots { get; set; }
        public List<string> FinalTimeSlots { get; set; }
    }
}