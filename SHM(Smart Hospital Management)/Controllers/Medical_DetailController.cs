using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Break_Tables;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.Notifications;
using SHM_Smart_Hospital_Management_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class Medical_DetailController : Controller
    {
        private readonly ApplicationDbContext _context;
        public Medical_DetailController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ShowMedicalDetailsForPatient(int id) //Patient (id)
        {
            var patient = _context.Patients.Find(id);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient");
            var details = await _context.Medical_Details.Include(m => m.Patient).FirstOrDefaultAsync(d => d.Patient.Patient_Id == id);

            var allergies = await (from a in _context.Allergies
                             join ma in _context.Medical_Allergies
                             on a.Allergy_Id equals ma.Allergy_Id
                             where (ma.Medical_Detail_Id == details.Medical_Details_Id)
                             select a).ToListAsync();
            var family_diseases =  await (from d in _context.Diseases
                                   join md in _context.Medical_Diseases on d.Disease_Id equals md.Disease_Id
                                   where (md.Medical_Detail_Id == details.Medical_Details_Id && md.Family_Health_History == true)
                                   select d).ToListAsync();

            var chronic_diseases = await (from d in _context.Diseases
                                    join md in _context.Medical_Diseases on d.Disease_Id equals md.Disease_Id
                                    where (md.Medical_Detail_Id == details.Medical_Details_Id && md.Chronic_Diseases == true)
                                    select d).ToListAsync();
            ViewBag.allergies = allergies;
            ViewBag.PatientId = id;
            ViewBag.family_diseases = family_diseases;
            ViewBag.chronic_diseases = chronic_diseases;

            return View(details);
        }
        [Authorize(Roles ="Doctor,DeptManager")]
        public async Task<IActionResult> ShowMedicalDetailsForDoctor(int id, int DocId, int HoId) //Patient (id)
        {

            // (Active / DeptManagger)
            var details = await _context.Medical_Details.Include(m => m.Patient).FirstOrDefaultAsync(d => d.Patient.Patient_Id == id);
            if (details == null)
            {
                return RedirectToAction("Create", new { id = id, DocId = DocId, HoId = HoId });
            }
            var allergies = await (from a in _context.Allergies
                                   join ma in _context.Medical_Allergies
                                   on a.Allergy_Id equals ma.Allergy_Id
                                   where (ma.Medical_Detail_Id == details.Medical_Details_Id)
                                   select a).ToListAsync();


            var family_diseases = await (from d in _context.Diseases
                                         join md in _context.Medical_Diseases on d.Disease_Id equals md.Disease_Id
                                         where (md.Medical_Detail_Id == details.Medical_Details_Id && md.Family_Health_History == true)
                                         select d).ToListAsync();

            var chronic_diseases = await (from d in _context.Diseases
                                          join md in _context.Medical_Diseases on d.Disease_Id equals md.Disease_Id
                                          where (md.Medical_Detail_Id == details.Medical_Details_Id && md.Chronic_Diseases == true)
                                          select d).ToListAsync();
            var examination = await (from d in _context.Doctors
                                     join p in _context.Previews
                                     on d.Doctor_Id equals p.Doctor_Id
                                     where p.Patient_Id == id
                                     && p.ExaminationRecord != null
                                     && p.Preview_Date.Month >= DateTime.Now.Month - 1
                                     select new ShowExaminationRecords
                                     {
                                         DoctorName = "د." + d.Doctor_First_Name + " " + d.Doctor_Last_Name,
                                         Date = p.Preview_Date.ToString("dd-MM-yyyy"),
                                         Examination = p.ExaminationRecord
                                     }).ToListAsync();

            ViewBag.Examination = examination;
            ViewBag.allergies = allergies;
            ViewBag.family_diseases = family_diseases;
            ViewBag.chronic_diseases = chronic_diseases;
            ViewBag.DocId = DocId;
            ViewBag.HoId = HoId;
            return View(details);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Create(int? id, int DocId, int HoId)
        {
            var patient = _context.Patients.Find(id);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient");
            Medical_Detail d = new Medical_Detail()
            {
                Pa_Id = (int)id
            };
            ViewBag.DocId = DocId;
            ViewBag.HoId = HoId;
            ViewBag.Allergies = await _context.Allergies.ToListAsync();
            ViewBag.DiseasesTypes = await _context.Diseases_Types.ToListAsync();
            ViewBag.Diseases = new List<SelectListItem>();
            ViewBag.Diseases_Type = await _context.Diseases_Types.Select(dt => new SelectListItem { Value = dt.Disease_Type_Id.ToString(), Text = dt.Disease_Type_Name }).ToListAsync();
            return View(d);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medical_Detail medical_Detail, int[] Allergies, int[] Diseases, string[] Chronic, string[] Family, int DocId, int HoId)
        {

            ViewBag.Allergies = await _context.Allergies.ToListAsync();
            ViewBag.DiseasesTypes = await _context.Diseases_Types.ToListAsync();
            ViewBag.Diseases = new List<SelectListItem>();
            ViewBag.Diseases_Type = await _context.Diseases_Types.Select(dt => new SelectListItem { Value = dt.Disease_Type_Id.ToString(), Text = dt.Disease_Type_Name }).ToListAsync();

            if (ModelState.IsValid)
            {
                medical_Detail.Patient =await _context.Patients.FindAsync(medical_Detail.Pa_Id);
                _context.Add(medical_Detail);
                await _context.SaveChangesAsync();
                Medical_Allergy[] ma = new Medical_Allergy[Allergies.Length];
                for (int i = 0; i < ma.Length; i++)
                {
                    ma[i] = new Medical_Allergy()
                    {
                        Medical_Detail_Id = medical_Detail.Medical_Details_Id,
                        Allergy_Id = Allergies[i]
                    };
                }
                await _context.AddRangeAsync(ma);
                await _context.SaveChangesAsync();
                Medical_Disease[] md = new Medical_Disease[Diseases.Length];
                for (int i = 0; i < md.Length; i++)
                {
                    md[i] = new Medical_Disease()
                    {
                        Medical_Detail_Id = medical_Detail.Medical_Details_Id,
                        Disease_Id = Diseases[i],
                        Chronic_Diseases = Chronic[i] == "true" ? true : false,
                        Family_Health_History = Family[i] == "true" ? true : false
                    };
                }
                ViewBag.DocId = DocId;
                ViewBag.HoId = HoId;
                await _context.AddRangeAsync(md);
                await _context.SaveChangesAsync();
                #region send notification
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "channelId","other" },
                        { "title","الملف الطبي" },
                        { "body","تم إضافة الملف الطبي الخاص بك " },
                    }
                };

                await FCMService.SendNotificationToUserAsync(medical_Detail.Pa_Id, UserType.pat, message);

                #endregion
                return RedirectToAction("ShowMedicalDetailsForDoctor", new { id = medical_Detail.Pa_Id, DocId = DocId, HoId = HoId });
            }
            return View(medical_Detail);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Edit(int id, int DocId, int HoId)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if(!doctor.Active)
                return RedirectToAction("LogOut" , "Doctor");
            var dept = _context.Departments.Where(d => d.Department_Id == doctor.Department_Id).Include(d => d.Dept_Manager).ToArray()[0];
            if (doctor.Doctor_Id == dept.Dept_Manager.Doctor_Id)
                return RedirectToAction("LogOut", "Doctor");
            var medical = _context.Medical_Details.FirstOrDefault(m => m.Medical_Details_Id == id);

            var allergies = await (from a in _context.Medical_Allergies
                                   join all in _context.Allergies
                                   on a.Allergy_Id equals all.Allergy_Id
                                   where medical.Medical_Details_Id == a.Medical_Detail_Id
                                   select all
                             ).ToListAsync();
            var family = await (from md in _context.Medical_Diseases
                                join d in _context.Diseases
                                on md.Disease_Id equals d.Disease_Id
                                where medical.Medical_Details_Id == md.Medical_Detail_Id && md.Family_Health_History == true
                                select d).ToListAsync();

            var chronic = await (from md in _context.Medical_Diseases
                                 join d in _context.Diseases
                                 on md.Disease_Id equals d.Disease_Id
                                 where medical.Medical_Details_Id == md.Medical_Detail_Id && md.Chronic_Diseases == true
                                 select d).ToListAsync();



            var dis = chronic.Intersect(family);
            family = family.Except(dis).ToList();
            chronic = chronic.Except(dis).ToList();
            var data = new List<PatientDiseases>();
            data.AddRange(dis.Select(s => new PatientDiseases { Disease = s, Disease_Type = _context.Diseases_Types.Find(s.Disease_Type_Id), IsChronic = true, IsFamily = true }));
            data.AddRange(family.Select(s => new PatientDiseases { Disease = s, Disease_Type = _context.Diseases_Types.Find(s.Disease_Type_Id), IsChronic = false, IsFamily = true }));
            data.AddRange(chronic.Select(s => new PatientDiseases { Disease = s, Disease_Type = _context.Diseases_Types.Find(s.Disease_Type_Id), IsChronic = true, IsFamily = false }));

            ViewBag.Allergies = _context.Allergies.ToList();
            ViewBag.PatientAllergies = allergies;
            ViewBag.PatientDiseases = data;
            ViewBag.HoId = HoId;
            ViewBag.DocId = DocId;
            ViewBag.DiseasesTypes = _context.Diseases_Types.ToList();
            ViewBag.Diseases = new List<SelectListItem>();
            ViewBag.Diseases_Type = _context.Diseases_Types.Select(dt => new SelectListItem { Value = dt.Disease_Type_Id.ToString(), Text = dt.Disease_Type_Name }).ToList();
            return View(medical);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medical_Detail medical_Detail, int[] Diseases, string[] Chronic, string[] Family, int HoId, int DocId, int[] Allergies)
        {
            var doc = _context.Doctors.Find(DocId);
            if (!doc.Active)
            {
                RedirectToAction("LogOut", "Doctor", new { id = DocId });
            }
            _context.Update(medical_Detail);
            var aller = _context.Medical_Allergies.Where(m => m.Medical_Detail_Id == medical_Detail.Medical_Details_Id).ToList();
            _context.Medical_Allergies.RemoveRange(aller);
            await _context.SaveChangesAsync();
            if (Allergies.Length != 0)
            {
                Medical_Allergy[] ma = new Medical_Allergy[Allergies.Length] ;
                for (int i = 0; i < ma.Length; i++)
                {
                    ma[i] = new Medical_Allergy()
                    {
                        Medical_Detail_Id = medical_Detail.Medical_Details_Id,
                        Allergy_Id = Allergies[i]
                    };
                }
                await _context.AddRangeAsync(ma);
            }

            var MedicalDisease = _context.Medical_Diseases.Where(m => m.Medical_Detail_Id == medical_Detail.Medical_Details_Id).ToList();
            _context.Medical_Diseases.RemoveRange(MedicalDisease);
            await _context.SaveChangesAsync();
            Medical_Disease[] mds = new Medical_Disease[Diseases.Length];
            for (int i = 0; i < Family.Length; i++)
            {
                Medical_Disease temp = new Medical_Disease()
                {
                    Medical_Detail_Id = medical_Detail.Medical_Details_Id,
                    Disease_Id = int.Parse(Family[i]),
                    Family_Health_History = Family[i] == "true",
                    Chronic_Diseases = Chronic[i]== "true"
                };
                mds[i]=temp;
            }

            await _context.AddRangeAsync(mds);
            await _context.SaveChangesAsync();

            #region send notification
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تعديل على الملف الطبي"},
                            { "body","تم تعديل على الملف الطبي " },
                        }
            };
            var pat = await _context.Medical_Details.Include(p => p.Patient).FirstOrDefaultAsync(p => p.Medical_Details_Id == medical_Detail.Medical_Details_Id);
            await FCMService.SendNotificationToUserAsync(pat.Patient.Patient_Id, UserType.pat, message);

            #endregion
            return RedirectToAction("ShowMedicalDetailsForDoctor", new { id = pat.Patient.Patient_Id, DocId = DocId, HoId = HoId });
        }

        public async Task<IActionResult> GetDiseases(string Diseases_TypeId)
        {
            if (!string.IsNullOrWhiteSpace(Diseases_TypeId))
            {
                List<SelectListItem> DiseasesSelect = await _context.Diseases.Where(a => a.Disease_Type_Id.ToString() == Diseases_TypeId)
                    .Select(n => new SelectListItem { Value = n.Disease_Id.ToString(), Text = n.Disease_Name }).ToListAsync();
                return Json(DiseasesSelect);
            }
            return null;
        }
    }
}
