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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PatientController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Master(int id)
        { 
            var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Patient_Id == id);
            if (!patient.Active)
                return RedirectToAction("LogOut");
            var surgeries =await  _context.Surgeries.Where(s => s.Patient_Id == id && s.Surgery_Date.Date >= DateTime.Now.Date).ToListAsync();
            ViewBag.Surgeries = surgeries;
            return View(patient);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> DoctorIndex(int id, int HoId) // Doctor (id)
        {
            var doctor =await _context.Doctors.FindAsync(id);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var patients = await (from p in _context.Patients
                            join pre in _context.Previews
                            on p.Patient_Id equals pre.Patient_Id
                            where (pre.Doctor_Id == id && pre.Caring == true && p.Active)
                            select p).Distinct().ToListAsync();
            ViewBag.DocId = id;
            ViewBag.HoId = HoId;
            return View(patients);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> HoPatientsForBill(int id, int EmpId, string search = "") // Hospital (id)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut");
            var patients =await _context.Patients.Where(p => p.Ho_Id == id && p.Active).ToListAsync();
            ViewBag.HospitalId = id;
            ViewBag.EmpId = EmpId;
            if (string.IsNullOrEmpty(search))
                return View(patients);
            else
            {
                patients = await _context.Patients.Where(p => p.Ho_Id == id && p.Active &&(p.Patient_First_Name.Contains(search) || p.Patient_Last_Name.Contains(search) || p.Patient_Middle_Name.Contains(search))).ToListAsync();
                return View(patients);
            }
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> HoPatientsForPreview(int id, int DocId, string search = "") // Hospital (id)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var data = await(from pat in _context.Patients
                        join pre in _context.Previews
                        on pat.Patient_Id equals pre.Patient_Id
                        where pre.Doctor_Id == DocId
                        select pat).Distinct().ToListAsync();
            ViewBag.HospitalId = id;
            ViewBag.DocId = DocId;
            if (string.IsNullOrEmpty(search))
                return View(data);
            else
            {
                data = await (from pat in _context.Patients
                              join pre in _context.Previews
                              on pat.Patient_Id equals pre.Patient_Id
                              where pre.Doctor_Id == DocId && (pat.Patient_First_Name.Contains(search) || pat.Patient_Last_Name.Contains(search) || pat.Patient_Middle_Name.Contains(search))
                              select pat).Distinct().ToListAsync();
                return View(data);
            }
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> HoPatientsForReservation(int id, int EmpId, string search = "")// Hospital (id)
        {
            var Resception =await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut");
            var patients = await _context.Patients.Include(p => p.Patient_Phone_Numbers).Where(p => p.Ho_Id == id).ToListAsync();
            ViewBag.HoId = id;
            ViewBag.EmpId = EmpId;
            if (string.IsNullOrEmpty(search))
                return View(patients);
            else
            {
                patients = await _context.Patients.Include(p => p.Patient_Phone_Numbers).Where(p => p.Ho_Id == id &&(p.Patient_First_Name.Contains(search) || p.Patient_Last_Name.Contains(search) || p.Patient_Middle_Name.Contains(search))).ToListAsync();
                return View(patients);
            }
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> HoPatientsForResception(int id, int EmpId, string search = "")// Hospital (id)
        {
            var Resception =await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut");
            var patients = await _context.Patients.Where(p => p.Ho_Id == id && p.Active).ToListAsync();
            ViewBag.HoId = id;
            ViewBag.EmpId = EmpId;
            if (string.IsNullOrEmpty(search))
                return View(patients);
            else
            {
                patients = await _context.Patients.Where(p => p.Ho_Id == id && p.Active&&(p.Patient_First_Name.Contains(search) || p.Patient_Last_Name.Contains(search) || p.Patient_Middle_Name.Contains(search))).ToListAsync();
                return View(patients);
            }
        }
        [Authorize(Roles = "Nurse,HeadNurse")]
        public async Task<IActionResult> DisplayPatientsByName(int id, int EmpId, string patientName = "") // Hospital (id)
        {
            var Nurse = await  _context.Employees.FindAsync(EmpId);
            if (!Nurse.Active)
                return RedirectToAction("LogOut");
            var patients = await _context.Patients.Where(p => p.Ho_Id == id && p.Active).ToListAsync();
            ViewBag.EmpId = EmpId;
            if (string.IsNullOrEmpty(patientName))
                return View(patients);
            return View(patients.Where(p => p.Patient_Full_Name.Contains(patientName)).ToList());
        }
        [Authorize(Roles = "Nurse,HeadNurse")]
        public async Task<IActionResult> DisplayPatientsByRoomNumber(int id, int EmpId, int roomNumber) // Hospital (id)
        {
            var Nurse = await _context.Employees.FindAsync(EmpId);
            if (!Nurse.Active)
                return RedirectToAction("LogOut");
            var pats =await _context.Patients.Where(p => p.Ho_Id == id && p.Active).ToListAsync();
            var patients = (from p in pats
                            join res in _context.Reservations.ToList()
                            on p.Patient_Id equals res.Patient_Id
                            select new { p, res }).ToList();

            var data = (from p in patients
                        join r in _context.Rooms.ToList()
                        on p.res.Room_Id equals r.Room_Id
                        where r.Room_Id == roomNumber
                        select p.p).Distinct().ToList();
            ViewBag.EmpId = EmpId;
            return View(data);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> DetailsForResception(int id, int EmpId, int HoId) // Patient (id)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut");
            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.Patient_Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            ViewBag.HoId = HoId;
            ViewBag.EmpId = EmpId;
            ViewBag.PhoneNumbers =await _context.Patient_Phone_Numbers.Where(pn => pn.Patient_Id == id).ToListAsync();
            return View(patient);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> DetailsForDoctor(int id, int DocId, int HoId) // Patient (id)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var patient = await _context.Patients.FindAsync(id);                    
            ViewBag.HoId = HoId;
            ViewBag.DocId = DocId;
            ViewBag.PhoneNumbers =await _context.Patient_Phone_Numbers.Where(pn => pn.Patient_Id == id).ToListAsync();

            return View(patient);
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> EditPersonalDetails(int id) // Patient (id)
        {
            var patient = await _context.Patients.Include(p=>p.Patient_Phone_Numbers).FirstOrDefaultAsync(p=>p.Patient_Id == id);
            if (!patient.Active)
                return RedirectToAction("LogOut");
            int patientCityId = (from c in _context.Cities
                                 join a in _context.Areas
                                 on c.City_Id equals a.City_Id
                                 where a.Area_Id == patient.Area_Id
                                 select c.City_Id).ToArray()[0];
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name ,Selected = c.City_Id == patientCityId?true:false}).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.PatientArea = _context.Areas.Find(patient.Area_Id).Area_Name;
            return View(patient);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPersonalDetails(Patient patient , string[] pn)
        {
            if (ModelState.IsValid)
            {
                List<Patient_Phone_Numbers> pns = new List<Patient_Phone_Numbers>();
                foreach (var item in pn)
                {
                    pns.Add(new Patient_Phone_Numbers
                    {
                        Patient_Id = patient.Patient_Id,
                        Patient_Phone_Number = item
                    });
                }
                _context.Patient_Phone_Numbers.RemoveRange(_context.Patient_Phone_Numbers.Where(p=>p.Patient_Id == patient.Patient_Id));
                _context.AddRange(pns.Distinct());
                _context.Update(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction("Master", new { id = patient.Patient_Id });
            }
            return View();
        }

        
       
        public async Task<IActionResult> LogIn()
        {
            var hospitals =await _context.Hospitals.ToListAsync();
            return View(hospitals);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(IFormCollection fc, string ReturnUrl)
        { 
            var patients = _context.Patients.Where(d => d.Patient_Email == fc["email"].ToString() && d.Patient_Password == fc["password"].ToString()).ToList();
            if (patients.Count == 0) return NotFound();
            var patient = patients.FirstOrDefault(p => p.Ho_Id == int.Parse(fc["hospital"]));
            if (patient is not null)
            {
                //************* cookie Auth
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email , patient.Patient_Email),
                    new Claim(ClaimTypes.Name,patient.Patient_Password),
                    new Claim(ClaimTypes.Role,"Patient"),
                };
                var claimsIdentity = new ClaimsIdentity(claims, "LogIn");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                if (ReturnUrl == null)
                {
                    FCMService.UpdateToken(fc["fcmToken"].ToString(), patient.Patient_Id, UserType.pat, Platform.Web);
                    return RedirectToAction("Master", "Patient", new { id = patient.Patient_Id});
                }
                return RedirectToAction(ReturnUrl);
            }
            //*************
            return View();
        }
        public async Task<IActionResult> LogOut(int id)
        {
            await HttpContext.SignOutAsync();
            FCMService.RemoveUnusedToken(id, UserType.pat, Platform.Web);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> Create(int id, int EmpId) // Hospital (id)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut");
            Patient p = new Patient
            {
                Ho_Id = id,
                Patient_Email = "mmmmmmmmmmmmmmmmm",
                Patient_Password = "mmmmmmmmmmmmmmm"
            };
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            ViewBag.EmpId = EmpId;
            return View(p);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Patient patient, int EmpId, string[] pn)
        {      
            if (ModelState.IsValid)
            {
                patient.Patient_Email = patient.Patient_EmailName.Replace(" ", "_"); // Name in english
                patient.Patient_Password = PasswordHashing.HashPassword(patient.Patient_National_Number);
                _context.Add(patient);
                await _context.SaveChangesAsync();

                Patient_Phone_Numbers[] pns = new Patient_Phone_Numbers[pn.Length];
                for (int i = 0; i < pn.Length; i++)
                {
                    pns[i] = new Patient_Phone_Numbers
                    {
                        Patient_Id = patient.Patient_Id,
                        Patient_Phone_Number = pn[i]
                    };
                }
                await _context.AddRangeAsync(pns.Distinct());
                await _context.SaveChangesAsync();
                TempData["PatientAdded"] = "تمت إضافة" + patient.Patient_Full_Name;
                FCMService.AddToken(patient.Patient_Id, UserType.pat);
                return RedirectToAction("Master", "Employee", new { id = EmpId });
            }
            return View(patient);
        }

        [Authorize(Roles ="IT")]

        public async Task<IActionResult> ShowActivePatientsForIT(int id, string search = "") //IT (id)
        {
            var IT = _context.Employees.Find(id);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            ViewBag.EmpId = id;
            if (string.IsNullOrEmpty(search))
                return View(await _context.Patients.Where(p => p.Ho_Id == IT.Ho_Id && p.Active).ToListAsync());
            else
            {
                return View(await _context.Patients.Where(p => p.Ho_Id == IT.Ho_Id && p.Active&&(p.Patient_First_Name.Contains(search) || p.Patient_Last_Name.Contains(search) || p.Patient_Middle_Name.Contains(search))).ToListAsync());
            }
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> ShowUnActivePatientsForIT(int id) //IT (id)
        {
            var IT = _context.Employees.Find(id);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            ViewBag.EmpId = id;
            var patients = await _context.Patients.Where(p => p.Ho_Id == IT.Ho_Id && p.Active == false).Except(_context.Death_Cases.Include(p => p.Dead_Patient).Select(s => s.Dead_Patient)).ToListAsync();
            return View(patients);
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeActivate(int? id, int EmpId) //IT (id)
        {
            var IT = _context.Employees.Find(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            var patient = await _context.Patients.FindAsync(id);
            patient.Active = false;
            await _context.SaveChangesAsync();
            FCMService.RemoveToken(patient.Patient_Id, UserType.pat);
            return RedirectToAction("ShowActivePatientsForIT", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Activate(int? id, int EmpId) //IT (id)
        {
            var IT = _context.Employees.Find(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            var patient = await _context.Patients.FindAsync(id);
            patient.Active = true;
            await _context.SaveChangesAsync();
            FCMService.AddToken(patient.Patient_Id, UserType.pat);
            return RedirectToAction("ShowUnActivePatientsForIT", new { id = EmpId });
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
