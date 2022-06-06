using SHM_Smart_Hospital_Management_.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Work_Days
    {
        [Key]
        [Column(Order = 1)]
        public int Doctor_Id { get; set; } // foreign key
        [Key]
        [Column(Order = 2)]
        public WeekDays Day { get; set; }
        [Display(Name = "Start Hour")]
        public TimeSpan Start_Hour { get; set; }
        [Display(Name = "End Hour")]
        public TimeSpan End_Hour { get; set; }
    }
}
