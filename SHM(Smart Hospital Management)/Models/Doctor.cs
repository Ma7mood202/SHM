using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Doctor
    {
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [NotMapped]
        [Display(Name = "الاسم بالانكليزي")]
        public string Doctor_EmailName { get; set; }
        [Key]
        public int Doctor_Id { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 3,ErrorMessage ="الرجاء ادخال اسم صالح")]
        [Display(Name = "الاسم الأول ")]
        public string Doctor_First_Name { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "الرجاء ادخال اسم صالح")]
        [Display(Name = "اسم الأب")]
        public string Doctor_Middle_Name { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "الرجاء ادخال اسم صالح")]
        [Display(Name = "الكنية")]
        public string Doctor_Last_Name { get; set; }
        [Display(Name = " الاسم الكامل")]
        public string Doctor_Full_Name { get { return Doctor_First_Name + " " + Doctor_Middle_Name + " " + Doctor_Last_Name; } }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(11,MinimumLength = 11, ErrorMessage ="الرجاء ادخال رقم وطني صالح")]
        [Display(Name = "الرقم الوطني")]
        public string Doctor_National_Number { get; set; }
        [StringLength(6)]
        [Display(Name = "الجنس")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public string Doctor_Gender { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(100)]
        [Display(Name = "البريد الالكتروني")]
        public string Doctor_Email { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "كلمة السر")]
        [StringLength(250, MinimumLength = 8)]
        public string Doctor_Password { get; set; }
        [Range(0, 25)]
        [Display(Name = "عدد افراد العائلة")]

        public int? Doctor_Family_Members { get; set; } = 0;
        [Display(Name = "المؤهلات")]
        // max
        public string Doctor_Qualifications { get; set; }
        [StringLength(10)]
        [Display(Name = "الحالة الاجتماعية")]
        public string Doctor_Social_Status { get; set; } // single selected
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "تاريخ الولادة")]
        public DateTime Doctor_Birth_Date { get; set; }
        [Display(Name = "العمر")]
        public int? Doctor_Age { get { return DateTime.Now.Year - Doctor_Birth_Date.Year; } }

        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "تاريخ التعيين")]
        public DateTime Doctor_Hire_Date { get; set; } = DateTime.Now;
        [Display(Name = "فعّال")]
        public bool Active { get; set; } = true;
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "مكان الولادة")]
        public int? Doctor_Birth_Place { get; set; } // Foreign Key mn al City
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "المنطقة")]
        public int? Area_Id { get; set; } // ForeignKey;
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "القسم")]
        public int? Department_Id { get; set; } // Foreign Key // set null 

        [ForeignKey("Doctor_Id")]
        [Display(Name = "أرقام الهواتف")]
        public virtual ICollection<Doctor_Phone_Numbers> Doctor_Phone_Numbers { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Preview> Doctor_Previews { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Surgery> Doctor_Surgeries { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Work_Days> Work_Days { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Request> Doctor_Requests { get; set; }

    }
}
