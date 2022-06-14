using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class BillController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BillController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ShowBillForPatient(int? id) // Patient (id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patient = await _context.Patients.FindAsync(id);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient", new { id = id });
            var bills = await _context.Bills.Where(b => b.Patient_Id == patient.Patient_Id).Select(s => new ShowBills
            {
                Bill = s,
                Total = (double)(s.Bill_Examination + s.Bill_Medical_Test + s.Bill_Medication + s.Bill_Rays + s.Bill_Room_Service + s.Bill_Surgeries),
                FullName = patient.Patient_Full_Name
            }).ToListAsync();
            ViewBag.PatientId = id;
            return View(bills);
        }
        [Authorize(Roles ="Resception")]
        public async Task<IActionResult> ShowBillForResception(int? id, int EmpId) // Patient (id)
        {
            var Resception = _context.Employees.Find(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut", "Employee" ,new {id =EmpId });
            var patient = await _context.Patients.FindAsync(id);
            if (patient is null)
                return NotFound();
            var bills = await _context.Bills.Where(b => b.Patient_Id == id && b.Paid == false)
                .Select(s => new ShowBills
                {
                    Bill = s,
                    Total=(double)( s.Bill_Examination + s.Bill_Medical_Test + s.Bill_Medication + s.Bill_Rays + s.Bill_Room_Service + s.Bill_Surgeries),
                    FullName = patient.Patient_Full_Name
                }).ToListAsync();
            ViewBag.PatientId = id;
            ViewBag.EmpId = EmpId;
            ViewBag.HospitalId = patient.Ho_Id;
            return View(bills);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> PayBill(int id, int PatId, int EmpId) // Bill (id)
        {
            var Resception = _context.Employees.Find(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut", "Employee",new {id = EmpId });
            var bill = _context.Bills.Find(id);
            bill.Paid = true;
            _context.Update(bill);
            await _context.SaveChangesAsync();
            #region send notification
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                    {
                        { "channelId","other" },
                        { "title","تم دفع الفاتورة" },
                        { "body","تم دفع الفاتورة بتاريخ " + bill.Bill_Date.ToShortDateString() },
                    }

            };

            await FCMService.SendNotificationToUserAsync(bill.Patient_Id, UserType.pat, message);

            #endregion

            return RedirectToAction("ShowBillForResception", new { id = PatId, EmpId = EmpId });
        }
        [Authorize(Roles = "Resception")]
        public IActionResult Create(int id, int EmpId) // Patient (id)
        {
            var Resception = _context.Employees.Find(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut", "Employee",new {id =EmpId });
            var patient = _context.Patients.FirstOrDefault(m => m.Patient_Id == id);
            Bill b = new Bill()
            {
                Patient_Id = id,
                Bill_Date = DateTime.Now
            };
            ViewBag.HospitalId = patient.Ho_Id;
            ViewBag.EmpId = EmpId;

            return View(b);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bill bill, int id, int EmpId)
        {
            if (ModelState.IsValid)
            {
                var HoId = _context.Employees.Find(EmpId).Ho_Id;
                _context.Add(bill);
                await _context.SaveChangesAsync();
                return RedirectToAction("HoPatientsForBill", "Patient", new { id = HoId, EmpId });
            }
            return View(bill);
        }
    }
}
