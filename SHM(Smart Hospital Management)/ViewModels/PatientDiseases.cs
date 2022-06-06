using SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class PatientDiseases
    {
        public Disease Disease { get; set; }
        public Disease_Type Disease_Type { get; set; }
        public bool IsChronic { get; set; }
        public bool IsFamily { get; set; }
    }
}
