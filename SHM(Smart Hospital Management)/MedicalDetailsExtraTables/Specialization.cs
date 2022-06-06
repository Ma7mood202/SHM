using SHM_Smart_Hospital_Management_.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Specialization
    {
        [Key]
        public int Specialization_Id { get; set; }
        [Required]
        [StringLength(60)]
        [Display(Name = "Specialization")]
        public string Specialization_Name { get; set; }
        [ForeignKey("Department_Name")]
        public virtual ICollection<Department> Departments { get; set; }
    }
}
