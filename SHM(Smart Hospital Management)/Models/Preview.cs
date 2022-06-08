using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Preview
    {
        [Key]
        public int Preview_Id { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "تاريخ المعاينة")]
        public DateTime Preview_Date { get; set; }
        [Display(Name = "العناية")]
        public bool Caring { get; set; }
        [Display(Name = "التشخيص")]
        public string ExaminationRecord { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Patient_Id { get; set; } // Foreign Key // cascade
        [Required]
        public int? Doctor_Id { get; set; } // Foreign Key // Set null 
    }
}
