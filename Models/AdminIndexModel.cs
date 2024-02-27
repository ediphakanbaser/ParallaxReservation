using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Parallax.Models
{
    public class AdminIndexModel
    {
        public int YorumSayisi { get; set; }
        public int MemnunSayisi { get; set; }
        public int MemnunOlmayanSayisi { get; set; }
        public int MusteriSayisi { get; set; }
        public int CalisanSayisi { get; set; }
        public decimal CalisanAylıkMaasToplam { get; set; }
        public int HizmetSayisi { get; set; }
        public int TekilHizmetSayisi { get; set; }
        public int PaketHizmetSayisi { get; set; }

    }
}