using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.Notifications;
using SHM_Smart_Hospital_Management_.PasswordHash;
using SHM_Smart_Hospital_Management_.PhoneNumbers;
using SHM_Smart_Hospital_Management_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }
        //[Authorize(Roles = "IT,Resception,Nurse,HeadNurse")]
        public IActionResult Master(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = _context.Employees.FirstOrDefault(e => e.Employee_Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            if (employee.Employee_Job == "IT") return RedirectToAction("IT", new { id = id });
            if (employee.Employee_Job == "Resception") return RedirectToAction("Resception", new { id = id });
            if (employee.Employee_Job == "Nurse") return RedirectToAction("Nurse", new { id = id });
            if (employee.Employee_Job == "HeadNurse") return RedirectToAction("HeadNurse", new { id = id });
            return NotFound();
        }
       // [Authorize(Roles = "IT")]
        public async Task<IActionResult> IT(int id)
        {
            var IT = await _context.Employees.FindAsync(id);
            if (!IT.Active)
                return RedirectToAction("LogOut",new {id=id });
            if (IT == null) return NotFound();
            return View(IT);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> Resception(int id)
        {
            var Resception = await _context.Employees.FindAsync(id);
            if (!Resception.Active)
                return RedirectToAction("LogOut",new {id=id });
            if (Resception == null) return NotFound();
            ViewBag.HospitalName = _context.Hospitals.Find(Resception.Ho_Id).Ho_Name;
            return View(Resception);
        }

        [Authorize(Roles = "HeadNurse")]
        public async Task<IActionResult> HeadNurse(int id)
        {
            var HeadNurse = await _context.Employees.FindAsync(id);
            if (!HeadNurse.Active)
                return RedirectToAction("LogOut",new {id=id });
            var XYsPlusNames = await _context.Employees.Where(e => e.Employee_Job == "Nurse" && e.Ho_Id == HeadNurse.Ho_Id && e.Active).Select(s => s.Employee_X_Y + "," + s.Employee_Full_Name).ToListAsync();
            ViewBag.NursesXYs = XYsPlusNames;
            var patientRequests = _context.Requests.Where(r => r.Employee_Id == id && r.Accept == false).Select(s => s.Request_Data);
            ViewBag.PatientRequests = await patientRequests.ToListAsync();
            ViewBag.Rooms = await _context.Rooms.Where(r => r.Ho_Id == HeadNurse.Ho_Id && r.Active).ToListAsync();
            if (HeadNurse == null) return NotFound();
            return View(HeadNurse);
        }
        [Authorize(Roles = "HeadNurse")]
        public async Task<IActionResult> DisplayNurses(int id, int HoId, string search = "")
        {
            var HeadNurse = await _context.Employees.FindAsync(id);
            if (!HeadNurse.Active)
                return RedirectToAction("LogOut",new {id=id });
            ViewBag.HoId = HoId;
            ViewBag.EmpId = id;
            var employees = await _context.Employees.Where(e => e.Ho_Id == HoId && e.Employee_Job == "Nurse" && e.Active).ToListAsync();
            if (string.IsNullOrEmpty(search))
                return View(employees);
            else
            {
                employees = employees.Where(e => e.Employee_Full_Name.Contains(search)).ToList();
                return View(employees);
            }
        }
        [Authorize(Roles = "HeadNurse")]
        public async Task<IActionResult> CreateNurse(int id, int EmpId) // Hospital (id)
        {

            Employee e = new Employee
            {
                Ho_Id = id,
                Employee_Job = "Nurse",
                Active = true,
                Employee_Hire_Date = DateTime.Now,
                Employee_Email = "mmmmmmmmmmmmmmmmmmm",
                Employee_Password = "mmmmmmmmmmmmmmmm",

            };
            ViewBag.Cities =await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.EmpId = EmpId;
            return View(e);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNurse(Employee Nurse, int EmpId, string[] pn) 
        {
            if (ModelState.IsValid)
            {
                Nurse.Employee_Email = Nurse.Employee_EmailName.Replace(' ', '_');
                Nurse.Employee_Password = PasswordHashing.HashPassword(Nurse.Employee_National_Number);
                _context.Add(Nurse);
                await _context.SaveChangesAsync();

                Employee_Phone_Numbers[] pns = new Employee_Phone_Numbers[pn.Length];
                for (int i = 0; i < pn.Length; i++)
                {

                    pns[i] = new Employee_Phone_Numbers
                    {
                        Employee_Id = Nurse.Employee_Id,
                        Employee_Phone_Number = pn[i]
                    };
                }
                await _context.AddRangeAsync(pns.Distinct());
                await _context.SaveChangesAsync();
                FCMService.AddToken(Nurse.Employee_Id, UserType.emp);
                return RedirectToAction("Master", new { id = EmpId });
            }
            return View(Nurse);
        }

        [Authorize(Roles = "Nurse")]
        public async Task<IActionResult> Nurse(int id)
        {
            var Nurse = await _context.Employees.FindAsync(id);
            if (!Nurse.Active)
                return RedirectToAction("LogOut",new {id=id });
            if (Nurse == null) return NotFound();
            ViewBag.Rooms = await _context.Rooms.Where(r => r.Ho_Id == Nurse.Ho_Id && r.Active).ToListAsync();
            return View(Nurse);
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> MasterHoMgr(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Employee_Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            var Hospital =await _context.Hospitals.FirstOrDefaultAsync(h => h.Manager.Employee_Id == employee.Employee_Id && h.Active);
            if (Hospital == null)
            {
                return NotFound();
            }
            ViewBag.Requests = await _context.Requests.Where(r => r.Employee_Id == id && r.Accept == false).ToListAsync();
            return View(employee);
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ShowReports(int id) // Manager (id)
        {
            var manager = await _context.Employees.FindAsync(id);
            if (!manager.Active)
                return RedirectToAction("LogOut", new { id });


            var surgeriesAndDoctors = await (from s in _context.Surgeries
                                             join d in _context.Doctors
                                             on s.Doctor_Id equals d.Doctor_Id
                                             where s.Surgery_Date.Date.Month == DateTime.Now.Date.Month
                                             select new { Surgery = s, Doctor = d }).ToListAsync();
            var depts = (from dept in _context.Departments.ToList()
                         join sd in surgeriesAndDoctors
                         on dept.Department_Id equals sd.Doctor.Department_Id
                         where dept.Ho_Id == manager.Ho_Id
                         group dept by dept into GroupedDepts
                         select new Report
                         {
                             DepartmentName = _context.Specializations.Find(GroupedDepts.Key.Department_Name).Specialization_Name,
                             Count = surgeriesAndDoctors.Count(sd => sd.Doctor.Department_Id == GroupedDepts.Key.Department_Id)
                         }).ToList();
            var totalSurgeries = depts.Sum(d => d.Count);



            var previewsAndDoctors = await (from pre in _context.Previews
                                            join doc in _context.Doctors
                                            on pre.Doctor_Id equals doc.Doctor_Id
                                            where pre.Preview_Date.Date.Month == DateTime.Now.Date.Month
                                            select new { Preview = pre, Doctor = doc }).ToListAsync();
            var depts1 = (from dept in _context.Departments.ToList()
                          join pd in previewsAndDoctors
                          on dept.Department_Id equals pd.Doctor.Department_Id
                          where dept.Ho_Id == manager.Ho_Id
                          group dept by dept into GroupedDepts
                          select new Report
                          {
                              DepartmentName = _context.Specializations.Find(GroupedDepts.Key.Department_Name).Specialization_Name,
                              Count = previewsAndDoctors.Count(pd => pd.Doctor.Department_Id == GroupedDepts.Key.Department_Id)
                          }).ToList();
            var totalPreviews = depts1.Sum(d => d.Count);


            var model = new ShowReports
            {
                Surgeries = depts,
                TotalSurgeries = totalSurgeries,
                Previews = depts1,
                TotalPreviews = totalPreviews
            };
            ViewBag.EmpId = id;
            return View(model);
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> HoEmployees(int id, string search = "")
        {
            var manager =await _context.Employees.FindAsync(id);
            if (!manager.Active)
            {
                return RedirectToAction("LogOut",new {id=id });
            }
            var hospital =await _context.Hospitals.FirstOrDefaultAsync(h => h.Manager.Employee_Id == id && h.Active);
            var Employees = await _context.Employees.Where(e => e.Ho_Id == hospital.Ho_Id && (e.Employee_Job == "Resception" || e.Employee_Job == "IT" || e.Employee_Job == "HeadNurse") && e.Active).ToListAsync();
            ViewBag.HospitalId = hospital.Ho_Id;
            ViewBag.MgrId = id;
            if (string.IsNullOrEmpty(search))
                return View(Employees);
            else
            {
                Employees = Employees.Where(e => e.Employee_Full_Name.Contains(search)).ToList();
                return View(Employees);
            }
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateManager(int id)
        {
            Employee mgr = new Employee()
            {
                Ho_Id = id,
                Employee_Email = "mmmmmmmmmm",
                Employee_Password = "mmmmmmmmmmmmmmm",
                Employee_Hire_Date = DateTime.Now,
                Active = true,
                Employee_Job = "Manager",
                Employee_X_Y = "Manager",
            };
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            return View(mgr);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateManager(Employee employee, string[] pn)
        {
            if (ModelState.IsValid)
            {
                employee.Employee_Email = employee.Employee_EmailName.Replace(" ", "_");
                employee.Employee_Password = PasswordHashing.HashPassword(employee.Employee_National_Number);
                _context.Add(employee);
                _context.SaveChanges();
                Employee_Phone_Numbers[] pns = new Employee_Phone_Numbers[pn.Length];
                for (int i = 0; i < pn.Length; i++)
                {
                    pns[i] = new Employee_Phone_Numbers
                    {
                        Employee_Id = employee.Employee_Id,
                        Employee_Phone_Number = pn[i]
                    };
                }
                await _context.AddRangeAsync(pns.Distinct());
                await _context.SaveChangesAsync();
                var hospital = _context.Hospitals.Find(employee.Ho_Id);
                hospital.Manager = employee;
                _context.Update(hospital);
                _context.SaveChanges();
                FCMService.AddToken(employee.Employee_Id, UserType.emp);
                return RedirectToAction("Master", "Admin");
            }
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            return View(employee);
        }
        public async Task<IActionResult> LogInIT()
        {
            var hospitals = await _context.Hospitals.ToListAsync();
            return View(hospitals);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInIT(IFormCollection fc, string ReturnUrl)
        {
            var employees = _context.Employees.Where(d => d.Employee_Email == fc["email"].ToString() && d.Employee_Password == fc["password"].ToString() && d.Employee_Job == "IT" && d.Active).ToList();
            if (employees.Count == 0)
            {
                TempData["LogInFailed"] = "البيانات المدخلة غير صحيحة ";
                return RedirectToAction("LogInIT");
            }

            var employee = employees.FirstOrDefault(e => e.Ho_Id == int.Parse(fc["hospital"]));
            if (employee is not null)
            {
                //************* cookie Auth
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email , employee.Employee_Email),
                    new Claim(ClaimTypes.Name,employee.Employee_Password),
                    new Claim(ClaimTypes.Role,"IT"),

                };
                var claimsIdentity = new ClaimsIdentity(claims, "LogIn");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (ReturnUrl == null)
                {
                    FCMService.UpdateToken(fc["fcmToken"].ToString(), employee.Employee_Id, UserType.emp, Platform.Web);
                    return RedirectToAction("Master", "Employee", new { id = employee.Employee_Id });
                }
                return RedirectToAction(ReturnUrl);
            }
            //*************
            return NotFound();
        }
        public async Task<IActionResult> LogInResception()
        {
            var hospitals = await _context.Hospitals.ToListAsync();
            return View(hospitals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInResception(IFormCollection fc, string ReturnUrl)
        {
            var employees = _context.Employees.Where(d => d.Employee_Email == fc["email"].ToString() && d.Employee_Password == fc["password"].ToString() && d.Employee_Job == "Resception" && d.Active).ToList();
            if (employees.Count == 0)
            {
                TempData["LogInFailed"] = "البيانات المدخلة غير صحيحة ";
                return RedirectToAction("LogInResception");
            }

            var employee = employees.FirstOrDefault(e => e.Ho_Id == int.Parse(fc["hospital"]));
            if (employee is not null)
            {
                //************* cookie Auth
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email , employee.Employee_Email),
                    new Claim(ClaimTypes.Name,employee.Employee_Password),
                    new Claim(ClaimTypes.Role,"Resception"),

                };
                var claimsIdentity = new ClaimsIdentity(claims, "LogIn");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (ReturnUrl == null)
                {
                    FCMService.UpdateToken(fc["fcmToken"].ToString(), employee.Employee_Id, UserType.emp, Platform.Web);
                    return RedirectToAction("Master", "Employee", new { id = employee.Employee_Id });
                }
                return RedirectToAction(ReturnUrl);
            }
            //*************
            return NotFound();
        }

        public async Task<IActionResult> LogInNurse()
        {
            var hospitals = await _context.Hospitals.ToListAsync();
            return View(hospitals);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInNurse(IFormCollection fc, string ReturnUrl)
        {
            var employees = _context.Employees.Where(d => d.Employee_Email == fc["email"].ToString() && d.Employee_Password == fc["password"].ToString() && (d.Employee_Job == "Nurse" || d.Employee_Job == "HeadNurse") && d.Active).ToList();
            if (employees.Count == 0)
            {
                TempData["LogInFailed"] = "البيانات المدخلة غير صحيحة ";
                return RedirectToAction("LogInNurse");
            }

            var employee = employees.FirstOrDefault(e => e.Ho_Id == int.Parse(fc["hospital"]));
            if (employee is not null)
            {
                //************* cookie Auth
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email , employee.Employee_Email),
                    new Claim(ClaimTypes.Name,employee.Employee_Password),
                    new Claim(ClaimTypes.Role,employee.Employee_Job),

                };
                var claimsIdentity = new ClaimsIdentity(claims, "LogIn");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (ReturnUrl == null)
                {
                    FCMService.UpdateToken(fc["fcmToken"].ToString(), employee.Employee_Id, UserType.emp, Platform.Web);
                    return RedirectToAction("Master", "Employee", new { id = employee.Employee_Id });
                }
                return RedirectToAction(ReturnUrl);
            }
            //*************
            return NotFound();
        }
        public async Task<IActionResult> LogInHoMgr()
        {
            var hospitals =await _context.Hospitals.ToListAsync();
            return View(hospitals);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInHoMgr(IFormCollection fc, string ReturnUrl)
        {
            TempData["LogInFailed"] = "";
            var employees = _context.Employees.Where(d => d.Employee_Email == fc["email"].ToString() && d.Employee_Password == fc["password"].ToString()).ToList();
            if (employees.Count == 0)
            {
                TempData["LogInFailed"] = "البيانات المدخلة غير صحيحة";
                return RedirectToAction("LogInHoMgr");
            }
            var HoManager = employees.FirstOrDefault(e => e.Ho_Id == int.Parse(fc["hospital"]));
            if (HoManager is not null)
            {
                //************* cookie Auth
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email , HoManager.Employee_Email),
                    new Claim(ClaimTypes.Name,HoManager.Employee_Password),
                    new Claim(ClaimTypes.Role,"Manager"),

                };
                var claimsIdentity = new ClaimsIdentity(claims, "LogIn");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (ReturnUrl == null)
                {
                    //FCMService.UpdateToken(fc["fcmToken"].ToString(), HoManager.Employee_Id, UserType.emp, Platform.Web);
                    return RedirectToAction("MasterHoMgr", "Employee", new { id = HoManager.Employee_Id });
                }
                return RedirectToAction(ReturnUrl);
            }
            //*************
            return View();

        }
        public async Task<IActionResult> LogOut(int id)
        {
            await HttpContext.SignOutAsync();
            FCMService.RemoveUnusedToken(id, UserType.emp, Platform.Web);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create(int HoId, int MgrId) // Mgr.HoId
        {
            var manager =await _context.Employees.FindAsync(MgrId);
            if (!manager.Active)
            {
                return RedirectToAction("LogOut",new {id=MgrId});
            }
            Employee e = new Employee
            {
                Ho_Id = HoId,
                Active = true,
                Employee_Email = "mmmmmmmmmmmmmmmmm",
                Employee_Password = "mmmmmmmmmmmmmmmmm",
                Employee_Hire_Date = DateTime.Now,
                Employee_X_Y = "mmmmmmmmmmmmmmm"
            };
            ViewBag.Cities =await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.MgrId = MgrId;
            return View(e);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, int MgrId, string[] pn)
        {
            var manager = _context.Employees.Find(MgrId);
            if (!manager.Active)
            {
                return RedirectToAction("LogOut",new {id=MgrId });
            }
            if (ModelState.IsValid)
            {
                employee.Employee_Email = employee.Employee_EmailName.Replace(' ', '_');
                employee.Employee_Password = PasswordHashing.HashPassword(employee.Employee_National_Number);
                employee.Employee_X_Y = employee.Employee_Job;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                Employee_Phone_Numbers[] pns = new Employee_Phone_Numbers[pn.Length];
                for (int i = 0; i < pn.Length; i++)
                {
                    pns[i] = new Employee_Phone_Numbers
                    {
                        Employee_Id = employee.Employee_Id,
                        Employee_Phone_Number = pn[i]
                    };
                }
                await _context.AddRangeAsync(pns.Distinct());
                await _context.SaveChangesAsync();
                FCMService.AddToken(employee.Employee_Id, UserType.emp);
                return RedirectToAction("MasterHoMgr", new { id = MgrId });
            }
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.MgrId = MgrId;
            return View(employee);
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id, int MgrId)
        {
            var employee = await _context.Employees.FindAsync(id);
            employee.Active = false;
            await _context.SaveChangesAsync();
            FCMService.RemoveToken(employee.Employee_Id, UserType.emp);
            return RedirectToAction("HoEmployees", new { id = MgrId });
        }
        [Authorize(Roles = "HeadNurse")]
        public async Task<IActionResult> DeleteNurse(int? id, int HeadNurseId)
        {
            var employee = await _context.Employees.FindAsync(id);
            employee.Active = false;
            await _context.SaveChangesAsync();
            FCMService.RemoveToken(employee.Employee_Id, UserType.emp);
            return RedirectToAction("DisplayNurses", new { id = HeadNurseId, HoId = employee.Ho_Id });
        }
        [Authorize(Roles = "HeadNurse,IT,Nurse,Resception")]
        public async Task<IActionResult> EditPersonalDetails(int id) // EMployee (id)
        {
            var employee = await _context.Employees.Include(e => e.Employee_Phone_Numbers).FirstOrDefaultAsync(e => e.Employee_Id == id);
            if (!employee.Active)
                return RedirectToAction("LogOut",new {id=id });
            int employeeCityId = (from c in _context.Cities
                                 join a in _context.Areas
                                 on c.City_Id equals a.City_Id
                                 where a.Area_Id == employee.Area_Id
                                 select c.City_Id).ToArray()[0];
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name, Selected = c.City_Id == employeeCityId ? true : false }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.EmployeeArea = _context.Areas.Find(employee.Area_Id).Area_Name;
            employee.Employee_EmailName = "mmmmmmmmmmm";
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPersonalDetails(Employee employee, string[] pn)
        {
            if (ModelState.IsValid)
            {
                List<Employee_Phone_Numbers> pns = new List<Employee_Phone_Numbers>();
                foreach (var item in pn)
                {
                    pns.Add(new Employee_Phone_Numbers
                    {
                        Employee_Id = employee.Employee_Id,
                        Employee_Phone_Number= item
                    });
                }
                _context.Employee_Phone_Numbers.RemoveRange(_context.Employee_Phone_Numbers.Where(d => d.Employee_Id == employee.Employee_Id));
                _context.AddRange(pns.Distinct());
                _context.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction("Master", new { id = employee.Employee_Id });
            }
            int employeeCityId = (from c in _context.Cities
                                  join a in _context.Areas
                                  on c.City_Id equals a.City_Id
                                  where a.Area_Id == employee.Area_Id
                                  select c.City_Id).ToArray()[0];
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name, Selected = c.City_Id == employeeCityId ? true : false }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.EmployeeArea = _context.Areas.Find(employee.Area_Id).Area_Name;
            return View(employee);
        }
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> EditMgrPersonalDetails(int id) // EMployee (id)
        {
            var Mgr = await _context.Employees.Include(e => e.Employee_Phone_Numbers).FirstOrDefaultAsync(e => e.Employee_Id == id);
            if (!Mgr.Active)
                return RedirectToAction("LogOut",new {id=id });
            int employeeCityId = (from c in _context.Cities
                                  join a in _context.Areas
                                  on c.City_Id equals a.City_Id
                                  where a.Area_Id == Mgr.Area_Id
                                  select c.City_Id).ToArray()[0];
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name, Selected = c.City_Id == employeeCityId ? true : false }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.EmployeeArea = _context.Areas.Find(Mgr.Area_Id).Area_Name;
            Mgr.Employee_EmailName = "mmmmmmmmmm";
            return View(Mgr);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMgrPersonalDetails(Employee Mgr, string[] pn)
        {
            if (ModelState.IsValid)
            {
                List<Employee_Phone_Numbers> pns = new List<Employee_Phone_Numbers>();
                foreach (var item in pn)
                {
                    pns.Add(new Employee_Phone_Numbers
                    {
                        Employee_Id = Mgr.Employee_Id,
                        Employee_Phone_Number = item
                    });
                }
                _context.Employee_Phone_Numbers.RemoveRange(_context.Employee_Phone_Numbers.Where(d => d.Employee_Id == Mgr.Employee_Id));
                _context.AddRange(pns.Distinct());
                _context.Update(Mgr);
                await _context.SaveChangesAsync();
                return RedirectToAction("MasterHoMgr", new { id = Mgr.Employee_Id });
            }
            int employeeCityId = (from c in _context.Cities
                                  join a in _context.Areas
                                  on c.City_Id equals a.City_Id
                                  where a.Area_Id == Mgr.Area_Id
                                  select c.City_Id).ToArray()[0];
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name, Selected = c.City_Id == employeeCityId ? true : false }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.EmployeeArea = _context.Areas.Find(Mgr.Area_Id).Area_Name;
            return View();
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> ShowActiveEmployeesForIT(int id , string search = "") // IT (id)
        {
            var IT = _context.Employees.Find(id);
            if (!IT.Active)
                return RedirectToAction("LogOut",new {id=id});
            ViewBag.EmpId = id;
            var Nurses = await _context.Employees.Where(e => e.Ho_Id == IT.Ho_Id && e.Active && e.Employee_Job == "Nurse").ToListAsync();
            if (string.IsNullOrEmpty(search))
                return View(Nurses);
            else
            {
                Nurses = Nurses.Where(e => e.Employee_Full_Name.Contains(search)).ToList();
                return View(Nurses);
            }
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> ShowUnActiveEmployeesForIT(int id) // IT (id)
        {
            var IT = _context.Employees.Find(id);
            if (!IT.Active)
                return RedirectToAction("LogOut",new{id=id});
            ViewBag.EmpId = id;
            return View(await _context.Employees.Where(e => e.Ho_Id == IT.Ho_Id && e.Active == false && e.Employee_Job == "Nurse").ToListAsync());
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeActivate(int? id, int EmpId)
        {
            var IT = _context.Employees.Find(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut",new {id=EmpId });
            var employee = await _context.Employees.FindAsync(id);
            employee.Active = false;
            await _context.SaveChangesAsync();
            FCMService.RemoveToken(employee.Employee_Id, UserType.emp);
            return RedirectToAction("ShowActiveEmployeesForIT", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Activate(int? id, int EmpId)
        {
            var IT = _context.Employees.Find(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut",new {id=EmpId});
            var employee = await _context.Employees.FindAsync(id);
            employee.Active = true;
            await _context.SaveChangesAsync();
            FCMService.AddToken(employee.Employee_Id, UserType.emp);
            return RedirectToAction("ShowUnActiveEmployeesForIT", new { id = EmpId });
        }

        public IActionResult GetAreas(string CityId)
        {
            if (!string.IsNullOrWhiteSpace(CityId))
            {
                List<SelectListItem> AreaSelect = _context.Areas.Where(a => a.City_Id.ToString() == CityId)
                    .Select(n => new SelectListItem { Value = n.Area_Id.ToString(), Text = n.Area_Name }).ToList();
                return Json(AreaSelect);
            }
            return null;
        }
    }
}
