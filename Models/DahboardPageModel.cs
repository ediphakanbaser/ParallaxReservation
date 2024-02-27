using System;
using System.ComponentModel.DataAnnotations;

namespace Parallax.Models
{
    public class DahboardPageModel
    {
        public string AboutText { get; set; }

        [StringLength(100)]
        public string OverlayImageURL { get; set; }

        [StringLength(100)]
        public string OverlayLogoURL { get; set; }

        [StringLength(100)]
        public string DiscountImageURL { get; set; }

        [StringLength(50)]
        public string DiscountTitle { get; set; }

        public string DiscountParagraph { get; set; }

        [StringLength(50)]
        public string DiscountAmount { get; set; }

        public TimeSpan WorkStartTime { get; set; }

        public TimeSpan BreakStartTime { get; set; }

        public TimeSpan BreakEndTime { get; set; }

        public TimeSpan WorkEndTime { get; set; }
    }
}
