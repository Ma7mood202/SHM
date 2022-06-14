using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    public class SurgeryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurgeryController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Create Surgery For Doctor
        [Authorize(Roles ="Doctor,DeptManager")]
        public async Task<IActionResult> DisplaySurgeries(int id, int HoId) //Doctor(id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var surgeries = await (from s in _context.Surgeries
                                   join p in _context.Patients
                                   on s.Patient_Id equals p.Patient_Id
                                   where s.Doctor_Id == id &&
                                   s.Surgery_Date.Date >= DateTime.Now.Date &&
                                   p.Active
                                   select s
                            ).ToListAsync();

            var data = (from s in surgeries
                        join sr in _context.Surgery_Rooms.ToList()
                        on s.Surgery_Room_Id equals sr.Surgery_Room_Id
                        select new DisplaySurguries
                        {
                            Id = s.Surgery_Number,
                            SurgeryName = s.Surgery_Name,
                            Floor = sr.Su_Room_Floor,
                            PatientName = _context.Patients.FirstOrDefault(p => p.Patient_Id == s.Patient_Id && p.Active).Patient_Full_Name,
                            PhoneNumbers = _context.Patient_Phone_Numbers.Where(pn => pn.Patient_Id == s.Patient_Id).ToList(),
                            RoomNumber = sr.Su_Room_Number,
                            SurguryDate = s.Surgery_Date.ToString("dd/MM/yyyy hh:mm tt"),
                            SurgeryLength = s.Surgery_Time.ToString("c").Substring(0,5)
                        }).ToList();
            ViewBag.HoId = HoId;
            ViewBag.DocId = id;
            return View(data);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> DisplayEmptySurgeryRooms(int id, int HoId) //Docotor(id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            ViewBag.DocId = id;
            ViewBag.HoId = HoId;
            var EmptySurgeryRooms = await _context.Surgery_Rooms.Where(sr => sr.Surgery_Room_Ready == true && sr.Ho_Id == HoId).ToListAsync();
            return View(EmptySurgeryRooms);
        }
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> DisplayPatients(int id, int DocId, int HoId , string search="") //Surgery_Room(id)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            ViewBag.SrId = id;
            ViewBag.HoId = HoId;
            ViewBag.DocId = DocId;
            var patients = await _context.Patients.Where(p => p.Ho_Id == HoId && p.Active).ToListAsync();
            if (string.IsNullOrEmpty(search))
                return View(patients);
            patients = patients.Where(p => p.Patient_Full_Name.Contains(search)).ToList();
            return View(patients);
        }

        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Create(int DocId, int SrId, int PatId, int HoId, string ErrorMessageDate = "", string ErrorMessageTime = "")
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            ViewBag.SrId = SrId;
            ViewBag.HoId = HoId;
            ViewBag.DocId = DocId;
            ViewBag.PatId = PatId;
            ViewBag.ErrorMessage = ErrorMessageDate;
            ViewBag.ErrorMessageTime = ErrorMessageTime;
            ViewBag.Times = new List<SelectListItem>();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(int HoId, int PatId, int SrId, int DocId, int hour, int minute, DateTime date, TimeSpan time, string name)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            ViewBag.SrId = SrId;
            ViewBag.HoId = HoId;
            ViewBag.DocId = DocId;
            ViewBag.PatId = PatId;

            if (date < DateTime.Now || date == default )
                return RedirectToAction("Create", new { DocId, SrId, PatId, HoId, ErrorMessageDate = "الرجاء اختيار تاريخ صالح" });
            else if (time.ToString() == "12:02:00")
            {
                return RedirectToAction("Create", new { DocId, SrId, PatId, HoId, ErrorMessageTime = "الرجاء اختيار الوقت " });
            }

            DateTime d = date.Date.Add(time);
            TimeSpan t = TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(minute);
            Surgery s = new Surgery
            {
                Doctor_Id = DocId,
                Patient_Id = PatId,
                Surgery_Date = d,
                Surgery_Room_Id = SrId,
                Surgery_Time = t,
                Surgery_Name = name
            };
            #region send notification
            //==============================================================================================
            var doc = await _context.Doctors.FindAsync(DocId);
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    { "channelId","other" },
                    { "title", "تم تحديد موعد عمليتك"},
                    { "body","عند الطبيب "+doc.Doctor_Full_Name },
                }
            };
            await FCMService.SendNotificationToUserAsync(PatId, UserType.pat, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("AddSurgery", "Request", new { id = DocId, HoId = HoId, surgery = JsonConvert.SerializeObject(s) });
        }

        #endregion
        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> Delete(int? id, int DocId, int HoId)
        {
            var doctor = await _context.Doctors.FindAsync(DocId);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var surgery = await _context.Surgeries.FindAsync(id);
            _context.Surgeries.Remove(surgery);
            await _context.SaveChangesAsync();
            #region send notification
            //==============================================================================================
            var doc = await _context.Doctors.FirstOrDefaultAsync(d => d.Doctor_Id == surgery.Doctor_Id);
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    { "channelId","other" },
                    { "title", "ألغيت العملية"},
                    { "body","تم إلغاء العملية عند الطبيب " + doc.Doctor_Full_Name },
                }
            };
            await FCMService.SendNotificationToUserAsync((int)surgery.Patient_Id, UserType.pat, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("DisplaySurgeries", new { id = DocId, HoId });
        }
        public async Task<IActionResult> GetTimes(DateTime date, int Sr_Id, int hour, int minute)
        { // "2022-12-10"

            if (date != default)
            {
                var su = await _context.Surgeries.Where(s => s.Surgery_Room_Id == Sr_Id && s.Surgery_Date.Date >= DateTime.Now.Date).ToListAsync();
                List<TimeSpan> DateSurgery = new List<TimeSpan>();
                List<TimeSpan> time = new List<TimeSpan>();
                List<TimeSpan> Invalid = new List<TimeSpan>();
                TimeSpan temp;
                TimeSpan halfHour = TimeSpan.FromMinutes(30);
                Dictionary<string, string> d = new Dictionary<string, string>();
                var surgeries = su.Count != 0 ? su.Where(s => s.Surgery_Date.ToString("yyyy-MM-dd") == date.ToString("yyyy-MM-dd")).ToList() : new List<Surgery>();
                if (surgeries.Count == 0)
                {
                    for (TimeSpan i = TimeSpan.Zero; i < TimeSpan.FromDays(1); i += halfHour)
                    {
                        temp = TimeSpan.FromHours(i.Hours + hour) + TimeSpan.FromMinutes(i.Minutes + minute);
                        if (date.Date == DateTime.Now.Date)
                        {
                            if (temp < TimeSpan.FromDays(1) && i.Hours > DateTime.Now.Hour)
                                d.Add(i.ToString("c"), i.Hours >= 12 ? i.Hours == 12 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours - 12 + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours == 0 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " AM" : i.Hours + ":" + (i.Minutes == 0 ? "00" : "30") + "AM");

                        }
                        else
                        {
                            if (temp < TimeSpan.FromDays(1))
                                d.Add(i.ToString("c"), i.Hours >= 12 ? i.Hours == 12 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours - 12 + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours == 0 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " AM" : i.Hours + ":" + (i.Minutes == 0 ? "00" : "30") + "AM");
                        }
                    }
                }
                else
                {
                    foreach (var item in surgeries)
                    {
                        TimeSpan t = TimeSpan.FromHours(item.Surgery_Date.Hour) + TimeSpan.FromMinutes(item.Surgery_Date.Minute);
                        TimeSpan t1 = TimeSpan.FromHours(item.Surgery_Time.Hours + item.Surgery_Date.Hour) + TimeSpan.FromMinutes(item.Surgery_Time.Minutes + item.Surgery_Date.Minute);
                        DateSurgery.Add(t);
                        time.Add(t1);
                    }
                    bool check = false;
                    for (int i = 0; i < surgeries.Count; i++)
                    {
                        for (TimeSpan j = TimeSpan.Zero; j < TimeSpan.FromDays(1); j += halfHour)
                        {
                            temp = TimeSpan.FromHours(j.Hours + hour) + TimeSpan.FromMinutes(j.Minutes + minute);
                            if ((j >= DateSurgery[i] && j <= time[i]) || (temp >= DateSurgery[i] && temp <= time[i]) || j.Hours <= DateTime.Now.Hour || temp > TimeSpan.FromDays(1))
                            {
                                Invalid.Add(j);
                            }
                            for (TimeSpan ti = j; ti < temp; ti += halfHour)
                            {
                                if (ti == DateSurgery[i])
                                    check = true;
                            }
                            if (check)
                            {
                                Invalid.Add(j);
                            }
                            check = false;
                        }

                    }

                    for (TimeSpan i = TimeSpan.Zero; i < TimeSpan.FromDays(1); i += halfHour)
                    {
                        if (!Invalid.Contains(i))
                        {
                            d.Add(i.ToString("c"), i.Hours >= 12 ? i.Hours == 12 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " PM " : (i.Hours - 12) + ":" + (i.Minutes == 0 ? "00" : "30") + " PM " : i.Hours == 0 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " AM " : i.Hours + ":" + (i.Minutes == 0 ? "00" : "30") + " AM ");
                        }
                    }

                }
                List<SelectListItem> TimeSelect = null;
                string message = "";
                if (d.Count == 0)
                {
                    message = "ليس هناك وقت متاح في هذا اليوم";
                    return Json(new { TimeSelect, message });
                }
                TimeSelect = d
                   .Select(n => new SelectListItem { Value = n.Key.ToString(), Text = n.Value }).ToList();
                return Json(new { TimeSelect, message });
            }
            return null;
        }

    }
}
