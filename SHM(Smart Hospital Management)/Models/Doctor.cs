using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Doctor
    {
        [NotMapped]
        [Display(Name = "Name in English ")]
        public string Doctor_EmailName { get; set; }
        [Key]
        public int Doctor_Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First Name Must Be Between 2 and 30 characters ..")]
        [Display(Name = "First Name")]
        public string Doctor_First_Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First Name Must Be Between 2 and 30 characters ..")]
        [Display(Name = "Middle Name")]
        public string Doctor_Middle_Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First Name Must Be Between 2 and 30 characters ..")]
        [Display(Name = "Last Name")]
        public string Doctor_Last_Name { get; set; }
        [Display(Name = "Full Name")]
        public string Doctor_Full_Name { get { return Doctor_First_Name + " " + Doctor_Middle_Name + " " + Doctor_Last_Name; } }
        [Required]
        [StringLength(25)]
        [Display(Name = "National Number")]

        public string Doctor_National_Number { get; set; }// Unique   // Remeber that he is a doctor !!!!!!!!!
        [StringLength(6)]
        [Display(Name = "Gender")]
        [Required]
        public string Doctor_Gender { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Doctor_Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "Must Be Between 8 and 25 Characters ")]
        public string Doctor_Password { get; set; }
        [Range(0, 25)]
        [Display(Name = "Family Members")]

        public int? Doctor_Family_Members { get; set; }
        [Display(Name = "Qualifications")]
        // max
        public string Doctor_Qualifications { get; set; }
        [StringLength(10)]
        [Display(Name = "Social Status")]
        public string Doctor_Social_Status { get; set; } // single selected
        [Display(Name = "Birth Date")]
        public DateTime Doctor_Birth_Date { get; set; }
        [Display(Name = "Age")]
        public int? Doctor_Age { get { return DateTime.Now.Year - Doctor_Birth_Date.Year; } }

        [Required]
        [Display(Name = "Hire Date")]
        public DateTime Doctor_Hire_Date { get; set; } = DateTime.Now;
        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
        [Required]
        public int? Doctor_Birth_Place { get; set; } // Foreign Key mn al City
        [Required]
        public int? Area_Id { get; set; } // ForeignKey;
        [Required]
        public int? Department_Id { get; set; } // Foreign Key // set null 

        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Doctor_Phone_Numbers> Doctor_Phone_Numbers { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Preview> Doctor_Previews { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Surgery> Doctor_Surgeries { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Work_Days> Work_Days { get; set; }
        [ForeignKey("Doctor_Id")]
        public virtual ICollection<Request> Doctor_Requests { get; set; }

    }
}
