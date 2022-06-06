using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class DeptManagers
    {
        public int Id { get; set; }
        public string MgrName { get; set; }
        public string DeptName { get; set; }
        public int DoctorManagerId { get; set; }
    }
}
