using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Break_Tables;
using SHM_Smart_Hospital_Management_.CityAndArea;
using SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Specialization> Specializations { get; set; }
        // Models
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Death_Case> Death_Cases { get; set; }
        public DbSet<Medical_Detail> Medical_Details { get; set; }
        public DbSet<Medical_Test> Medical_Tests { get; set; }
        public DbSet<Medical_Ray> Medical_Rays { get; set; }
        public DbSet<Preview> Previews { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Surgery_Room> Surgery_Rooms { get; set; }
        public DbSet<Surgery> Surgeries { get; set; }
        public DbSet<Work_Days> Work_Days { get; set; }


        // Medidcal_deatails Tables

        public DbSet<External_Records> External_Records { get; set; }
        public DbSet<Allergy> Allergies { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Test_Type> Test_Types { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Ray_Type> Ray_Types { get; set; }
        public DbSet<Disease_Type> Diseases_Types { get; set; }
        // Break Tables
        public DbSet<Medical_Allergy> Medical_Allergies { get; set; }
        public DbSet<Medical_Disease> Medical_Diseases { get; set; }
        // City And Area
        public DbSet<City> Cities { get; set; }
        public DbSet<Area> Areas { get; set; }
        // Class Attriputes
        public DbSet<Patient_Phone_Numbers> Patient_Phone_Numbers { get; set; }
        public DbSet<Hospital_Phone_Numbers> Hospital_Phone_Numbers { get; set; }
        public DbSet<Doctor_Phone_Numbers> Doctor_Phone_Numbers { get; set; }
        public DbSet<Employee_Phone_Numbers> Employee_Phone_Numbers { get; set; }
        public object Admins { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient_Phone_Numbers>().HasKey(o => new { o.Patient_Id, o.Patient_Phone_Number });
            modelBuilder.Entity<Doctor_Phone_Numbers>().HasKey(o => new { o.Doctor_Id, o.Doctor_Phone_Number });
            modelBuilder.Entity<Hospital_Phone_Numbers>().HasKey(o => new { o.Ho_Id, o.Hospital_Phone_Number });
            modelBuilder.Entity<Employee_Phone_Numbers>().HasKey(o => new { o.Employee_Id, o.Employee_Phone_Number });
            modelBuilder.Entity<Work_Days>().HasKey(w => new { w.Doctor_Id, w.Day });
            modelBuilder.Entity<Medical_Allergy>().HasKey(ma => new { ma.Allergy_Id, ma.Medical_Detail_Id });
            modelBuilder.Entity<Medical_Disease>().HasKey(md => new { md.Disease_Id, md.Medical_Detail_Id });
        }
    }
}
