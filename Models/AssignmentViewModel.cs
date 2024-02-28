using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class AssignmentViewModel
    {
        public List<TBLEMPLOYEE> EmployeeModel { get; set; }
        public List<TBLSERVICE> ServiceModel { get; set; }
        public List<SKILL> SkillModel { get; set; }
    }
}