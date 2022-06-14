using SHM_Smart_Hospital_Management_.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class ShowBills
    {
        [Display(Name ="الإسم")]
        public string FullName { get; set; }
        [Display(Name = "الإجمالي")]
        public double Total { get; set; }
        public Bill Bill { get; set; }
    }
}
