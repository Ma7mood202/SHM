using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class DisplaySurguries
    {
        public int Id { get; set; }
        public string PatientName { get; set; }
        public string SurgeryName { get; set; }
        public string RoomNumber { get; set; }
        public int Floor { get; set; }
        public List<Patient_Phone_Numbers> PhoneNumbers { get; set; }
        public string SurguryDate { get; set; }
        public string SurgeryLength { get; set; }
    }
}
