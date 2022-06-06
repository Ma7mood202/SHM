using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Employee
    {
        [NotMapped]
        [Display(Name = "Name in English ")]
        public string Employee_EmailName { get; set; }
        [Key]
        public int Employee_Id { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "First Name Must Be Between 2 and 30 characters ..")]
        [Display(Name = "First Name")]
        public string Employee_First_Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Middle Name Must Be Between 2 and 30 characters ..")]
        [Display(Name = "Middle Name")]
        public string Employee_Middle_Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Last Name Must Be Between 2 and 30 characters ..")]
        [Display(Name = "Last Name")]
        public string Employee_Last_Name { get; set; }
        [Display(Name = "Full Name")]
        public string Employee_Full_Name { get { return Employee_First_Name + " " + Employee_Middle_Name + " " + Employee_Last_Name; } }
        [Required]
        [StringLength(100)]
        [Display(Name = "Email")]
        public string Employee_Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "Must Be Between 8 and 25 Characters ")]
        public string Employee_Password { get; set; }
        [Required]
        [Display(Name = "National Number")]
        [StringLength(25)]
        public string Employee_National_Number { get; set; }
        [StringLength(6)]
        [Display(Name = "Gender")]
        [Required]
        public string Employee_Gender { get; set; }
        [Required]
        [Display(Name = "(X,Y)")]
        [StringLength(61)]
        public string Employee_X_Y { get; set; }
        [Display(Name = "Number of Family Members")]
        [Range(0, 25, ErrorMessage = "Must be between 0 and 25")]
        public int? Employee_Family_Members { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Job")]
        public string Employee_Job { get; set; }
        [Display(Name = "Social Status")]
        [StringLength(20)]
        public string Employee_Social_Status { get; set; } // selected single
        [Display(Name = "Birth Date")]
        public DateTime Employee_Birth_Date { get; set; }
        [Display(Name = "Age")]
        public int? Employee_Age { get { return DateTime.Now.Year - Employee_Birth_Date.Year; } }
        [Display(Name = "Hire Date")]
        [Required]
        public DateTime Employee_Hire_Date { get; set; } = DateTime.Now;
        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;
        [Required]
        public int Area_Id { get; set; } // Foreign Key
        [Required]
        public int? Employee_Birth_Place { get; set; } // Foreign Key mn al City
        public int Ho_Id { get; set; } // Foreign Key // cascade

        [ForeignKey("Employee_Id")]
        public virtual ICollection<Employee_Phone_Numbers> Employee_Phone_Numbers { get; set; }
        [ForeignKey("Employee_Id")]
        public virtual ICollection<Request> Employee_Requests { get; set; }
    }
}
