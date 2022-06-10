using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class ShowPreviewsForDoctor
    {
        public int PreviewId { get; set; }
        [Display(Name ="تاريخ المعاينة")]
        public string PreviewDate { get; set; }
        [Display(Name = "اسم المريض")]
        public string PatientName { get; set; }
        [Display(Name = "وقت المعاينة")]
        public string PreviewHour { get; set; }
        public string ExaminationRecord { get; set; }
    }
}
