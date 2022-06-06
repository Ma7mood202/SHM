using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SHM_Smart_Hospital_Management_.Data;
using System;
using System.Collections.Generic;
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
        public IActionResult Index()
        {
            return View();
        }
    }
}
