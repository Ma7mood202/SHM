using SHM_Smart_Hospital_Management_.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class ShowWorkDays
    {
        public WeekDays Day { get; set; }
        public string Start_Hour { get; set; }
        public string End_Hour { get; set; }
        public int Doctor_Id { get; set; }
    }
}
