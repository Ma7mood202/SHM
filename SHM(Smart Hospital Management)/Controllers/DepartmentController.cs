using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> HoDepartments(int? id, int EmpId, string search = "") //IT.Ho_Id
        {
            var IT = await _context.Employees.FindAsync(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee",new {id = EmpId });
            if (id == null) return NotFound();
            var departments = await (from d in _context.Departments
                                     join s in _context.Specializations
                                     on d.Department_Name equals s.Specialization_Id
                                     where d.Ho_Id == id
                                     select new Specialization_Dept
                                     {
                                         Dept_Id = d.Department_Id,
                                         Spec_Name = s.Specialization_Name,
                                         Active = d.Active
                                     }).ToListAsync();

            ViewBag.HoId = id;
            ViewBag.EmpId = EmpId;
            if (string.IsNullOrEmpty(search))
                return View(departments);
 
                departments = departments.Where(d => d.Spec_Name.Contains(search)).ToList();

                return View(departments);
            
        }
        //***********************************************************
        // GET: Department/Create
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Create(int id, int EmpId)
        {
            var IT = await _context.Employees.FindAsync(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee",new {id = EmpId });
            Department d = new Department
            {
                Ho_Id = id,
                Active = true
            };
            ViewBag.EmpId = EmpId;
            ViewBag.HoId = id;
            var specs = await (from spec in _context.Specializations
                               join dept in _context.Departments
                               on spec.Specialization_Id equals dept.Department_Name
                               where dept.Ho_Id == id && dept.Active
                               select spec).ToListAsync();
            var AllSpecs = await _context.Specializations.ToListAsync();

            foreach (var item in specs)
            {
                if (AllSpecs.Contains(item))
                {
                    AllSpecs.Remove(item);
                }
            }
            ViewBag.Specializations = AllSpecs;
            return View(d);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department, int EmpId)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("AddDepartment", "Request", new { EmpId, department = JsonConvert.SerializeObject(department) });
            }

            return View(department);
        }

    }
}
