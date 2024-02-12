namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TBLRESERVATION")]
    public partial class TBLRESERVATION
    {
        [Key]
        public int ReservationID { get; set; }

        public int UserID { get; set; }

        public int EmployeeServiceID { get; set; }

        public DateTime? RecordDateTime { get; set; }

        public DateTime? ReserveDateTime { get; set; }

        public DateTime? ServiceEndDateTime { get; set; }

        public int? ReviewID { get; set; }

        public bool? ReviewStatus { get; set; }

        public virtual SKILL SKILL { get; set; }

        public virtual TBLREVIEW TBLREVIEW { get; set; }

        public virtual TBLUSER TBLUSER { get; set; }
    }
}
