using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using SHM_Smart_Hospital_Management_.PasswordHash;
using SHM_Smart_Hospital_Management_.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using SHM_Smart_Hospital_Management_.PhoneNumbers;
using Newtonsoft.Json;
using SHM_Smart_Hospital_Management_.Notifications;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    [ResponseCache(Location =ResponseCacheLocation.None,NoStore =true)]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Master(int? id, int? HoId) // Doctor/DeptManager (id)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(m => m.Doctor_Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            if (!doctor.Active)
                return RedirectToAction("LogOut");
            var Department =await _context.Departments.FirstOrDefaultAsync(m => m.Dept_Manager.Doctor_Id == doctor.Doctor_Id);

            if (Department != null)
            {
                return RedirectToAction("MasterDeptMgr", new { id = id, HoId = HoId });
            }
            var doctorPreviews = await(from pre in _context.Previews
                                  join pat in _context.Patients
                                  on pre.Patient_Id equals pat.Patient_Id
                                  where pre.Doctor_Id == id && pre.Preview_Date > DateTime.Now
                                  orderby pre.Preview_Date
                                  select new ShowPreviewsForDoctor
                                  {
                                      PreviewId = pre.Preview_Id,
                                      PatientName = pat.Patient_First_Name + " " + pat.Patient_Last_Name,
                                      PreviewDate = pre.Preview_Date.ToString("MM/dd/yyyy"),
                                      PreviewHour = pre.Preview_Date.ToString("hh-mm-tt"),
                                      ExaminationRecord = pre.ExaminationRecord
                                  }).ToListAsync();
            ViewBag.DoctorName = doctor.Doctor_First_Name + " " + doctor.Doctor_Last_Name;
            ViewBag.DoctorId = doctor.Doctor_Id;
            ViewBag.HospitalId = HoId;
            return View(doctorPreviews);
        }
        [Authorize(Roles = "DeptManager")]
        public async Task<IActionResult> MasterDeptMgr(int id, int HoId) // DeptManager (id)
        {
            var doctor =await _context.Doctors.FirstOrDefaultAsync(m => m.Doctor_Id == id);
            var doctorPreviews =await (from pre in _context.Previews
                                  join pat in _context.Patients
                                  on pre.Patient_Id equals pat.Patient_Id
                                  where pre.Doctor_Id == id && pre.Preview_Date > DateTime.Now
                                  orderby pre.Preview_Date
                                  select new ShowPreviewsForDoctor
                                  {
                                      PreviewId = pre.Preview_Id,
                                      PatientName = pat.Patient_First_Name + " " + pat.Patient_Last_Name,
                                      PreviewDate = pre.Preview_Date.ToString("MM/dd/yyyy"),
                                      PreviewHour = pre.Preview_Date.ToString("hh-mm-tt"),
                                      ExaminationRecord = pre.ExaminationRecord
                                  }).ToListAsync();
            ViewBag.DeptMgrName = doctor.Doctor_First_Name + " " + doctor.Doctor_Last_Name;
            ViewBag.DeptMgrId = doctor.Doctor_Id;
            ViewBag.DoctorName = doctor.Doctor_First_Name + " " + doctor.Doctor_Last_Name;
            ViewBag.HoId = HoId;
            return View(doctorPreviews);
        }
        [Authorize(Roles = "DeptManager")]
        public async Task<IActionResult> DisplayDoctrosByDeptId(int id, int HoId, string search = "") // DeptManager (id)
        {
            var Deptmanager = await _context.Doctors.FirstOrDefaultAsync(d => d.Doctor_Id == id);
            var dept = _context.Departments.Where(d => d.Department_Id == Deptmanager.Department_Id).Include(d => d.Dept_Manager).ToArray()[0];
            if (!Deptmanager.Active && Deptmanager.Doctor_Id == dept.Dept_Manager.Doctor_Id)
                return RedirectToAction("LogOut");
            var doctors = await _context.Doctors.Where(d => d.Department_Id == Deptmanager.Department_Id && d.Doctor_Id != Deptmanager.Doctor_Id && d.Active).ToListAsync();
            if (doctors == null) return Ok("this deprtment is empty !!");
            ViewBag.DeptMgrId = id;
            ViewBag.DeptId = Deptmanager.Department_Id;
            ViewBag.HoId = HoId;
            if (string.IsNullOrEmpty(search))
                return View(doctors);
            else
            {
                doctors = await _context.Doctors.Where(d => d.Department_Id == Deptmanager.Department_Id && d.Doctor_Id != Deptmanager.Doctor_Id && d.Active && (d.Doctor_First_Name.Contains(search)|| d.Doctor_Last_Name.Contains(search)|| d.Doctor_Middle_Name.Contains(search))).ToListAsync();
                return View(doctors);
            }
        }
        [Authorize(Roles ="Resception")]
        public async Task<IActionResult> HoDoctors(int id, int EmpId, string search = "") // Hospital (id)
        { // تفاصيل عامة ===> ساعات الدوام 
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut", "Employee");
            var departments = await _context.Departments.Where(d => d.Ho_Id == id).ToListAsync();
            var doctors =  (from d in _context.Doctors.ToList()
                           join dept in departments
                           on d.Department_Id equals dept.Department_Id
                           where d.Active == true
                           select new HoDoctors
                           {
                               DoctorId = d.Doctor_Id,
                               DoctorFullName = d.Doctor_Full_Name,
                               Specialization = _context.Specializations.Find(dept.Department_Name).Specialization_Name,
                               PhoneNumbers = _context.Doctor_Phone_Numbers.Where(pn => pn.Doctor_Id == d.Doctor_Id).Select(s => s.Doctor_Phone_Number).ToList()
                           }).ToList();
            ViewBag.HoId = id;
            ViewBag.EmpId = EmpId;
            if (string.IsNullOrEmpty(search))
                return View(doctors);
            else
            {
                doctors = (from d in _context.Doctors.ToList()
                           join dept in departments
                           on d.Department_Id equals dept.Department_Id
                           where d.Active == true && (d.Doctor_First_Name.Contains(search) || d.Doctor_Last_Name.Contains(search) || d.Doctor_Middle_Name.Contains(search))
                           select new HoDoctors
                           {
                               DoctorId = d.Doctor_Id,
                               DoctorFullName = d.Doctor_Full_Name,
                               Specialization = _context.Specializations.Find(dept.Department_Name).Specialization_Name,
                               PhoneNumbers = _context.Doctor_Phone_Numbers.Where(pn => pn.Doctor_Id == d.Doctor_Id).Select(s => s.Doctor_Phone_Number).ToList()
                           }).ToList();
                return View(doctors);
            }
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeptManagers(int id, int EmpId, string search = "") // Hospital (id)
        {
            var IT = await _context.Employees.FindAsync(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut");
            var dept = await _context.Departments.Where(d => d.Dept_Manager != null && d.Ho_Id == id).ToListAsync();
            var deptmanagers = (from doc in _context.Doctors.ToList()
                                join d in dept
                                on doc.Doctor_Id equals d.Dept_Manager.Doctor_Id
                                into groupDept
                                from c in groupDept.DefaultIfEmpty()
                                where c is not null
                                select new { c, doc }
                                into a
                                join spec in _context.Specializations.ToList()
                                on a.c.Department_Name equals spec.Specialization_Id
                                select new DeptManagers
                                {
                                    MgrName = a.doc.Doctor_Full_Name,
                                    DeptName = spec.Specialization_Name,
                                    Id = a.c.Department_Id,
                                    DoctorManagerId = a.doc.Doctor_Id
                                }).ToList();
            ViewBag.Ho_Id = id;
            ViewBag.EmpId = EmpId;
            if (string.IsNullOrEmpty(search))
                return View(deptmanagers);
            else
            {
                deptmanagers = (from doc in _context.Doctors.ToList()
                                join d in dept
                                on doc.Doctor_Id equals d.Dept_Manager.Doctor_Id
                                into groupDept
                                from c in groupDept.DefaultIfEmpty()
                                where c is not null && (doc.Doctor_First_Name.Contains(search) || doc.Doctor_Last_Name.Contains(search) || doc.Doctor_Middle_Name.Contains(search))
                                select new { c, doc }
                                into a
                                join spec in _context.Specializations.ToList()
                                on a.c.Department_Name equals spec.Specialization_Id
                                select new DeptManagers
                                {
                                    MgrName = a.doc.Doctor_Full_Name,
                                    DeptName = spec.Specialization_Name,
                                    Id = a.c.Department_Id,
                                    DoctorManagerId = a.doc.Doctor_Id
                                }).ToList();
                return View(deptmanagers);
            }
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> EditDeptManager(int id, int DoctorId, int EmpId, string search = "") // Department (id)
        {
            var IT = await _context.Employees.FindAsync(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            var department = await _context.Departments.FindAsync(id);
            department.Dept_Manager = await _context.Doctors.FindAsync(DoctorId);
            var Doctors = await _context.Doctors.Where(d => d.Department_Id == id
             && d.Doctor_Id != department.Dept_Manager.Doctor_Id)
            .ToListAsync();
            ViewBag.DeptID = id;
            ViewBag.EmpId = EmpId;
            ViewBag.Ho_Id = department.Ho_Id;
            ViewBag.DoctorId = DoctorId;
            if (string.IsNullOrEmpty(search))
                return View(Doctors);
            else
                Doctors = Doctors.Where(d => d.Doctor_Full_Name.Contains(search)).ToList();
            return View(Doctors);
        }
        public async Task<IActionResult> LogIn()
        {
            var hospitals = await _context.Hospitals.ToListAsync();
            return View(hospitals);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(IFormCollection fc, string ReturnUrl)
        {
            // TempData["LoginFailed"] = "Login Failed";
            var doctors = _context.Doctors.Where(d => d.Doctor_Email == fc["email"].ToString() && d.Doctor_Password == (fc["password"].ToString())).ToList();
            if (doctors.Count == 0) return NotFound();
            var hospital = _context.Hospitals.FirstOrDefault(h => h.Ho_Id == int.Parse(fc["hospital"]));
            var data = (from doc in doctors
                        join dept in _context.Departments.ToList()
                        on doc.Department_Id equals dept.Department_Id
                        where dept.Ho_Id == hospital.Ho_Id
                        select doc).ToList();
            if (data.Count == 0) return NotFound();
            Doctor doctor = data[0];
            if (doctor is not null)
            {
                var deptManagers = await _context.Departments.Where(d => d.Ho_Id == hospital.Ho_Id).Include(d => d.Dept_Manager).Select(d => d.Dept_Manager).ToListAsync(); 
                //************* cookie Auth
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email , doctor.Doctor_Email),
                    new Claim(ClaimTypes.Name,doctor.Doctor_Password),
                    new Claim(ClaimTypes.Role,deptManagers.Contains(doctor) ? "DeptManager" : "Doctor")
                };
                var claimsIdentity = new ClaimsIdentity(claims, "LogIn");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (ReturnUrl == null)
                {
                    FCMService.UpdateToken(fc["fcmToken"].ToString(), doctor.Doctor_Id, UserType.doc, Platform.Web);
                    return RedirectToAction("Master", "Doctor", new { id = doctor.Doctor_Id, HoId = hospital.Ho_Id });
                }
                return RedirectToAction(ReturnUrl);
            }
            //*************
            return View();
        }
        public async Task<IActionResult> LogOut(int id)
        {
            await HttpContext.SignOutAsync();
            FCMService.RemoveUnusedToken(id, UserType.doc, Platform.Web);
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> CreateDeptManager(int id, int EmpId) // Hospital (id)
        {
            Doctor doctor = new Doctor()
            {
                Doctor_Email = "mmmmmmmmmm",
                Doctor_Password = "mmmmmmmmmmmmmmm",
                Doctor_Hire_Date = DateTime.Now
            };
            var specializations =await (from s in _context.Specializations
                                   join dept in _context.Departments
                                   on s.Specialization_Id equals dept.Department_Name
                                   where dept.Ho_Id == id && dept.Dept_Manager == null
                                   select new Specialization_Dept
                                   {
                                       Dept_Id = dept.Department_Id,
                                       Spec_Name = s.Specialization_Name,
                                       Active = dept.Active
                                   })
                                   .ToListAsync();
            ViewBag.Specializations = specializations;
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.Ho_Id = id;
            ViewBag.EmpId = EmpId;
            return View(doctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDeptManager(Doctor doctor, int EmpId, string[] pn)
        {
            var IT =await  _context.Employees.FindAsync(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut");
            if (ModelState.IsValid)
            {
                doctor.Doctor_Email = doctor.Doctor_EmailName.Replace(" ", "_");
                doctor.Doctor_Password = PasswordHashing.HashPassword(doctor.Doctor_National_Number);

                List<Doctor_Phone_Numbers> pns = new();
                for (int i = 0; i < pn.Length; i++)
                {
                    if (pn[i] is not null)
                    {
                        pns.Add(new Doctor_Phone_Numbers
                        {
                            Doctor_Id = doctor.Doctor_Id,
                            Doctor_Phone_Number = pn[i]
                        });
                    }
                }
                doctor.Doctor_Phone_Numbers = pns.Distinct().ToList();
                return RedirectToAction("AddDeptManager", "Request", new { EmpId, doctor = JsonConvert.SerializeObject(doctor) });
            }
            return View(doctor);
        }
        [Authorize(Roles = "DeptManager")]
        public async Task<IActionResult> DetailsForDeptMgr(int? id, int HoId, int DeptMgrId) // Doctor (id)
        {
            var Deptmanager = _context.Doctors.Find(DeptMgrId);
            var dept = _context.Departments.Where(d => d.Department_Id == Deptmanager.Department_Id).Include(d => d.Dept_Manager).ToArray()[0];
            if (!Deptmanager.Active && Deptmanager.Doctor_Id == dept.Dept_Manager.Doctor_Id)
                return RedirectToAction("LogOut");
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Doctor_Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewBag.HoId = HoId;
            ViewBag.DeptMgrId = DeptMgrId;
            return View(doctor);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> DetailsForResception(int? id, int EmpId, int HoId) // Doctor (id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut", "Employee");

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(m => m.Doctor_Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewBag.HoId = HoId;
            ViewBag.EmpId = EmpId;
            ViewBag.PhoneNumbers =await _context.Doctor_Phone_Numbers.Where(pn => pn.Doctor_Id == id).ToListAsync();

            return View(doctor);
        }
        [Authorize(Roles = "DeptManager")]
        public async Task<IActionResult> Create(int id, int DocId, int HoId) // Department (id)
        {
            var Deptmanager = await _context.Doctors.FirstOrDefaultAsync(d => d.Doctor_Id == DocId);
            var dept = _context.Departments.Where(d => d.Department_Id == Deptmanager.Department_Id).Include(d => d.Dept_Manager).ToArray()[0];
            if (!Deptmanager.Active && Deptmanager.Doctor_Id == dept.Dept_Manager.Doctor_Id)
                return RedirectToAction("LogOut");
            Doctor doctor = new Doctor
            {
                Doctor_Email = "mmmmmmmmmm",
                Doctor_Password = "mmmmmmmmmmmmmmm",
                Doctor_Hire_Date = DateTime.Now,
                Department_Id = id
            };
            ViewBag.Cities = _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToList();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.HoId = HoId;
            ViewBag.DocId = DocId;
            ViewBag.DeptId = id;
            return View(doctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor, int HoId, int DocId, string[] pn) // DocId = DeptManagerId
        {
            var Deptmanager = await _context.Doctors.FirstOrDefaultAsync(d => d.Doctor_Id == DocId);
            var dept = _context.Departments.Where(d => d.Department_Id == Deptmanager.Department_Id).Include(d => d.Dept_Manager).ToArray()[0];
            if (!Deptmanager.Active && Deptmanager.Doctor_Id == dept.Dept_Manager.Doctor_Id)
                return RedirectToAction("LogOut");         
            
            if (ModelState.IsValid)
            {
                doctor.Doctor_Email = doctor.Doctor_EmailName.Replace(" ", "_");
                doctor.Doctor_Password = PasswordHashing.HashPassword(doctor.Doctor_National_Number);
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                List<Doctor_Phone_Numbers> pns = new();
                for (int i = 0; i < pn.Length; i++)
                {
                    if (pn[i] is not null)
                    {
                        pns.Add(new Doctor_Phone_Numbers
                        {
                            Doctor_Id = doctor.Doctor_Id,
                            Doctor_Phone_Number = pn[i]
                        });
                    }
                }
                await _context.AddRangeAsync(pns.Distinct());
                await _context.SaveChangesAsync();
                FCMService.AddToken(doctor.Doctor_Id, UserType.doc);
                return RedirectToAction("Master", new { id = DocId, HoId = HoId });
            }
            return View(doctor);
        }

        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> EditPersonalDetails(int id ,int HoId) // Doctor (id)
        {
            var doctor = await _context.Doctors.Include(d => d.Doctor_Phone_Numbers).FirstOrDefaultAsync(d => d.Doctor_Id == id);
            if (!doctor.Active)
                return RedirectToAction("LogOut");
            int doctorCityId = (from c in _context.Cities
                                 join a in _context.Areas
                                 on c.City_Id equals a.City_Id
                                 where a.Area_Id == doctor.Area_Id
                                 select c.City_Id).ToArray()[0];
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name, Selected = c.City_Id == doctorCityId ? true : false }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.DoctorArea = _context.Areas.Find(doctor.Area_Id).Area_Name;
            return View(doctor);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPersonalDetails(Doctor doctor, string[] pn)
        {
            if (ModelState.IsValid)
            {
                List<Doctor_Phone_Numbers> pns = new List<Doctor_Phone_Numbers>();
                foreach (var item in pn)
                {
                    pns.Add(new Doctor_Phone_Numbers
                    {
                        Doctor_Id = doctor.Doctor_Id,
                        Doctor_Phone_Number = item
                    });
                }
                _context.Doctor_Phone_Numbers.RemoveRange(_context.Doctor_Phone_Numbers.Where(d => d.Doctor_Id == doctor.Doctor_Id));
                _context.AddRange(pns.Distinct());
                _context.Update(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Master", new { id = doctor.Doctor_Id });
            }
            return View();
        }
        [Authorize(Roles = "DeptManager")]
        public async Task<IActionResult> Delete(int? id, int HoId, int DeptMgrId)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            doctor.Active = false;
            await _context.SaveChangesAsync();
            FCMService.RemoveToken(doctor.Doctor_Id, UserType.doc);
            return RedirectToAction("DisplayDoctrosByDeptId", new { id = DeptMgrId, HoId = HoId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> ShowActiveDoctorsForIT(int id, string search = "") //IT(id)
        {
            var IT = await _context.Employees.FindAsync(id);
            if (!IT.Active)
                return RedirectToAction("LogOut");
            ViewBag.EmpId = id;
            var data = await (from doc in _context.Doctors
                              join dept in _context.Departments
                              on doc.Department_Id equals dept.Department_Id
                              where dept.Ho_Id == IT.Ho_Id
                              && doc.Active
                              select doc).ToListAsync();
            if (string.IsNullOrEmpty(search))
                return View(data);
            else
            {
                 data = await (from doc in _context.Doctors
                                 join dept in _context.Departments
                                 on doc.Department_Id equals dept.Department_Id
                                 where dept.Ho_Id == IT.Ho_Id && (doc.Doctor_First_Name.Contains(search) || doc.Doctor_Last_Name.Contains(search) || doc.Doctor_Middle_Name.Contains(search))
                                 && doc.Active
                                 select doc).ToListAsync();
                return View(data);
            }
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> ShowUnActiveDoctorsForIT(int id) //IT(id)
        {
            var IT = await _context.Employees.FindAsync(id);
            if (!IT.Active)
                return RedirectToAction("LogOut");
            ViewBag.EmpId = id;
            var data = await (from doc in _context.Doctors
                              join dept in _context.Departments
                              on doc.Department_Id equals dept.Department_Id
                              where dept.Ho_Id == IT.Ho_Id
                              && doc.Active == false
                              select doc).ToListAsync();
            return View(data);
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeActivate(int? id, int EmpId) // Doctor (id)
        {
            var IT = _context.Employees.Find(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            var doctor = await _context.Doctors.FindAsync(id);
            doctor.Active = false;
            await _context.SaveChangesAsync();
            FCMService.RemoveToken(doctor.Doctor_Id, UserType.doc);
            return RedirectToAction("ShowActiveDoctorsForIT", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Activate(int? id, int EmpId)// Doctor (id)
        {
            var IT = _context.Employees.Find(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            var doctor = await _context.Doctors.FindAsync(id);
            doctor.Active = true;
            await _context.SaveChangesAsync();
            FCMService.AddToken(doctor.Doctor_Id, UserType.doc);
            return RedirectToAction("ShowUnActiveDoctorsForIT", new { id = EmpId });
        }
        public async Task<IActionResult> GetAreas(string CityId)
        {
            if (!string.IsNullOrWhiteSpace(CityId))
            {
                List<SelectListItem> AreaSelect = await _context.Areas.Where(a => a.City_Id.ToString() == CityId)
                    .Select(n => new SelectListItem { Value = n.Area_Id.ToString(), Text = n.Area_Name }).ToListAsync();
                return Json(AreaSelect);
            }
            return null;
        }
    }
}
