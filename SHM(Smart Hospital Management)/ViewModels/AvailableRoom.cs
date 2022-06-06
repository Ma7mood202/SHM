using SHM_Smart_Hospital_Management_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class AvailableRoom
    {
        public Room Room { get; set; }
        public int EmptyBedCount { get; set; }
        public int ReservationsCount { get; set; }
    }
}
