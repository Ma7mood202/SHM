using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables
{
    public class External_Records
    {
        [Required(ErrorMessage = "الرجاء إختيار صورة الملف الخارجي")]
        [NotMapped]
        public IFormFile File { get; set; }
        [Key]
        public int External_Records_Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Details { get; set; }
        public DateTime Date { get; set; }
        public int Medical_Detail_Id { get; set; } // foreign Key
    }
}
