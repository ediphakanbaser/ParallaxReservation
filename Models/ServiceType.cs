namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ServiceType
    {
        [Key]
        [Column(Order = 0)]
        public decimal ServicePrice { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string ServiceName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string TypeName { get; set; }

        public decimal? DiscountedPrice { get; set; }

        [Key]
        [Column(Order = 3)]
        public TimeSpan TimeSpent { get; set; }
    }
}
