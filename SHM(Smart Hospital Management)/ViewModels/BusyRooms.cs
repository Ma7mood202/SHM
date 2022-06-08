using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class BusyRooms
    {
        public int Id { get; set; }
        [Display(Name = "رقم الغرفة")]
        public string RoomNumber { get; set; }
        [Display(Name = "تاريخ الحجز")]
        public DateTime StartDate { get; set; }
        [Display(Name = "الإسم")]
        public string PatientName { get; set; }
    }
}
