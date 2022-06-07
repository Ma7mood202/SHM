using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Hospital
    {
        [NotMapped]
        public int Ho_Mgr_Id { get; set; }
        [Key]
        public int Ho_Id { get; set; }
        [Required(ErrorMessage ="الرجاء عدم ترك الحقل فارغ")]
        [StringLength(50)]
        [Display(Name = "Hospital Name ")]
        public string Ho_Name { get; set; }
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        [Display(Name = "Subscribtion Date")]
        public DateTime Ho_Subscribtion_Date { get; set; }
        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
        [Required(ErrorMessage = "الرجاء عدم ترك الحقل فارغ")]
        public int Area_Id { get; set; } // foreignKey

        [ForeignKey("Mgr_Id")] // NoAction // unique // type<int?>
        public virtual Employee Manager { get; set; }

        [ForeignKey("Ho_Id")]
        public virtual ICollection<Hospital_Phone_Numbers> Hospital_Phone_Numbers { get; set; }
        [ForeignKey("Ho_Id")]
        public virtual ICollection<Patient> Ho_Patients { get; set; }
        [ForeignKey("Ho_Id")]
        public virtual ICollection<Employee> Ho_Employees { get; set; }
        [ForeignKey("Ho_Id")]
        public virtual ICollection<Room> Hospital_Rooms { get; set; }
        [ForeignKey("Ho_Id")]
        public virtual ICollection<Surgery_Room> Hospital_Surgery_Rooms { get; set; }
        [ForeignKey("Ho_Id")]
        public virtual ICollection<Department> Hospital_Departments { get; set; }
    }
}
