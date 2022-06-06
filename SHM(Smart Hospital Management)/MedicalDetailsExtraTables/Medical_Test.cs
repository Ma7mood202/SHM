using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Medical_Test
    {
        [NotMapped]
        public IFormFile File { get; set; }
        [Key]
        public int Medical_Test_Id { get; set; }
        [Display(Name = "Test Date")]
        [Required]
        public DateTime Test_Date { get; set; }
        [Required]
        [Display(Name = "Test Result")]
        [StringLength(250)]
        public string Test_Result { get; set; } 
        public int Test_Id { get; set; } // Foreign Key
        [Required]
        public int Medical_Detail_Id { get; set; } // Foreign Key 
    }
}