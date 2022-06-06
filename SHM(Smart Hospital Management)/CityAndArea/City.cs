using SHM_Smart_Hospital_Management_.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.CityAndArea
{
    public class City
    {
        [Key]
        public int City_Id { get; set; }
        [StringLength(100)]
        [Required]
        public string City_Name { get; set; }
        [ForeignKey("City_Id")]
        public virtual ICollection<Area> Areas { get; set; }
        [ForeignKey("Doctor_Birth_Place")]
        public virtual ICollection<Doctor> Doctors { get; set; }
        [ForeignKey("Patient_Birth_Place")]
        public virtual ICollection<Patient> Patients { get; set; }
        [ForeignKey("Employee_Birth_Place")]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
