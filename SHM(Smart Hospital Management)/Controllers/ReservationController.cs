﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles ="Resception")]
        public async Task<IActionResult> Create(int RoomId, int PatientId, int ReservationBedNumber, int EmptyBedCount, int EmpId)
        {
            var Resception = await _context.Employees.FindAsync(EmpId);
            if (!Resception.Active)
                return RedirectToAction("Logout", "Employee");
            var Room = await _context.Rooms.FindAsync(RoomId);
            Reservation res = new Reservation()
            {
                Patient_Id = PatientId,
                Room_Id = RoomId,
                End_Date = DateTime.MinValue,
                Start_Date = DateTime.Now
            };
            if (ReservationBedNumber == EmptyBedCount)
                Room.Room_Empty = false;
            _context.Update(Room);
             _context.Add(res);
            await _context.SaveChangesAsync();
            TempData["Message"] = "تم الحجز بنجاح";
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
    }
}
