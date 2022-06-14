using Microsoft.EntityFrameworkCore;
using SHM_Smart_Hospital_Management_.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.HangFire
{
    public static class TimingOperations
    {
        private static DbContextOptions<ApplicationDbContext> opt = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseSqlServer(@"Server=DESKTOP-G3NE69C\SQLEXPRESS;Database=LastHMS2;trusted_connection=yes;").Options;

        private static readonly ApplicationDbContext context = new ApplicationDbContext(opt);
        public static void EmptySurgeryRooms()
        {
            int munite = (DateTime.Now.Hour * 60) + DateTime.Now.Minute;
            var surgeries = context.Surgeries.Where(s => s.Surgery_Date.Day == DateTime.Now.Day).ToList();
            foreach (var item in surgeries)
            {
                int x = (item.Surgery_Date.Hour + item.Surgery_Time.Hours) * 60 + item.Surgery_Date.Minute + item.Surgery_Time.Minutes + 30;
                if (x == munite)
                {
                    context.Surgery_Rooms.Find(item.Surgery_Room_Id).Surgery_Room_Ready = true;
                    context.SaveChanges();
                }
            }

        }

        public async static Task AlterPreviews()
        {
            var patients = context.Patients.ToList();
            foreach (var item in patients)
            {
                item.Canceled = false;
                item.PreviewCount = 0;
            }
            context.UpdateRange(patients);
            await context.SaveChangesAsync();
        }

        public async static Task DeleteAcceptedRequests()
        {
            var requests = context.Requests.Where(r => r.Accept == true);
            context.RemoveRange(requests);
            await context.SaveChangesAsync();
        }
        public async static Task AlterSentColumn()
        {
            foreach (var item in context.Patients.Where(p => p.Active && p.Sent))
            {
                item.Sent = false;
            }
            await context.SaveChangesAsync();
        }

        public async static Task DeletePaidBills()
        {
            context.Bills.RemoveRange( await context.Bills.Where(b => DateTime.Now.Date - b.Bill_Date > TimeSpan.FromDays(20) && b.Paid).ToListAsync());
            await context.SaveChangesAsync();
        }
    }
}
