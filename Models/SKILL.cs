namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SKILLS")]
    public partial class SKILL
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SKILL()
        {
            TBLRESERVATIONs = new HashSet<TBLRESERVATION>();
        }

        [Key]
        public int EmployeeServiceID { get; set; }

        public short EmployeeID { get; set; }

        public int ServiceID { get; set; }

        public virtual TBLEMPLOYEE TBLEMPLOYEE { get; set; }

        public virtual TBLSERVICE TBLSERVICE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLRESERVATION> TBLRESERVATIONs { get; set; }
    }
}
