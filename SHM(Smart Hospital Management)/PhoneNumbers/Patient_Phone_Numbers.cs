using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.PhoneNumbers
{
    public class Patient_Phone_Numbers
    {
        [Key]
        [Column(Order = 1)]
        public int Patient_Id { get; set; }
        [Key]
        [Column(Order = 2)]
        [Required]
        [StringLength(25)]
        public string Patient_Phone_Number { get; set; }
    }
}
