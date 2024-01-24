namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EmpService")]
    public partial class EmpService
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string EmpName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string EmpSurname { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string ServiceName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string TypeName { get; set; }

        [Key]
        [Column(Order = 4)]
        public TimeSpan TimeSpent { get; set; }

        public decimal? DiscountedPrice { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmpDismissalDate { get; set; }
    }
}
