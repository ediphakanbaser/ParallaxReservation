namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TBLSERVICE")]
    public partial class TBLSERVICE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBLSERVICE()
        {
            SKILLS = new HashSet<SKILL>();
        }

        [Key]
        public int ServiceID { get; set; }

        [Required]
        [StringLength(30)]
        public string ServiceName { get; set; }

        public byte TypeID { get; set; }

        public decimal ServicePrice { get; set; }

        public decimal? DiscountedPrice { get; set; }

        [StringLength(100)]
        public string ServiceImageURL { get; set; }

        [Required]
        [StringLength(200)]
        public string ServiceParagraph { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public bool ServiceStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SKILL> SKILLS { get; set; }

        public virtual TYPE TYPE { get; set; }
    }
}
