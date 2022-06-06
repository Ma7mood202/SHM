using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class ShowMedicalTest
    {
        public int Test_Id { get; set; }
        public string Date { get; set; }
        public string Test_Name { get; set; }
        public string Result { get; set; }
    }
}
