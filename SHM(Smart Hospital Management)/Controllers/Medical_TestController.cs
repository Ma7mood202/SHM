using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables;
using SHM_Smart_Hospital_Management_.Notifications;
using SHM_Smart_Hospital_Management_.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class Medical_TestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;
        public Medical_TestController(ApplicationDbContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ShowMedicalTestForPatient(int id, int PatId)// Medical_Detail (id)
        {
            var patient = await _context.Patients.FindAsync(PatId);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient");
            var test = (from mt in _context.Medical_Tests.ToList()
                        join t in _context.Tests
                        on mt.Test_Id equals t.Test_Id
                        where mt.Medical_Detail_Id == id
                        select new ShowMedicalTest
                        {
                            Test_Id = mt.Medical_Test_Id,
                            Date = mt.Test_Date.ToString("dd/MM/yyyy hh:mm tt"),
                            Test_Name = t.Test_Name,
                            Result = mt.Test_Result
                        }).ToList();
            ViewBag.MedicalDetailId = id;
            ViewBag.PatientId = PatId;
            return View(test);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> ShowMedicalTestForDoctor(int id, int DocId, int HoId)// Medical_Detail (id)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var test = await (from mt in _context.Medical_Tests
                        join t in _context.Tests
                        on mt.Test_Id equals t.Test_Id
                        where mt.Medical_Detail_Id == id
                        select new ShowMedicalTest
                        {
                            Date = mt.Test_Date.ToString("dd/MM/yyyy hh:mm tt"),
                            Test_Name = t.Test_Name,
                            Test_Id = mt.Test_Id,
                            Result = mt.Test_Result
                        }).ToListAsync();
            var medical = await _context.Medical_Details.Include(p => p.Patient).FirstOrDefaultAsync(md => md.Medical_Details_Id == id);
            ViewBag.PatientId = medical.Patient.Patient_Id;
            ViewBag.Medical_Detail_Id = id;
            ViewBag.DocId = DocId;
            ViewBag.HoId = HoId;
            return View(test);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Create(int id, int DocId, int HoId)
        {
            Medical_Test mt = new Medical_Test()
            {
                Medical_Detail_Id = id
            };
            ViewBag.Test_Type_Id =await _context.Test_Types.Select(s => new SelectListItem
            {
                Value = s.Test_Type_Id.ToString(),
                Text = s.Test_Type_Name
            }).ToListAsync();
            ViewBag.Tests = new List<SelectListItem>();
            ViewBag.HoId = HoId;
            ViewBag.DocId = DocId;
            return View(mt);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime[] date, int[] test, IFormFile[] files, int medicalDetailId, int DocId, int HoId)
        {
            if (ModelState.IsValid)
            {
                List<Medical_Test> tests = new List<Medical_Test>();
                try
                {
                    int i = 0;
                    foreach (var item in files)
                    {
                        string fileName = string.Empty;
                        if (item != null)
                        {
                            fileName = item.FileName;
                            string Medical_Test_Result = Path.Combine(_hosting.WebRootPath, "Medical_Test_Result");
                            string fullPath = Path.Combine(Medical_Test_Result, fileName);
                            FileStream f = new FileStream(fullPath, FileMode.OpenOrCreate);
                            item.CopyTo(f);
                            fileName = item.FileName;
                            tests.Add(new Medical_Test
                            {
                                Test_Result = fileName,
                                Test_Date = date[i],
                                Test_Id = test[i],
                                Medical_Detail_Id = medicalDetailId
                            });
                            i++;
                            f.Close();
                        }

                    }
                }
                catch { }

                await _context.AddRangeAsync(tests);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var p = _context.Medical_Details.Include(p => p.Patient).FirstOrDefault(p => p.Medical_Details_Id == medicalDetailId);
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تحليل جديد"},
                            { "body","تم إضافة  " + tests.Count + " تحاليل جديدة."},
                        }

                };
                await FCMService.SendNotificationToUserAsync(p.Patient.Patient_Id, UserType.pat, message);
                //=========================================================================================
                #endregion
                return RedirectToAction(nameof(ShowMedicalTestForDoctor), new { id = medicalDetailId, DocId, HoId });
            }
            return View();
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Delete(int id, int medicalId, int PatId)
        {
            var test = await _context.Medical_Tests.FindAsync(id);
            _context.Medical_Tests.Remove(test);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowMedicalTestForPatient), new { id = medicalId, PatId });
        }
        public async Task<IActionResult> GetTests(int Test_Type_Id)
        {
            if (Test_Type_Id != default)
            {
                List<SelectListItem> DiseasesSelect = await _context.Tests.Where(t => t.Test_Type_Id== Test_Type_Id)
                    .Select(n => new SelectListItem { Value = n.Test_Id.ToString(), Text = n.Test_Name}).ToListAsync();
                return Json(DiseasesSelect);
            }
            return null;
        }
    }

}
