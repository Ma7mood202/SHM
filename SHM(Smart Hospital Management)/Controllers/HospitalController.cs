using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{

    public class HospitalController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitalController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            Hospital h = new Hospital
            {
                Active = true,
                Ho_Subscribtion_Date = DateTime.Now,
            };
            ViewBag.Cities = await _context.Cities.Select(c => new SelectListItem { Value = c.City_Id.ToString(), Text = c.City_Name }).ToListAsync();
            ViewBag.Areas = new List<SelectListItem>();
            return View(h);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hospital hospital, string[] pn)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();

                List<Hospital_Phone_Numbers> pns = new List<Hospital_Phone_Numbers>();
                for (int i = 0; i < pn.Length; i++)
                {
                    if (pn[i] is not null)
                    {
                        pns.Add(new Hospital_Phone_Numbers
                        {
                            Ho_Id = hospital.Ho_Id,
                            Hospital_Phone_Number = pn[i]
                        });
                    }
                }
                await _context.AddRangeAsync(pns);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreateManager", "Employee", new { id = hospital.Ho_Id });
            }
            return View(hospital);
        }
        public async Task<IActionResult> GetAreas(string CityId)
        {
            if (!string.IsNullOrWhiteSpace(CityId))
            {
                List<SelectListItem> AreaSelect = await _context.Areas.Where(a => a.City_Id.ToString() == CityId)
                    .Select(n => new SelectListItem { Value = n.Area_Id.ToString(), Text = n.Area_Name }).ToListAsync();
                return Json(AreaSelect);
            }
            return null;
        }
    }
}
