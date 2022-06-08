using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Patient
    {
      
        [NotMapped]
        [Display(Name = "الاسم بالانكليزي ")]
        public string Patient_EmailName { get; set; }
        [Key]
        public int Patient_Id { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "الاسم الأول")]
        public string Patient_First_Name { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = " اسم الأب")]
        public string Patient_Middle_Name { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "الكنية")]
        public string Patient_Last_Name { get; set; }
        [Display(Name = "الاسم الكامل")]
        public string Patient_Full_Name { get { return Patient_First_Name + " " + Patient_Middle_Name + " " + Patient_Last_Name; } }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(100)]
        [Display(Name = "البريد الالكتروني")]
        public string Patient_Email { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "كلمة السر")]
        [StringLength(250, MinimumLength = 8)]
        public string Patient_Password { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "(X,Y)")]
        [StringLength(61)]
        public string Patient_X_Y { get; set; }
        [Display(Name = "الرقم الوطني")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "الرجاء ادخال رقم وطني صالح")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public string Patient_National_Number { get; set; }
        [StringLength(6)]
        [Display(Name = "الجنس")]
        public string Patient_Gender { get; set; } // Male or Female
        [StringLength(10)]
        [Display(Name = "الحالة الاجتماعية")]
        public string Patient_Social_Status { get; set; }  // Married or Single
        [Display(Name = "تاريخ الولادة")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public DateTime Patient_Birth_Date { get; set; }

        [Display(Name = "العمر")]
        public int? Patient_Age { get { return DateTime.Now.Year - Patient_Birth_Date.Year; } }
        [Display(Name = "فعّال")]
        public bool Active { get; set; } = true;
        public bool Canceled { get; set; }
        public bool Sent { get; set; } = false;
        public int? PreviewCount { get; set; }
        [Display(Name = "المنطقة")]
        public int Area_Id { get; set; } // ForeignKey
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "مكان السكن")]
        public int? Patient_Birth_Place { get; set; } // foreign key mn al city
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Ho_Id { get; set; } // Foreign key // cascade

        [ForeignKey("Patient_Id")]
        public virtual ICollection<Patient_Phone_Numbers> Patient_Phone_Numbers { get; set; }

        [ForeignKey("Patient_Id")]
        public virtual ICollection<Preview> Patient_Previews { get; set; }
        [ForeignKey("Patient_Id")]
        public virtual ICollection<Reservation> Patient_Reservations { get; set; }
        [ForeignKey("Patient_Id")]
        public virtual ICollection<Bill> Patient_Bills { get; set; }
        [ForeignKey("Patient_Id")]
        public virtual ICollection<Surgery> Patient_Surgeries { get; set; }
        [ForeignKey("Patient_Id")]
        public virtual ICollection<Request> Patient_Requests { get; set; }
    }
}
