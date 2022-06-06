using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Break_Tables
{
    public class Medical_Disease
    {
        [Key]
        [Column(Order = 1)]
        public int Disease_Id { get; set; } // foreignkry
        [Key]
        [Column(Order = 2)]
        public int Medical_Detail_Id { get; set; } // ForeignKey

        public bool Family_Health_History { get; set; }
        public bool Chronic_Diseases { get; set; }
    }
}
