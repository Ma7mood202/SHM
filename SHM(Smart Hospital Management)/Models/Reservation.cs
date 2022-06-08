using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Reservation
    {
        [Key]
        public int Reservation_Id { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "تاريخ بداية الحجز")]
        public DateTime Start_Date { get; set; }
        [Display(Name = "تاريخ انتهاء الحجز")]
        public DateTime End_Date { get; set; } 
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Patient_Id { get; set; } // Foreign Key // cascade
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Room_Id { get; set; } // Foreign Key // No Action 
    }
}
