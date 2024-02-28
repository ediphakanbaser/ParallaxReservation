using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class ServiceViewModel
    {        
        public HttpPostedFileBase SrvImage { get; set; }
        public int SrvID { get; set; }
        public string SrvImageURL { get; set; }
        public string SrvName { get; set; }
        public byte SrvType { get; set; }
        public string SrvSpent { get; set; }
        public decimal SrvPrice { get; set; }
        public decimal SrvDisc { get; set; }
        public string SrvPrg { get; set; }
        public bool ServiceStatus { get; set; }
    }
}