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
    public class Work_DaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Work_DaysController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> GetWorkDays(int id, int HoId, int DeptMgrId = 0)
        {
            ViewBag.HoId = HoId;
            int x = id;
            Doctor doctor;
            bool isDeptManager = false;
            if (DeptMgrId != 0)
            {
                doctor = await _context.Doctors.FindAsync(DeptMgrId);
                var dept =  _context.Departments.Where(d => d.Department_Id == doctor.Department_Id).Include(d => d.Dept_Manager).ToArray()[0];
                if (!doctor.Active && doctor.Doctor_Id == dept.Dept_Manager.Doctor_Id)
                    return RedirectToAction("LogOut", "Doctor");
                isDeptManager = true;
                x = DeptMgrId;
                ViewBag.DoctorId = id;
            }
            doctor = await _context.Doctors.FindAsync(id);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");

            ViewBag.IsDeptManager = isDeptManager;
            ViewBag.DocId = x;
            ViewBag.DoctorName = _context.Doctors.Find(id).Doctor_Full_Name;
            return View(await _context.Work_Days.Where(w => w.Doctor_Id == id).Select(s =>
            new ShowWorkDays
            {
                Doctor_Id = s.Doctor_Id,
                Day = s.Day,
                End_Hour = new DateTime(2000, 3, 3, s.End_Hour.Hours, s.End_Hour.Minutes, s.End_Hour.Seconds).ToString("hh : mm tt"),
                Start_Hour = new DateTime(2000, 3, 3, s.Start_Hour.Hours, s.Start_Hour.Minutes, s.Start_Hour.Seconds).ToString("hh : mm tt")
            }).ToListAsync());
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> ShowDoctorWorkDays(int id, int EmpId, int HoId)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut", "Employee");
            ViewBag.HoId = HoId;
            ViewBag.EmpId = EmpId;
            ViewBag.DoctorName = _context.Doctors.Find(id).Doctor_Full_Name;
            return View(await _context.Work_Days.Where(w => w.Doctor_Id == id).Select(s=>
            new ShowWorkDays
            {
                Doctor_Id = s.Doctor_Id,
                Day = s.Day,
                End_Hour = new DateTime(2000, 3, 3, s.End_Hour.Hours, s.End_Hour.Minutes, s.End_Hour.Seconds).ToString("hh : mm tt"),
                Start_Hour = new DateTime(2000, 3, 3, s.Start_Hour.Hours, s.Start_Hour.Minutes, s.Start_Hour.Seconds).ToString("hh : mm tt")
            }).ToListAsync());
        }
        [Authorize(Roles = "DeptManager")]
        public IActionResult Create(int id, int HoId, int DeptMgrId)
        {
            Work_Days wd = new Work_Days
            {
                Doctor_Id = id
            };
            ViewBag.DeptMgrId = DeptMgrId;
            ViewBag.HoId = HoId;
            return View(wd);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int[] Day, TimeSpan[] sdate, TimeSpan[] edate, int DoctorId, int HoId, int DeptMgrId)
        {
            bool valid = true;
            string ErrorMessage = "s";

            for (int i = 0; i < Day.Length - 1; i++)
            {
                for (int j = i + 1; j < Day.Length; j++)
                {
                    if (Day[i] == Day[j]) { valid = false; ErrorMessage = "you Cant repeat the day !!"; }
                }
            }
            if (valid)
            {
                var DoctorWorkDays = _context.Work_Days.Where(w => w.Doctor_Id == DoctorId).ToArray();
                for (int i = 0; i < Day.Length; i++)
                {
                    for (int j = 0; j < DoctorWorkDays.Length; j++)
                    {
                        if (Day[i] == (int)DoctorWorkDays[j].Day)
                        { valid = false; ErrorMessage = "This Day already exists in the data base !!"; }
                    }
                }
            }
            if (valid)
            {
                Work_Days[] wd = new Work_Days[Day.Length];
                for (int i = 0; i < wd.Length; i++)
                {
                    wd[i] = new Work_Days()
                    {
                        Day = (Enums.WeekDays)Day[i],
                        Start_Hour = sdate[i],
                        End_Hour = edate[i],
                        Doctor_Id = DoctorId
                    };
                }
                await _context.AddRangeAsync(wd);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "يمكنك الآن تفقد جدول دوامك"},
                            { "body","تم إضافة جدول الدوام"},
                        }
                };
                await FCMService.SendNotificationToUserAsync(DoctorId, UserType.doc, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("GetWorkDays", new { id = DoctorId, HoId, DeptMgrId });
            }
            ViewBag.DeptMgrId = DeptMgrId;
            ViewBag.HoId = HoId;
            TempData["ErrorMessage"] = ErrorMessage;
            return View(new Work_Days { Doctor_Id = DoctorId });
        }
        [Authorize(Roles = "DeptManager")]
        public async Task<IActionResult> Edit(int id, int Day, int DeptMgrId, int HoId)
        {
            var doctor = await _context.Doctors.FindAsync(DeptMgrId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var work_Day = await _context.Work_Days.FirstOrDefaultAsync(wd => wd.Doctor_Id == id && (int)wd.Day == Day);
            ViewBag.DeptMgrId = DeptMgrId;
            ViewBag.HoId = HoId;
            return View(work_Day);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Work_Days work_Days, int DeptMgrId, int HoId)
        {
            if (ModelState.IsValid)
            {
                _context.Update(work_Days);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title","تفقد جدول دوامك"},
                            { "body","تم تعديل جدول الدوام" },
                        }
                };
                await FCMService.SendNotificationToUserAsync(work_Days.Doctor_Id, UserType.doc, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("GetWorkDays", new { id = work_Days.Doctor_Id, HoId, DeptMgrId });
            }
            return View(work_Days);
        }
        [Authorize(Roles = "DeptManager")]
        public async Task<IActionResult> Delete(int id, int Day, int HoId, int DeptMgrId)
        {
            var doctor = await _context.Doctors.FindAsync(DeptMgrId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var work_Day = await _context.Work_Days.FirstOrDefaultAsync(wd => wd.Doctor_Id == id && (int)wd.Day == Day);
            _context.Work_Days.Remove(work_Day);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetWorkDays", new { id = work_Day.Doctor_Id, HoId, DeptMgrId });
        }
    }
}
