using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class Medical_Ray
    {
        [NotMapped]
        public IFormFile File { get; set; }
        [Key]
        public int Ray_Id { get; set; }
        [Required]
        public DateTime Ray_Date { get; set; }
        [Required]
        [Display(Name = "Ray Result")]
        [StringLength(250)]
        public string Ray_Result { get; set; } 
        public int Ray_Type_Id { get; set; } // Foreign Key
        [Required]
        public int Medical_Detail_Id { get; set; } // Foreign Key // cascade
    }
}