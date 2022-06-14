﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.ViewModels
{
    public class Specialization_Dept
    {
        public int Dept_Id { get; set; }
        [Display(Name ="القسم")]
        public string Spec_Name { get; set; }
        public bool Active { get; set; }
    }
}
