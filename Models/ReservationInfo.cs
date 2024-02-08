namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReservationInfo")]
    public partial class ReservationInfo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(100)]
        public string EmpImageURL { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(55)]
        public string EmpFullName { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime EmpStardDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmpDismissalDate { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(30)]
        public string ServiceName { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Username { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(55)]
        public string UserFullName { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime ReserveDateTime { get; set; }

        [Key]
        [Column(Order = 7)]
        public TimeSpan TimeSpent { get; set; }

        public TimeSpan? CompletionDateTime { get; set; }
    }
}
