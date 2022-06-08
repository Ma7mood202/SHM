using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Room
    {
        [Key]
        public int Room_Id { get; set; }
        [StringLength(20)]
        [Display(Name = "رقم الغرفة")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public string Room_Number { get; set; }
        [Range(0, 10,ErrorMessage ="الرجاء اختيار رقم بين 0 و 10")]
        [Display(Name = "الطابق")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Room_Floor { get; set; }
        [Display(Name = "متاحة")]
        public bool Room_Empty { get; set; }
        [Display(Name = "عدد الأسرة ")]
        [Range(0, 10,ErrorMessage = "الرجاء اختيار رقم بين 0 و 10")]
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Room_Beds_Count { get; set; } 

        [Display(Name = "فعّال")]
        public bool Active { get; set; } = true;
        public int Ho_Id { get; set; } // ForeignKey  // Not Required, we Take its value from a hidden Input // cascade

        [ForeignKey("Room_Id")]
        public virtual ICollection<Reservation> Room_Reservations { get; set; }
    }
}
