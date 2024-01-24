namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TekilHizmetler")]
    public partial class TekilHizmetler
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string ServiceName { get; set; }

        public decimal? DiscountedPrice { get; set; }

        [StringLength(100)]
        public string ServiceImageURL { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string ServiceParagraph { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte TypeID { get; set; }
    }
}
