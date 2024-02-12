using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class IndexViewModel
    {
        public TBLPAGE PageModel { get; set; }
        public List<TekilHizmetler> ServiceModels { get; set; }
        public List<TBLEMPLOYEE> EmployeeModels { get; set; }
        public List<PaketHizmet> PackageModels { get; set; }
        public TimeViewModel TimeModel { get; set; }
    }

}