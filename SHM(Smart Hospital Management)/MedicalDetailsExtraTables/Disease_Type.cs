using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Disease_Type
    {
        [Key]
        public int Disease_Type_Id { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Disease Name")]
        public string Disease_Type_Name { get; set; }
        [ForeignKey("Disease_Type_Id")]

        public virtual ICollection<Disease> Diseases { get; set; }
    }
}
