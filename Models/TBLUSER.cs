namespace Parallax.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TBLUSER")]
    public partial class TBLUSER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TBLUSER()
        {
            TBLRESERVATIONs = new HashSet<TBLRESERVATION>();
        }

        [Key]
        public int UserID { get; set; }

        public byte RoleID { get; set; }

        [Required(ErrorMessage = "Alaný lütfen doldurunuz.")]
        [Display(Name = "Kullanýcý Adý")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "(Alaný lütfen doldurunuz).")]
        [Display(Name = "Þifre")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "(Alaný lütfen doldurunuz).")]
        [Display(Name = "Þifre Onayý")]
        [DataType(DataType.Password)]
        [NotMapped]
        [Compare("Password", ErrorMessage = "Þifreler eþleþmiyor, lütfen tekrar deneyiniz.")]
        [StringLength(50)]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "(Alaný lütfen doldurunuz).")]
        [Display(Name = "Ýsim")]
        [StringLength(25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "(Alaný lütfen doldurunuz).")]
        [Display(Name = "Soyisim")]
        [StringLength(30)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "(Alaný lütfen doldurunuz).")]
        [Display(Name = "Telefon Numarasý")]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "(Alaný lütfen doldurunuz).")]
        [Display(Name = "Mail Adresi")]
        [StringLength(50)]
        public string Mail { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Kullanýcý Resmi")]
        public string UserImageURL { get; set; }

        public virtual ROLE ROLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLRESERVATION> TBLRESERVATIONs { get; set; }
    }
}
