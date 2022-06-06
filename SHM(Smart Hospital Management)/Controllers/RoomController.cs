using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class RoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoomController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> AvailableRooms(int? id, int EmpId) // Patient (id)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("LogOut");
            if (id == null) return NotFound();
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();
            var EmptyRooms = await _context.Rooms.Where(r => r.Ho_Id == patient.Ho_Id && r.Room_Empty == true).ToListAsync();
            var ValidReservations = await _context.Reservations.Where(res => res.End_Date == DateTime.MinValue).ToListAsync();
            var rooms = (from room in EmptyRooms
                         join res in ValidReservations
                         on room.Room_Id equals res.Room_Id into GroupedReservations
                         where room.Room_Beds_Count > ValidReservations.Where(res => res.Room_Id == room.Room_Id).Count()
                         from gr in GroupedReservations.DefaultIfEmpty()
                         select new
                         {
                             Room = room,
                             ReservationsCount = ValidReservations.Where(res => res.Room_Id == room.Room_Id).Count() < room.Room_Beds_Count ? ValidReservations.Where(res => res.Room_Id == room.Room_Id).Count() : -1
                         } into a
                         group a by a.Room into b
                         select new
                         {
                             Room = b.Key,
                             ReservationsCount = ValidReservations.Where(res => res.Room_Id == b.Key.Room_Id).Count() < b.Key.Room_Beds_Count ? ValidReservations.Where(res => res.Room_Id == b.Key.Room_Id).Count() : -1
                         }).ToList();


            var rooms1 = (from r in rooms
                          where r.ReservationsCount != -1
                          select new AvailableRoom
                          {
                              Room = r.Room,
                              ReservationsCount = r.ReservationsCount,
                              EmptyBedCount = r.Room.Room_Beds_Count - r.ReservationsCount
                          }).ToList();
            ViewBag.PatientId = patient.Patient_Id;
            ViewBag.EmployeeId = EmpId;
            ViewBag.HoId = patient.Ho_Id;
            return View(rooms1);
        }
        [Authorize(Roles = "Resception")]
        public async Task<IActionResult> BusyRooms(int id) 
        {
            var rooms = await _context.Rooms.Where(r => r.Ho_Id == id).ToListAsync();
            var data = (from res in _context.Reservations.ToList()
                        join room in rooms
                        on res.Room_Id equals room.Room_Id
                        where res.End_Date == DateTime.MinValue
                        select new { res, room }
                        into a
                        join p in _context.Patients.ToList()
                        on a.res.Patient_Id equals p.Patient_Id
                        select new BusyRooms
                        {
                            RoomNumber = a.room.Room_Number,
                            StartDate = a.res.Start_Date,
                            PatientName = p.Patient_First_Name + " " + p.Patient_Last_Name
                        }).ToList();
            return View(data);
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DisplayRooms(int id, int HoId) // IT(id)
        {
            var IT = await _context.Employees.FindAsync(id);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            ViewBag.EmpId = id;
            ViewBag.HoId = HoId;
            return View(await _context.Rooms.Where(r => r.Active && r.Ho_Id == HoId).ToListAsync());
        }
        [Authorize(Roles = "IT")]
        public IActionResult Create(int HoId, int EmpId)
        {
            var IT = _context.Employees.Find(EmpId);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            Room r = new Room
            {
                Active = true,
                Ho_Id = HoId,
                Room_Empty = true
            };
            ViewBag.EmpId = EmpId;
            return View(r);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room, int EmpId)
        {
            if (ModelState.IsValid)
                return RedirectToAction("AddRoom", "Request", new { EmpId, room = JsonConvert.SerializeObject(room) });
            return View(room);
        }
    }
}
