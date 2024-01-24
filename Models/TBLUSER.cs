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

        [Required(ErrorMessage = "Alan� l�tfen doldurunuz.")]
        [Display(Name = "Kullan�c� Ad�")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "(Alan� l�tfen doldurunuz).")]
        [Display(Name = "�ifre")]
        [DataType(DataType.Password)]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "(Alan� l�tfen doldurunuz).")]
        [Display(Name = "�ifre Onay�")]
        [DataType(DataType.Password)]
        [NotMapped]
        [Compare("Password", ErrorMessage = "�ifreler e�le�miyor, l�tfen tekrar deneyiniz.")]
        [StringLength(50)]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "(Alan� l�tfen doldurunuz).")]
        [Display(Name = "�sim")]
        [StringLength(25)]
        public string Name { get; set; }

        [Required(ErrorMessage = "(Alan� l�tfen doldurunuz).")]
        [Display(Name = "Soyisim")]
        [StringLength(30)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "(Alan� l�tfen doldurunuz).")]
        [Display(Name = "Telefon Numaras�")]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "(Alan� l�tfen doldurunuz).")]
        [Display(Name = "Mail Adresi")]
        [StringLength(50)]
        public string Mail { get; set; }

        public DateTime CreationDate { get; set; }

        [StringLength(100)]
        [Display(Name = "Kullan�c� Resmi")]
        public string UserImageURL { get; set; }

        public virtual ROLE ROLE { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TBLRESERVATION> TBLRESERVATIONs { get; set; }
    }
}
