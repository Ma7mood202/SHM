using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Test_Type
    {
        [Key]
        public int Test_Type_Id { get; set; }
        [Display(Name = "Test Type")]
        [StringLength(200)]
        [Required]
        public string Test_Type_Name { get; set; }
        [ForeignKey("Test_Type_Id")]
        public virtual ICollection<Test> Tests { get; set; }
    }
}
