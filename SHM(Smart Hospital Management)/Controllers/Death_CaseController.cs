using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class Death_CaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;

        public Death_CaseController(ApplicationDbContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }
        [Authorize(Roles ="Resception")]
        public IActionResult Create(int id, int EmpId) // Patient (id)
        {
            var Resception = _context.Employees.Find(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut","Employee", new { id = EmpId });

            Death_Case d = new Death_Case
            {
                PatientId = id,
                Death_Cause = "mmmmmmmmmmmmmm"
            };
            ViewBag.HoId = Resception.Ho_Id;
            ViewBag.EmpId = EmpId;
            return View(d);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Death_Case death_Case, int EmpId, int HoId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = string.Empty;
                    if (death_Case.Image != null)
                    {
                        string Medical_Test_Result = Path.Combine(_hosting.WebRootPath, "Death_Causes");
                        fileName = death_Case.Image.FileName;
                        string fullPath = Path.Combine(Medical_Test_Result, fileName);
                        FileStream f = new FileStream(fullPath, FileMode.OpenOrCreate);
                        death_Case.Image.CopyTo(f);
                        death_Case.Death_Cause = fileName;
                        f.Close();
                    }
                }
                catch { }
                var patient = await _context.Patients.FindAsync(death_Case.PatientId);
                patient.Active = false;
                death_Case.Dead_Patient = patient;
                _context.Add(death_Case);
                await _context.SaveChangesAsync();
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "channelId","other" },
                        { "title","وفاة مريض" },
                        { "body"," تم تسجيل وفاة المريض بتاريخ " + death_Case.Death_Date.ToShortDateString() },
                    }
                };

                await FCMService.SendNotificationToUserAsync(death_Case.Dead_Patient.Patient_Id, UserType.pat, message);

                return RedirectToAction("HoPatientsForResception", "Patient", new { id = HoId, EmpId });
            }
            ViewBag.HoId = HoId;
            ViewBag.EmpId = EmpId;
            return View(death_Case);
        }
    }
}
