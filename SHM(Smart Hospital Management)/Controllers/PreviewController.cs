using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class PreviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PreviewController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DisplayPatientsPreviews(int id) // Patient (id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient");
            var spec = await (from d in _context.Departments
                        join spe in _context.Specializations
                        on d.Department_Name equals spe.Specialization_Id
                        where d.Ho_Id == patient.Ho_Id
                        select new Specialization_Dept
                        {
                            Dept_Id = d.Department_Id,
                            Spec_Name = spe.Specialization_Name
                        }).ToListAsync();
            var previews =  (from pre in _context.Previews.ToList()
                            join doc in _context.Doctors.ToList()
                            on pre.Doctor_Id equals doc.Doctor_Id
                            where pre.Patient_Id == id && pre.Preview_Date.Date >= DateTime.Now.Date
                            orderby pre.Preview_Date
                            select new CancelPreview
                            {
                                PreviewId = pre.Preview_Id,
                                DocName = doc.Doctor_First_Name + " " + doc.Doctor_Last_Name,
                                PreviewDate = pre.Preview_Date.ToString("dd/MM/yyyy"),
                                PreviewHour = pre.Preview_Date.ToString("hh:mm:tt"),
                                DoctorPhoneNumber = _context.Doctor_Phone_Numbers.Where(dpn => dpn.Doctor_Id == doc.Doctor_Id).ToList(),
                                Speclization = spec.FirstOrDefault(s => s.Dept_Id == Convert.ToInt32(doc.Department_Id)).Spec_Name,
                                IsToday = pre.Preview_Date.Date == DateTime.Now.Date ? true : false,

                            }).ToList();
            ViewBag.PatientId = id;
            return View(previews);
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CancelPreview(int id, int PatId) // Preview (id)
        {
            var patient = await _context.Patients.FindAsync(PatId);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient");
            if (patient.Canceled)
                return RedirectToAction("DisplayPatientsPreviews", new { id = PatId });
            var preview = await _context.Previews.FindAsync(id);
            if (preview.Preview_Date.TimeOfDay >= DateTime.Now.TimeOfDay - TimeSpan.FromHours(2))
                return RedirectToAction("DisplayPatientsPreviews", new { id = PatId });
            patient.Canceled = true;
            _context.Previews.Remove(_context.Previews.Find(id));
            await _context.SaveChangesAsync();

            return RedirectToAction("DisplayPatientsPreviews", new { id = PatId });
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> AddExaminationRecord(int id, string examinationRecord, int DocId, int HoId)
        {
            var preview = await _context.Previews.FindAsync(id);
            preview.ExaminationRecord = examinationRecord;
            await _context.SaveChangesAsync();
            return RedirectToAction("Master", "Doctor", new { id = DocId, HoId });
        }


        #region Create Preview For Patient
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateForPatient(int id) // Patient(id) // Display All Departments in Patient.HoId
        {
            var patient =await _context.Patients.FindAsync(id);
            if (!patient.Active)
                return RedirectToAction("Logout", "Patient");
            var data = await GetDepartments(patient.Ho_Id);
            ViewBag.PatientId = id;
            return View(data);
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DeptDoctorsForPatient(int id, int DeptId)// Patient (id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (!patient.Active)
                return RedirectToAction("Logout", "Patient");
            var data = await GetHospitalDoctors(DeptId);
            ViewBag.PatientId = id;
            ViewBag.DeptId = DeptId;
            return View(data);
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> PickDateAsPatient(int id, int DeptId, int DocId, string ErrorMessage = "") // Patient (id) // Create
        {
            var patient = await _context.Patients.FindAsync(id);
            if (!patient.Active)
                return RedirectToAction("Logout", "Patient");
            var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == DocId).ToListAsync();
            ViewBag.PatientId = id;
            ViewBag.DeptId = DeptId;
            ViewBag.DocId = DocId;
            ViewBag.ErrorMessage = ErrorMessage;
            ViewBag.Times = new List<SelectListItem>();
            return View(workDays);
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> PickTimeAsPatientPost(int id, int DocId, int DeptId, DateTime date, TimeSpan PreviewTime)// Department(id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (!patient.Active)
                return RedirectToAction("Logout", "Patient");
            if (date < DateTime.Now || date == default || PreviewTime.ToString() == "12:02:00")
                return RedirectToAction("PickDateAsPatient", new { id = id, DocId = DocId, DeptId = DeptId, ErrorMessage = "الرجاء اختيار وقت متاح" });
            bool success = false;
            var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == DocId).ToListAsync();
            foreach (var item in workDays)
            {
                if (date.DayOfWeek.CompareTo((DayOfWeek)((int)item.Day)) == 0)
                {
                    success = true;
                    break;
                }
            }
            if (!success)
                return RedirectToAction("PickDateAsPatient", new { id = id, DocId = DocId, DeptId = DeptId, ErrorMessage = "الرجاء اختيار وقت متاح" });
            DateTime d = date.Date.Add(PreviewTime);
            var pre = await _context.Previews.Where(p => p.Patient_Id == id && p.Preview_Date == d).ToListAsync();
            if (!pre.Any())
            {
                if (patient.PreviewCount == 3)
                {
                    TempData["PreviewCount"] = "لقد تجاوزت الحد الأقصى لعدد الحجوزات في اليوم";
                    return RedirectToAction("Master", "Patient", new { id });
                }
                var previews = (from preview in _context.Previews.ToList()
                                join doc in _context.Doctors.ToList()
                                on preview.Doctor_Id equals doc.Doctor_Id
                                where preview.Patient_Id == id &&
                                doc.Department_Id == DeptId &&
                                preview.Preview_Date.Date == DateTime.Now.Date &&
                                doc.Active
                                select preview).ToList();
                if (previews.Count != 0)
                {
                    TempData["PreviewCount"] = "لقد تجاوزت الحد الأقصى لعدد الحجوزات اليومية في هذا القسم";
                    return RedirectToAction("Master", "Patient", new { id });
                }

                Preview p = new Preview
                {
                    Doctor_Id = DocId,
                    Patient_Id = id,
                    Preview_Date = d,
                    Caring = false
                };
                _context.Add(p);
                patient.PreviewCount++;
                await _context.SaveChangesAsync();
                TempData["PreviewAdded"] = "تم تسجيل الموعد بنجاح";
                return RedirectToAction("Master", "Patient", new { id = id });
            }
            TempData["TImePatient"] = "لدى المريض موعد آخر في نفس التوقيت يرجى اختيار توقيت آخر";
            return RedirectToAction("PickTimeAsPatient", new { id = id, DeptId = DeptId, DocId = DocId, date = date });
        }
        #endregion
        #region Create Preview For Doctor
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Create(int DoctorId, int PatientId, int HoId, string ErrorMessage = "")
        {
            var doctor =await  _context.Doctors.FindAsync(DoctorId);
            if (!doctor.Active)
                return RedirectToAction("Logout", "Doctor");
            ViewBag.PatientId = PatientId;
            ViewBag.DocId = DoctorId;
            ViewBag.HoId = HoId;
            ViewBag.ErrorMessage = ErrorMessage;
            ViewBag.Times = new List<SelectListItem>();
            return View(_context.Work_Days.Where(s => s.Doctor_Id == DoctorId));
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> GetAvailableTimePost(int DocId, int HoId, int PatientId, DateTime date, TimeSpan PreviewTime)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("Logout", "Doctor");
            if (date < DateTime.Now || date == default || PreviewTime.ToString() == "12:02:00")
                return RedirectToAction("Create", new { DoctorId = DocId, PatientId = PatientId, HoId = HoId, ErrorMessage = "الرجاء اختيار وقت متاح" });
            bool success = false;
            var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == DocId).ToListAsync();
            foreach (var item in workDays)
            {
                if (date.DayOfWeek.CompareTo((DayOfWeek)((int)item.Day)) == 0)
                {
                    success = true;
                    break;
                }
            }
            if (!success)
                return RedirectToAction("Create", new { DoctorId = DocId, PatientId = PatientId, HoId = HoId, ErrorMessage = "الرجاء إختيار احد ايام دوامك" });
            DateTime d = date.Date.Add(PreviewTime);
            var pre = await _context.Previews.Where(p => p.Patient_Id == PatientId && p.Preview_Date == d).ToListAsync();
            if (pre.Count == 0)
            {
                Preview p = new Preview
                {
                    Doctor_Id = DocId,
                    Patient_Id = PatientId,
                    Preview_Date = d,
                    Caring = false
                };
                _context.Add(p);
                await _context.SaveChangesAsync();

                return RedirectToAction("Master", "Doctor", new { id = DocId, HoId = HoId });
            }

            TempData["TimePatient"] = "لدى المريض موعد آخر في نفس التوقيت يرجى اختيار توقيت آخر";
            return RedirectToAction("Create", new { DoctorId = DocId, HoId = HoId, PatientId = PatientId });
        }

        #endregion
        #region Create Preview For Resception
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> CreateForResception(int id, int EmpId) // Patient(id)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("Logout", "Employee");
            var patient = await _context.Patients.FindAsync(id);
            var data = await GetDepartments(patient.Ho_Id);
            ViewBag.PatientId = id;
            ViewBag.EmpId = EmpId;
            return View(data);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> DeptDoctorsForResception(int id, int EmpId, int PatientId)// Department (id)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("Logout", "Employee");
            var data = await GetHospitalDoctors(id);
            ViewBag.PatientId = PatientId;
            ViewBag.EmpId = EmpId;
            ViewBag.DeptId = id;
            return View(data);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> PickDate(int id, int EmpId, int PatientId, int DocId, string ErrorMessage = "") // Department(id) // Create
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("Logout", "Employee");
            var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == DocId).ToListAsync();
            ViewBag.PatientId = PatientId;
            ViewBag.EmpId = EmpId;
            ViewBag.DeptId = id;
            ViewBag.DocId = DocId;
            ViewBag.ErrorMessage = ErrorMessage;
            ViewBag.Times = new List<SelectListItem>();
            return View(workDays);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> PickTimePost(int id, int DocId, int EmpId, int PatientId, DateTime date, TimeSpan PreviewTime)// Department(id)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("Logout", "Employee");
            if (date < DateTime.Now || date == default || PreviewTime.ToString() == "12:02:00")
                return RedirectToAction("PickDate", new { id = id, EmpId = EmpId, DocId = DocId, PatientId = PatientId, ErrorMessage = "الرجاء اختيار وقت متاح" });
            bool success = false;
            var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == DocId).ToListAsync();
            foreach (var item in workDays)
            {
                if (date.DayOfWeek.CompareTo((DayOfWeek)((int)item.Day)) == 0)
                {
                    success = true;
                    break;
                }
            }
            if (!success)
                return RedirectToAction("PickDate", new { id = id, EmpId = EmpId, DocId = DocId, PatientId = PatientId, ErrorMessage = "الرجاء اختيار وقت متاح" });
            DateTime d = date.Date.Add(PreviewTime);
            var pre = await _context.Previews.Where(p => p.Patient_Id == PatientId && p.Preview_Date == d).ToListAsync();
            if (pre.Count == 0)
            {
                Preview p = new Preview
                {
                    Doctor_Id = DocId,
                    Patient_Id = PatientId,
                    Preview_Date = d,
                    Caring = false
                };
                _context.Add(p);
                await _context.SaveChangesAsync();
                TempData["PreviewAdded"] = "تم تسجيل الموعد بنجاح";
                return RedirectToAction("Master", "Employee", new { id = EmpId });
            }

            TempData["TimePatient"] = "لدى المريض موعد آخر في نفس التوقيت يرجى اختيار توقيت آخر";
            //int id, int EmpId, int PatientId, int DocId, DateTime date
            return RedirectToAction("PickDate", new { id = id, EmpId = EmpId, DocId = DocId, PatientId = PatientId });
        }
        #endregion
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Delete(int id, int DocId, int HoId)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var preview = await _context.Previews.FindAsync(id);

            _context.Previews.Remove(preview);
            await _context.SaveChangesAsync();
            return RedirectToAction("Master", "Doctor", new { id = DocId, HoId = HoId });
        }

        private async Task<List<Specialization_Dept>> GetDepartments(int id)
        {

            var departments = await _context.Departments.Where(w => w.Ho_Id == id).ToListAsync();
            var data = (from dept in departments
                        join spec in _context.Specializations.ToList()
                        on dept.Department_Name equals spec.Specialization_Id
                        select new Specialization_Dept
                        {
                            Dept_Id = dept.Department_Id,
                            Spec_Name = spec.Specialization_Name
                        }).ToList();
            return data;
        }
        private async Task<List<HoDoctors>> GetHospitalDoctors(int id)
        {
            var doctors = await _context.Doctors.Where(d => d.Department_Id == id && d.Active).ToListAsync();
            var data = (from d in doctors
                        select new HoDoctors
                        {
                            DoctorId = d.Doctor_Id,
                            DoctorFullName = d.Doctor_Full_Name,
                            PhoneNumbers = _context.Doctor_Phone_Numbers.Where(dpn => dpn.Doctor_Id == d.Doctor_Id).Select(s => s.Doctor_Phone_Number).ToList()
                        }).ToList();
            return data;
        }
        public async Task<IActionResult> GetTimes(DateTime date, int DocId)
        { // "2022-12-10"

            if (date != default)
            {
                bool success = false;
                var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == DocId).ToListAsync();
                foreach (var item in workDays)
                {
                    if (date.DayOfWeek.CompareTo((DayOfWeek)((int)item.Day)) == 0)
                    {
                        if (true)
                        {

                        }
                        success = true;
                        break;
                    }
                }
                List<SelectListItem> TimeSelect = null;
                string message = string.Empty;
                if (!success)
                {
                    message = "الرجاء إختيار احد ايام دوامك";
                    return Json(new { TimeSelect, message });
                }
                // Start of algo;
                var DayHours = workDays.Where(w => date.DayOfWeek.CompareTo((DayOfWeek)((int)w.Day)) == 0).ToList()[0];
                TimeSpan HalfHour = TimeSpan.FromMinutes(30);
                var prev = _context.Previews.Where(p => p.Doctor_Id == DocId && p.Preview_Date.Date == date.Date).Select(s => s.Preview_Date.TimeOfDay).ToList();
                List<TimeSpan> data = new List<TimeSpan>();
                for (TimeSpan i = DayHours.Start_Hour; i < DayHours.End_Hour; i += HalfHour)
                {
                    if (date.DayOfWeek.CompareTo(DateTime.Now.DayOfWeek) == 0)
                    {
                        if (i.Hours <= DateTime.Now.Hour)
                        {
                            prev.Add(i);
                        }
                    }
                    if (!prev.Contains(i))
                        data.Add(i);
                }
                if (data.Count == 0)
                {
                    message = "لا يوجد اوقات متاحة في هذا اليوم !";
                    return Json(new { TimeSelect, message });
                }
                TimeSelect = data
                   .Select(n => new SelectListItem { Value = n.ToString(), Text = n.Hours >= 12 ? (n.Hours != 12 ? n.Hours - 12 : n.Hours).ToString() + " : " + (n.Minutes.ToString() == "0" ? "00  " : "30  ") + "PM" : (n.Hours == 0 ? 12 : n.Hours).ToString() + " : " + (n.Minutes.ToString() == "0" ? "00  " : "30  ") + "AM" }).ToList();
                return Json(new { TimeSelect, message });


            }

            return null;
        }

    }
}