using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class EmployeeViewModel
    {
        public HttpPostedFileBase EmpImage { get; set; }
        public short EmpID { get; set; }
        public string EmpImageURL { get; set; }
        public string EmpName { get; set; }
        public string EmpSurname { get; set; }
        public string EmpPhone { get; set; }
        public string EmpMail { get; set; }
        public decimal EmpSalary { get; set; }
        public DateTime EmpStart { get; set; }
        public DateTime EmpEnd { get; set; }

        // Skills özelliği
        public virtual ICollection<SKILL> SKILLS { get; set; }
    }
}