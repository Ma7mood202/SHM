using SHM_Smart_Hospital_Management_.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class ShowWorkDays
    {
        [Display(Name = "اليوم")]
        public WeekDays Day { get; set; }
        [Display(Name = "بداية الدوام")]
        public string Start_Hour { get; set; }
        [Display(Name = "نهاية الدوام")]
        public string End_Hour { get; set; }
        public int Doctor_Id { get; set; }
    }
}
