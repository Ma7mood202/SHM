using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Break_Tables
{
    public class Medical_Allergy
    {
        [Key]
        [Column(Order = 1)]
        public int Allergy_Id { get; set; } // foreignKey
        [Key]
        [Column(Order = 2)]
        public int Medical_Detail_Id { get; set; } // foreignKey
    }
}
