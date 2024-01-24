namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TBLREVIEW")]
    public partial class TBLREVIEW
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBLREVIEW()
        {
            TBLRESERVATIONs = new HashSet<TBLRESERVATION>();
        }

        [Key]
        public int ReviewID { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Comment { get; set; }

        public byte Rating { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLRESERVATION> TBLRESERVATIONs { get; set; }
    }
}
