using SHM_Smart_Hospital_Management_.Break_Tables;
using SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Models
{
    public class Medical_Detail
    {
        [NotMapped]
        public int Pa_Id { get; set; }
        [Key]
        public int Medical_Details_Id { get; set; }

        [StringLength(3)]
        [Display(Name = "زمرة الدم")]
        public string MD_Patient_Blood_Type { get; set; }
        [Display(Name = "الخطة العلاجية")]
        public string MD_Patient_Treatment_Plans_And_Daily_Supplements { get; set; }


        [Display(Name = "الاحتياجات الخاصة")]
        public string MD_Patient_Special_Needs { get; set; }

        [ForeignKey("Patient_Id")] // cascade
        public virtual Patient Patient { get; set; }

        [ForeignKey("Medical_Detail_Id")]
        public virtual ICollection<External_Records> External_Records { get; set; }
        [ForeignKey("Medical_Detail_Id")]
        public virtual ICollection<Medical_Allergy> Medical_Allergies { get; set; }
        [ForeignKey("Medical_Detail_Id")]
        public virtual ICollection<Medical_Disease> Medical_Diseases { get; set; }
        [ForeignKey("Medical_Detail_Id")]
        public virtual ICollection<Medical_Ray> Medical_Rays { get; set; }

        [ForeignKey("Medical_Detail_Id")]
        public virtual ICollection<Medical_Test> Medical_Tests { get; set; }
    }
}
