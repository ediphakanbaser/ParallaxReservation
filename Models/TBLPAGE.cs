namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TBLPAGE")]
    public partial class TBLPAGE
    {
        [Key]
        [Column(Order = 0, TypeName = "text")]
        public string AboutText { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string OverlayImageURL { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string OverlayLogoURL { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string DiscountImageURL { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string DiscountTitle { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "text")]
        public string DiscountParagraph { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string DiscountAmount { get; set; }

        
        [Column(Order = 7)]
        public TimeSpan WorkStartTime { get; set; }

        [Column(Order = 8)]
        public TimeSpan BreakStartTime { get; set; }

        [Column(Order = 9)]
        public TimeSpan BreakEndTime { get; set; }

        [Column(Order = 10)]
        public TimeSpan WorkEndTime { get; set; }
    }
}
