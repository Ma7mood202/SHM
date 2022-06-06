using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Surgery_Room
    {
        [Key]
        public int Surgery_Room_Id { get; set; }
        [StringLength(20)]
        [Required]
        [Display(Name = "Room Number")]
        public string Su_Room_Number { get; set; }
        [Required]
        [Range(0, 10)]
        [Display(Name = "Floor")]
        public int Su_Room_Floor { get; set; }
        [Display(Name = "Available")]
        public bool Surgery_Room_Ready { get; set; }
        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
        [Required]
        public int Ho_Id { get; set; } // Foreign Key // cascade
        [ForeignKey("Surgery_Room_Id")]
        public virtual ICollection<Surgery> Surgery_Room_Surgeries { get; set; }
    }
}
