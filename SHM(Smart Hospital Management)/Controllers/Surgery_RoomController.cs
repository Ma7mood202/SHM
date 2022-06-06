using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class Surgery_RoomController : Controller
    {
        private readonly ApplicationDbContext _context;

        public Surgery_RoomController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles ="IT")]
        public async Task<IActionResult> DisplaySurgeryRooms(int id, int HoId) // IT (id)
        {
            var IT =await _context.Employees.FindAsync(id);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            ViewBag.EmpId = id;
            ViewBag.HoId = HoId;
            return View(await _context.Surgery_Rooms.Where(sr => sr.Active && sr.Ho_Id == HoId).ToListAsync());
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> Create(int id, int HoId) // IT (id)
        {
            var IT = await _context.Employees.FindAsync(id);
            if (!IT.Active)
                return RedirectToAction("LogOut", "Employee");
            Surgery_Room surgery_Room = new Surgery_Room
            {
                Active = true,
                Ho_Id = HoId,
                Surgery_Room_Ready = true,
            };
            ViewBag.EmpId = id;
            return View(surgery_Room);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Surgery_Room surgery_Room, int EmpId)
        {
            if (ModelState.IsValid)
                return RedirectToAction("AddSurgeryRoom" , "Request" , new { EmpId , surgeryRoom = JsonConvert.SerializeObject(surgery_Room) });
            return View(surgery_Room);
        }
    }
}
