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
    public class Medical_RayController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;
        public Medical_RayController(ApplicationDbContext context , IWebHostEnvironment hosting )
        {
            _context = context;
            _hosting = hosting;
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> ShowRaysForDoctor(int id, int DocId, int HoId)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor", new { id = DocId });
            var rays = await (from r in _context.Medical_Rays
                              join ry in _context.Ray_Types
                              on r.Ray_Type_Id equals ry.Ray_Type_Id
                              where r.Medical_Detail_Id == id
                              select new ShowRays
                              {
                                  RayId = r.Ray_Id,
                                  RayDate = r.Ray_Date.ToString("dd-MM-yyyy"),
                                  RayResult = r.Ray_Result,
                                  RayType = ry.Ray_Type_Name,
                              }).ToListAsync();
            var medical = await _context.Medical_Details.Include(p => p.Patient).FirstOrDefaultAsync(md => md.Medical_Details_Id == id);
            ViewBag.PatientId = medical.Patient.Patient_Id;
            ViewBag.Medical_Detail_Id = id;
            ViewBag.DocId = DocId;
            ViewBag.HoId = HoId;
            return View(rays);
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ShowRaysForPatient(int id, int PatId)
        {
            var patient = await _context.Patients.FindAsync(PatId);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient" , new { id = PatId});
            var rays = await (from r in _context.Medical_Rays
                              join ry in _context.Ray_Types
                              on r.Ray_Type_Id equals ry.Ray_Type_Id
                              where r.Medical_Detail_Id == id
                              select new ShowRays
                              {
                                  RayId = r.Ray_Id,
                                  RayDate = r.Ray_Date.ToString("dd-MM-yyyy"),
                                  RayResult = r.Ray_Result,
                                  RayType = ry.Ray_Type_Name,
                              }).ToListAsync();
            ViewBag.PatientId = PatId;
            ViewBag.MedicalDetailId = id;
            return View(rays);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Create(int id, int DocId, int HoId)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor", new { id = DocId });
            Medical_Ray ray = new Medical_Ray
            {
                Medical_Detail_Id = id
            };
           var types = _context.Ray_Types.Select(s => new SelectListItem
            {
                Value = s.Ray_Type_Id.ToString(),
                Text = s.Ray_Type_Name
            }).ToList();
            ViewBag.Ray_Type_Id = types;
            ViewBag.DocId = DocId;
            ViewBag.HoId = HoId;
            return View(ray);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime[] date, int[] ray, IFormFile[] files, int medicalDetailId, int DocId, int HoId)
        {
            if (ModelState.IsValid)
            {
                List<Medical_Ray> rays = new List<Medical_Ray>();
                try
                {
                    int i = 0;
                    foreach (var item in files)
                    {
                        string fileName = string.Empty;
                        if (item != null)
                        {
                            fileName = item.FileName;
                            string Ray_Result = Path.Combine(_hosting.WebRootPath, "Ray_Result");
                            string fullPath = Path.Combine(Ray_Result, fileName);
                            FileStream f = new FileStream(fullPath, FileMode.OpenOrCreate);
                            item.CopyTo(f);
                            fileName = item.FileName;
                            rays.Add(new Medical_Ray
                            {
                                Ray_Result = fileName,
                                Ray_Date = date[i],
                                Ray_Type_Id = ray[i],
                                Medical_Detail_Id = medicalDetailId
                            });
                            i++;
                            f.Close();
                        }

                    }
                }
                catch { }

                await _context.AddRangeAsync(rays);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var pat = await _context.Medical_Details.Include(p => p.Patient).FirstOrDefaultAsync(p => p.Medical_Details_Id ==medicalDetailId);
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "channelId","other" },
                        { "title", "صورة أشعة جديدة"},
                        { "body","تم إضافة  "+ rays.Count+" صورة جديدة" },
                    }
                };
                await FCMService.SendNotificationToUserAsync(pat.Patient.Patient_Id, UserType.pat, message);
                //=========================================================================================
                #endregion
                return RedirectToAction(nameof(ShowRaysForDoctor), new { id = medicalDetailId, DocId, HoId });
            }
            return View();
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Delete(int id,int medicalId, int PatId)
        {
            var patient = await _context.Patients.FindAsync(PatId);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient", new { id = PatId });
            var ray = await _context.Medical_Rays.FindAsync(id);

            _context.Medical_Rays.Remove(ray);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowRaysForPatient), new { id = medicalId, PatId });
        }
    }
}
