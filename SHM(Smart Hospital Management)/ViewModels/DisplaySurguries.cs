using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class DisplaySurguries
    {
        public int Id { get; set; }
        [Display(Name ="اسم المريض")]
        public string PatientName { get; set; }
        [Display(Name = "اسم العملية")]
        public string SurgeryName { get; set; }
        [Display(Name = "رقم غرفة العمليات")]
        public string RoomNumber { get; set; }
        [Display(Name = "الطابق")]
        public int Floor { get; set; }
        public List<Patient_Phone_Numbers> PhoneNumbers { get; set; }
        [Display(Name = "تاريخ العملية")]
        public string SurguryDate { get; set; }
        [Display(Name = "مدة العملية")]
        public string SurgeryLength { get; set; }
    }
}
