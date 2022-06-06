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
        [Display(Name = "Examination")]
        public double? Bill_Examination { get; set; } // Currency

        [Display(Name = "Surgeries")]
        public double? Bill_Surgeries { get; set; }

        [Display(Name = "Rays")]
        public double? Bill_Rays { get; set; }

        [Display(Name = "Medical Test")]
        public double? Bill_Medical_Test { get; set; }

        [Display(Name = "Room Service")]
        public double? Bill_Room_Service { get; set; }

        [Display(Name = "Medication")]
        public double? Bill_Medication { get; set; }

        [Display(Name = "Bill Date")]
        public DateTime Bill_Date { get; set; }
        [Display(Name = "Is Paid")]
        public bool Paid { get; set; } 
        [Required]
        public int Patient_Id { get; set; } // Foreign Key //cascade
    }
}
