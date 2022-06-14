using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class External_RecordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;
        public External_RecordController(ApplicationDbContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }
        [Authorize(Roles ="Patient")]
        public async Task<IActionResult> ShowExternalRecordsPatient(int id , int PatId)//medical id
        {
            var externalRecords =await _context.External_Records.Where(e => e.Medical_Detail_Id == id).ToListAsync();
            ViewBag.MedicalDetailId = id;
            ViewBag.PatientId = PatId;
            return View(externalRecords);
        }
        [Authorize(Roles ="Doctor,DeptManager")]
        public async Task<IActionResult> ShowExternalRecordsForDoctor(int id, int DocId, int HoId)//medical id
        {
            var externalRecords =await _context.External_Records.Where(e => e.Medical_Detail_Id == id).ToListAsync();
            var medical = await _context.Medical_Details.Include(p => p.Patient).FirstOrDefaultAsync(md => md.Medical_Details_Id == id);
            ViewBag.PatientId = medical.Patient.Patient_Id;
            ViewBag.Medical_Detail_Id = id;
            ViewBag.DocId = DocId;
            ViewBag.HoId = HoId;
            return View(externalRecords);
        }
        [Authorize(Roles = "Patient")]
        public IActionResult Create(int id, int PatId)
        {
            var externalRecord = new External_Records
            {
                Medical_Detail_Id = id
            };
            ViewBag.PatId = PatId;
            return View(externalRecord);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime[] date, IFormFile[] files, int medicalDetailId,int PatId)
        {
            if (ModelState.IsValid)
            {
                List<External_Records> externalRecords = new List<External_Records>();
                try
                {
                    int i = 0;
                    foreach (var item in files)
                    {
                        string fileName = string.Empty;
                        if (item != null)
                        {
                            fileName = item.FileName;
                            string Details = Path.Combine(_hosting.WebRootPath, "External_Records_Details");
                            string fullPath = Path.Combine(Details, fileName);
                            FileStream f = new FileStream(fullPath, FileMode.OpenOrCreate);
                            item.CopyTo(f);
                            fileName = item.FileName;
                            externalRecords.Add(new External_Records
                            {
                                Details = fileName,
                                Date = date[i],
                                Medical_Detail_Id = medicalDetailId
                            });
                            i++;
                            f.Close();
                        }

                    }
                }
                catch { }

                await _context.AddRangeAsync(externalRecords);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ShowExternalRecordsPatient), new { id = medicalDetailId,PatId });
            }
            ViewBag.PatId = PatId;
            return View(new External_Records { Medical_Detail_Id = medicalDetailId });
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Delete(int id ,int medicalId, int PatId)
        {
            var externalRecord = await _context.External_Records.FindAsync(id);
            _context.External_Records.Remove(externalRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ShowExternalRecordsPatient), new { id=medicalId, PatId });
        }
    }
}
