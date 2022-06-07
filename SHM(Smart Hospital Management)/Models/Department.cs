using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Department
    {

        [Key]
        public int Department_Id { get; set; }
        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
        [Required]
        public int Department_Name { get; set; } // Foreign Key
        public int Ho_Id { get; set; } // Foreign Key // cascade
        [ForeignKey("Dept_Mgr_Id")] // Foreign Key // set null unique // type<int?>
        public virtual Doctor Dept_Manager { get; set; }
        [ForeignKey("Department_Id")]
        public virtual ICollection<Doctor> Department_Doctors { get; set; }
    }
}
