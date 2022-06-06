using SHM_Smart_Hospital_Management_.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.CityAndArea
{
    public class Area
    {
        [Key]
        public int Area_Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Area")]
        public string Area_Name { get; set; }
        public int City_Id { get; set; } // foreign Key
        [ForeignKey("Area_Id")]
        public virtual ICollection<Hospital> Hospitals { get; set; }
        [ForeignKey("Area_Id")]
        public virtual ICollection<Doctor> Doctors { get; set; }
        [ForeignKey("Area_Id")]
        public virtual ICollection<Patient> Patients { get; set; }
        [ForeignKey("Area_Id")]
        public virtual ICollection<Employee> Employees { get; set; } 
    }
}
