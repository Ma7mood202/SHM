using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.HangFire;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DbConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true
            }));
            services.AddHangfireServer();

            //FireBase
            Stream stream = new FileStream(@"wwwroot\flutter-asp-notifications-firebase-adminsdk-9eao9-7a8b7e4bf5.json",FileMode.Open);
            var credential = GoogleCredential.FromStream(stream);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential,
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.AccessDeniedPath = "/Home/Error";
                  options.LoginPath = "/Home/Index";
              });


            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            // Triggers and Timing operations stuff ...
            app.UseHangfireDashboard();

            //RecurringJob.AddOrUpdate(() => TimingOperations.EmptySurgeryRooms()
            //, Cron.Minutely);
            //RecurringJob.AddOrUpdate(() => TimingOperations.AlterPreviews()
            //, Cron.Daily);
            //RecurringJob.AddOrUpdate(() => TimingOperations.AlterSentColumn()
            //, Cron.Daily);
            //RecurringJob.AddOrUpdate(() => TimingOperations.DeleteAcceptedRequests()
            //, Cron.Monthly);
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
