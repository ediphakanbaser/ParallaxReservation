namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("TBLEMPLOYEE")]
    public partial class TBLEMPLOYEE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBLEMPLOYEE()
        {
            SKILLS = new HashSet<SKILL>();
        }

        [Key]
        public short EmployeeID { get; set; }

        [Required]
        [StringLength(25)]
        public string EmpName { get; set; }

        [Required]
        [StringLength(30)]
        public string EmpSurname { get; set; }

        [Required]
        [StringLength(100)]
        public string EmpImageURL { get; set; }

        [Required]
        [StringLength(15)]
        public string EmpPhone { get; set; }

        [Required]
        [StringLength(50)]
        public string EmpMail { get; set; }

        public decimal EmpSalary { get; set; }

        public DateTime EmpRecordDateTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime EmpStardDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmpDismissalDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SKILL> SKILLS { get; set; }
    }
}
