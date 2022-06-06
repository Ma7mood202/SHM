using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Ray_Type
    {
        [Key]
        public int Ray_Type_Id { get; set; }
        [Required]
        [Display(Name = "Ray Type")]
        [StringLength(200)]
        public string Ray_Type_Name { get; set; }
        [ForeignKey("Ray_Type_Id")]
        public virtual ICollection<Medical_Ray> Medical_Rays { get; set; }
    }
}
