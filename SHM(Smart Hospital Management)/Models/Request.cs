using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Request
    {
        [Key]
        public int Request_Id { get; set; }
        [StringLength(200)]
        [Display(Name = "Type")]
        public string Request_Type { get; set; } // hidden
        [Display(Name = "Description")]
        public string Request_Description { get; set; } // hidden
        [Required]
        [Display(Name = "Date")]
        public DateTime Request_Date { get; set; }
        public bool Accept { get; set; }
        public int? Patient_Id { get; set; } //Foreign Key
        public int? Doctor_Id { get; set; } //Foreign Key
        public int? Employee_Id { get; set; } //Foreign Key
        public string Request_Data { get; set; }
    }
}
