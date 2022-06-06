using SHM_Smart_Hospital_Management_.Break_Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Allergy
    {
        [Key]
        public int Allergy_Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Allergy_Name { get; set; }
        [ForeignKey("Allergy_Id")]
        public virtual ICollection<Medical_Allergy> Medical_Allergies { get; set; }
    }
}
