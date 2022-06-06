using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Test
    {
        [Key]
        public int Test_Id { get; set; }
        public string Test_Name { get; set; }
        public int Test_Type_Id{ get; set; } // Foreign Key

        [ForeignKey("Test_Id")]
        public virtual ICollection<Medical_Test> Medical_Tests { get; set; }
    }
}
