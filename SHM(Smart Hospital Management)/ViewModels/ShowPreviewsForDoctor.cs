using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class ShowPreviewsForDoctor
    {
        public int PreviewId { get; set; }
        public string PreviewDate { get; set; }
        public string PatientName { get; set; }
        public string PreviewHour { get; set; }
        public string ExaminationRecord { get; set; }
    }
}
