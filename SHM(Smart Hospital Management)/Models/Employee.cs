using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Employee
    {
        [NotMapped]
        [Display(Name = "الاسم بالانكليزي ")]
        public string Employee_EmailName { get; set; }
        [Key]
        public int Employee_Id { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = " الاسم الأول")]
        public string Employee_First_Name { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = " اسم الأب")]
        public string Employee_Middle_Name { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30, MinimumLength = 2)]
        [Display(Name = "الكنية")]
        public string Employee_Last_Name { get; set; }
        [Display(Name = " الاسم الكامل")]
        public string Employee_Full_Name { get { return Employee_First_Name + " " + Employee_Middle_Name + " " + Employee_Last_Name; } }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(100)]
        [Display(Name = "البريد الالكتروني")]
        public string Employee_Email { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "كلمة السر")]
        [StringLength(250, MinimumLength = 8)]
        public string Employee_Password { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = " الرقم الوطني")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "الرجاء ادخال رقم وطني صالح")]
        public string Employee_National_Number { get; set; }
        [StringLength(6)]
        [Display(Name = "الجنس")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public string Employee_Gender { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "(X,Y)")]
        [StringLength(61)]
        public string Employee_X_Y { get; set; }
        [Display(Name = "عدد أفراد الأسرة")]
        [Range(0, 25, ErrorMessage = "يجب أن يكون بين 0 و 25")]
        public int? Employee_Family_Members { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [StringLength(30)]
        [Display(Name = "المهنة")]
        public string Employee_Job { get; set; }
        [Display(Name = "الحالة الاجتماعية ")]
        [StringLength(20)]
        public string Employee_Social_Status { get; set; } // selected single
        [Display(Name = "تاريخ الولادة ")]
        public DateTime Employee_Birth_Date { get; set; }
        [Display(Name = "العمر")]
        public int? Employee_Age { get { return DateTime.Now.Year - Employee_Birth_Date.Year; } }
        [Display(Name = "تاريخ التعيين")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public DateTime Employee_Hire_Date { get; set; } = DateTime.Now;
        [Display(Name = "فعّال")]
        public bool Active { get; set; } = true;
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "المنطقة")]
        public int Area_Id { get; set; } // Foreign Key
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "مكان السكن")]
        public int? Employee_Birth_Place { get; set; } // Foreign Key mn al City
        public int Ho_Id { get; set; } // Foreign Key // cascade

        [ForeignKey("Employee_Id")]
        public virtual ICollection<Employee_Phone_Numbers> Employee_Phone_Numbers { get; set; }
        [ForeignKey("Employee_Id")]
        public virtual ICollection<Request> Employee_Requests { get; set; }
    }
}
