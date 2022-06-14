using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class ShowReports
    {
        public List<Report> Surgeries { get; set; }
        public double TotalSurgeries { get; set; }
        public double TotalPreviews { get; set; }
        public List<Report> Previews { get; set; }
    }
}
