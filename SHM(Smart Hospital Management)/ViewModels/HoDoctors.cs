using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class HoDoctors
    {
        public int DoctorId { get; set; }
        [Display(Name ="الإسم")]
        public string DoctorFullName { get; set; }
        [Display(Name = "الإختصاص")]
        public string Specialization { get; set; }
        [Display(Name = "رقم الهاتف")]
        public List<string> PhoneNumbers { get; set; }
    }
}
