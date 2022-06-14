using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Bill
    {
        [Key]
        public int Bill_Id { get; set; }
        [Display(Name = "التشخيص")]
        public double? Bill_Examination { get; set; } = 0; // Currency

        [Display(Name = "العمليات")]
        public double? Bill_Surgeries { get; set; } = 0;

        [Display(Name = "الأشعة")]
        public double? Bill_Rays { get; set; } = 0;

        [Display(Name = "التحاليل الطبية")]
        public double? Bill_Medical_Test { get; set; } = 0;

        [Display(Name = "خدمة الغرفة")]
        public double? Bill_Room_Service { get; set; } = 0;

        [Display(Name = "الأدوية")]
        public double? Bill_Medication { get; set; } = 0;

        [Display(Name = "تاريخ الفاتورة")]
        public DateTime Bill_Date { get; set; }
        [Display(Name = "تم الدفع")]
        public bool Paid { get; set; } 
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Patient_Id { get; set; } // Foreign Key //cascade
    }
}
