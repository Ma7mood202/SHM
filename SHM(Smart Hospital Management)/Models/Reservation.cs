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
        [Required]
        [Display(Name = "Start Date")]
        public DateTime Start_Date { get; set; }
        [Display(Name = "End Date")]
        public DateTime End_Date { get; set; } 
        [Required]
        public int Patient_Id { get; set; } // Foreign Key // cascade
        [Required]
        public int Room_Id { get; set; } // Foreign Key // No Action 
    }
}
