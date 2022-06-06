using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class CancelPreview
    {
        public int PreviewId { get; set; }
        public string DocName { get; set; }
        public string PreviewDate { get; set; }
        public string PreviewHour { get; set; }
        public List<Doctor_Phone_Numbers> DoctorPhoneNumber { get; set; }
        public string Speclization { get; set; }
        public bool IsToday { get; set; }
    }
}
