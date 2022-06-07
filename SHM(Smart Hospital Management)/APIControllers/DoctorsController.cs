using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SHM_Smart_Hospital_Management_.Break_Tables;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.MedicalDetailsExtraTables;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.Notifications;
using SHM_Smart_Hospital_Management_.PhoneNumbers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.APIControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hosting;

        public DoctorsController(ApplicationDbContext context, IWebHostEnvironment hosting)
        {
            _context = context;
            _hosting = hosting;
        }


        #region LogIn
        [Route("[action]")]
        public async Task<IActionResult> LogIn()
        {
            var data = await _context.Hospitals.Select(x => new
            {
                HospitalId = x.Ho_Id,
                HospitalName = x.Ho_Name
            }).ToListAsync();
            return Ok(data);
        }
        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> LogIn([FromForm] IFormCollection fc)
        {
            //return Ok(new {  id =fc["HoId"] });
            bool IsDeptManager = false;
            var doctors = await _context.Doctors.Where(d => d.Doctor_Email == fc["email"].ToString() && d.Doctor_Password == fc["password"].ToString() && d.Active).ToListAsync();
            if (doctors.Count == 0)
                return Ok(new { Status = false, Message = "البيانات المدخلة غير صحيحة" });
            var data = (from doc in doctors
                        join dept in _context.Departments.ToList()
                        on doc.Department_Id equals dept.Department_Id
                        where dept.Ho_Id == int.Parse(fc["HoId"])
                        select doc).ToList();
            if (data.Count == 0)
                return Ok(new { Status = false, Message = "البيانات المدخلة غير صحيحة" });
            Doctor doctor = data[0];
            //check if the doctor is a deptmanager
            var Department = _context.Departments.FirstOrDefault(m => m.Dept_Manager.Doctor_Id == doctor.Doctor_Id);
            if (Department != null) IsDeptManager = true;
            FCMService.UpdateToken(fc["fcmToken"].ToString(), doctor.Doctor_Id, UserType.doc, Platform.Android);

            return Ok(new
            {
                Status = true,
                Message = "تم تسجيل الدخول بنجاح",
                data = new
                {
                    DoctorId = doctor.Doctor_Id,
                    DoctorName = doctor.Doctor_First_Name + " " + doctor.Doctor_Last_Name,
                    HospitalId = int.Parse(fc["HoId"]),
                    IsManager = IsDeptManager
                }
            });
        }
        #endregion

        [Route("[action]")]
        //عرض المناطق حسب المدينة
        public async Task<IActionResult> DisplayArea([FromQuery] string id)  //cityId
        {
            var area = await (_context.Areas.Where(a => a.City_Id == int.Parse(id))
                            .Select(a => new
                            {
                                id = a.Area_Id,
                                name = a.Area_Name
                            })).ToListAsync();
            return Ok(area);
        }
        [Route("[action]")]
        public async Task<IActionResult> Cities()
        {
            var data = await _context.Cities.Select(c => new
            {
                CityId = c.City_Id,
                CityName = c.City_Name,
            }).ToListAsync();
            return Ok(data);
        }

        [Route("[action]")]
        // (المرضى)
        public async Task<IActionResult> GetWorkDays([FromQuery] int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (!doctor.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });
            }
            var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == id).Select(s => new
            {
                Day = s.Day.ToString(),
                Start = s.Start_Hour.Hours >= 12 ? s.Start_Hour.Hours == 12 ? "12" + ":" + (s.Start_Hour.Minutes == 0 ? "00" : "30") + " PM" : s.Start_Hour.Hours - 12 + ":" + (s.Start_Hour.Minutes == 0 ? "00" : "30") + " PM" : s.Start_Hour.Hours == 0 ? "12" + ":" + (s.Start_Hour.Minutes == 0 ? "00" : "30") + " AM" : s.Start_Hour.Hours + ":" + (s.Start_Hour.Minutes == 0 ? "00" : "30") + " AM",
                End = s.End_Hour.Hours >= 12 ? s.End_Hour.Hours == 12 ? "12" + ":" + (s.End_Hour.Minutes == 0 ? "00" : "30") + " PM" : s.End_Hour.Hours - 12 + ":" + (s.End_Hour.Minutes == 0 ? "00" : "30") + " PM" : s.End_Hour.Hours == 0 ? "12" + ":" + (s.End_Hour.Minutes == 0 ? "00" : "30") + " AM" : s.End_Hour.Hours + ":" + (s.End_Hour.Minutes == 0 ? "00" : "30") + " AM",
                HourCount = s.End_Hour.Hours - s.Start_Hour.Hours
            }).ToListAsync();
            if (workDays.Count == 0)
            {
                return Ok(new { Status = false, Active = true, Message = "ليس لديك جدول دوام" });
            }
            return Ok(new { Status = true, data = new { workDays = workDays } });
        }

        [Route("[action]")]
        // (المرضى)
        public async Task<IActionResult> DisplayPatients([FromQuery] int id) // display doctor's patients by passing the doctor's Id .... 
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(m => m.Doctor_Id == id);
            if (!doctor.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });
            }

            var data = (from p in _context.Patients
                        join pre in _context.Previews
                        on p.Patient_Id equals pre.Patient_Id
                        where (pre.Doctor_Id == doctor.Doctor_Id && pre.Caring == true && p.Active == true)
                        select p
                         ).ToList();
            if (data.Count == 0)
                return Ok
               (new
               {
                   Status = false,
                   Active = true,
                   Message = "ليس لديك مرضى",
               }
               );
            var patients = (from d in data
                            group d by d into c
                            select new
                            {
                                patient_Id = c.Key.Patient_Id,
                                patient_Full_Name = c.Key.Patient_Full_Name,
                                patient_Age = DateTime.Now.Year - c.Key.Patient_Birth_Date.Year,
                                patient_Place = _context.Areas.Find(c.Key.Area_Id).Area_Name,
                                patient_Phone = _context.Patient_Phone_Numbers.Where(pn => pn.Patient_Id == c.Key.Patient_Id).ToList()
                            }).ToList();

            return Ok(new
            {
                Status = true,
                data = new { Patients = patients }
            });

        }
        // (العمليات)
        #region Doctors
        [Route("[action]")]
        public async Task<IActionResult> GetDoctorsForDeptMgr([FromQuery] int id)
        {
            var doc = await _context.Doctors.FindAsync(id);
            if (!doc.Active)
            {
                return Ok
                   (new
                   {
                       Status = false,
                       active = false,
                       Message = "لم يعد لديك حساب في هذه المشفى",
                   }
                   );
            }
            bool checkIfMge = _context.Departments.Include(d => d.Dept_Manager).FirstOrDefault(d => d.Department_Id == doc.Department_Id).Dept_Manager.Doctor_Id == doc.Doctor_Id ? true : false;
            if (!checkIfMge)
            {
                return Ok
                   (new
                   {
                       Status = false,
                       active = false,
                       Message = "لم تعد رئيساً للقسم",
                   }
                   );
            }
            var dept = await _context.Departments.FirstOrDefaultAsync(d => d.Dept_Manager.Doctor_Id == id);
            var doctors = await (from d in _context.Doctors
                                 join l in _context.Areas on d.Area_Id equals l.Area_Id
                                 join b in _context.Cities on d.Doctor_Birth_Place equals b.City_Id
                                 where (d.Department_Id == dept.Department_Id && d.Active && d.Doctor_Id != doc.Doctor_Id)
                                 select new
                                 {
                                     doctor_Full_Name = d.Doctor_Full_Name,
                                     hireDate = d.Doctor_Hire_Date.ToString("dd/MM/yyyy"),
                                     qualifications = d.Doctor_Qualifications,
                                     age = d.Doctor_Age.ToString(),
                                     birthPlace = b.City_Name,
                                     livesIn = l.Area_Name,
                                     gender = d.Doctor_Gender,
                                     socialStatus = d.Doctor_Social_Status,
                                     familyMembers = d.Doctor_Family_Members.ToString(),
                                     phoneNumbers = d.Doctor_Phone_Numbers
                                 }

                ).ToListAsync();
            if (doctors.Count == 0)
                return Ok
                   (new
                   {
                       Status = false,
                       active = true,
                       Message = "لا يوجد أطباء في القسم",
                   }
                   );
            return Ok(new
            {
                Status = true,
                data = new { Doctors = doctors }
            });
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromForm] IFormCollection fc, [FromQuery] DateTime birthDate, [FromQuery] DateTime hireDate, [FromQuery] List<String> phone)
        {                                               // ([FroBody]Doctor doctor)
            var doc = _context.Doctors.Find(int.Parse(fc["id"]));
            if (!doc.Active)
            {
                return Ok
                   (new
                   {
                       Status = false,
                       active = false,
                       Message = "لم يعد لديك حساب في هذه المشفى",
                   }
                   );
            }
            bool checkIfMge = _context.Departments.Include(d => d.Dept_Manager).FirstOrDefault(d => d.Department_Id == doc.Department_Id).Dept_Manager.Doctor_Id == doc.Doctor_Id ? true : false;
            if (!checkIfMge)
            {
                return Ok
                   (new
                   {
                       Status = false,
                       active = false,
                       Message = "لم تعد رئيساً للقسم",
                   }
                   );
            }
            var dept = await _context.Departments.FirstOrDefaultAsync(d => d.Dept_Manager.Doctor_Id == int.Parse(fc["id"]));
            Doctor d = new Doctor()
            {
                Doctor_First_Name = fc["firstName"].ToString(),
                Doctor_Middle_Name = fc["middleName"].ToString(),
                Doctor_Last_Name = fc["lastName"].ToString(),
                Doctor_Email = fc["email"].ToString().Replace(" ", "_"),
                Doctor_National_Number = fc["nationalNumber"].ToString(),
                Doctor_Password = fc["nationalNumber"].ToString(),
                Doctor_Birth_Date = birthDate,
                Doctor_Hire_Date = hireDate,
                Doctor_Gender = fc["gender"].ToString(),
                Doctor_Social_Status = fc["socialStatus"].ToString(),
                Doctor_Family_Members = fc["familyMembers"].ToString() == "" ? null : int.Parse(fc["familyMembers"]),
                Doctor_Birth_Place = fc["birthPlace"].ToString() == "" ? null : int.Parse(fc["birthPlace"]),
                Area_Id = fc["livesIn"].ToString() == "" ? null : int.Parse(fc["livesIn"]),
                Doctor_Qualifications = fc["qualifications"].ToString(),
                Active = true,
                Department_Id = dept.Department_Id,
            };

            var docs = await _context.Doctors.Where(doc => doc.Department_Id == d.Department_Id && doc.Doctor_National_Number == d.Doctor_National_Number).ToListAsync();
            if (docs.Count > 0)
            {
                return Ok(new
                {
                    Status = false,
                    active = true,
                    Message = "لقد تم إضافة هذا الطبيب مسبقاً"
                });
            }

            await _context.AddAsync(d);
            await _context.SaveChangesAsync();

            if (phone.Count != 0)
            {
                List<Doctor_Phone_Numbers> p = new List<Doctor_Phone_Numbers>();
                foreach (var item in phone)
                {
                    p.Add(new Doctor_Phone_Numbers()
                    {
                        Doctor_Id = d.Doctor_Id,
                        Doctor_Phone_Number = item,
                    });
                }


                await _context.AddRangeAsync(p);
                await _context.SaveChangesAsync();
            }



            return Ok(new
            {
                Status = true,
                Message = "تم إضافة الطبيب بنجاح"
            });
        }
        #endregion

        #region Surgery

        [Route("[action]")]
        public async Task<IActionResult> DeleteSurgery([FromQuery] int? id)
        {
            if (id == null)
            {
                return Ok(new
                {
                    Status = false,
                    Message = "العملية غير موجودة",
                });
            }
            var surgery = await _context.Surgeries.FirstAsync(p => p.Surgery_Number == id);
            if (surgery == null)
            {
                return Ok(new
                {
                    Status = false,
                    Message = "العملية غير موجودة",
                });
            }
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

            return Ok(new
            {
                Status = true,
                Message = "تم إلغاء العملية بنجاح!",
            });
        }


        [Route("[action]")]
        // (العمليات)
        public async Task<IActionResult> DisplaySurgeries([FromQuery] int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (!doctor.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });
            }
            var surgeries = await (from s in _context.Surgeries
                                   join p in _context.Patients
                                   on s.Patient_Id equals p.Patient_Id
                                   where s.Doctor_Id == id && s.Surgery_Date.Date >= DateTime.Now.Date && p.Active
                                   select s).ToListAsync();
            if (surgeries.Count == 0)
                return Ok(new { Status = false, Active = true, Message = "ليس لديك عمليات" });
            var sur = (from s in surgeries
                       join sr in _context.Surgery_Rooms
                       on s.Surgery_Room_Id equals sr.Surgery_Room_Id
                       select new
                       {
                           SurgeryId = s.Surgery_Number,
                           SurgeryName = s.Surgery_Name,
                           SurgeryDate = s.Surgery_Date.ToString("dd/MM/yyyy"),
                           SurgeryHour = s.Surgery_Date.ToString("hh:mm:tt"),
                           SurgeryTime = s.Surgery_Time.ToString("c").Substring(0, 5),
                           SurgeryRoom = sr.Su_Room_Number,
                           Floor = sr.Su_Room_Floor,
                           PatientName = _context.Patients.FirstOrDefault(p => p.Active && p.Patient_Id == s.Patient_Id)?.Patient_Full_Name,
                           PatientPhoneNumbers = _context.Patient_Phone_Numbers.Where(pn => pn.Patient_Id == s.Patient_Id).ToList()
                       }).ToList();

            return Ok(new
            {
                Status = true,
                Message = "Success",
                data = new { Surgeries = sur, DoctorId = id }
            });
        }

        [Route("[action]")]
        // (العمليات)
        public async Task<IActionResult> DisplayEmptySurgeryRoom(int HoId)
        {
            var surgeryRoom = await (_context.Surgery_Rooms.Where(su => su.Surgery_Room_Ready == true && su.Ho_Id == HoId && su.Active).Select(s => new
            {
                id = s.Surgery_Room_Id,
                name = s.Su_Room_Number,
                floor = s.Su_Room_Floor
            }
            )).ToListAsync();
            if (surgeryRoom.Count == 0)
            {
                return Ok(new
                {
                    status = false,
                    message = "لا يوجد غرف عمليات فارغة"
                });
            }
            return Ok(new
            {
                status = true,
                data = surgeryRoom
            });
        }

        [Route("[action]")]
        public async Task<IActionResult> DisplayPatientForSurgery(int HoId)
        {
            var patients = await (from p in _context.Patients
                                  join a in _context.Areas
                                  on p.Area_Id equals a.Area_Id
                                  where p.Ho_Id == HoId && p.Active == true
                                  select new
                                  {
                                      patient_Id = p.Patient_Id,
                                      patient_Full_Name = p.Patient_Full_Name,
                                      patient_Age = DateTime.Now.Year - p.Patient_Birth_Date.Year,
                                      patient_Place = a.Area_Name,
                                      patient_Phone = _context.Patient_Phone_Numbers.Where(pn => pn.Patient_Id == p.Patient_Id).ToList()
                                  }).ToListAsync();
            if (patients.Count == 0)
            {
                return Ok(new
                {
                    status = "false",
                    message = "لا يوجد مرضى"
                });
            }
            return Ok(new
            {
                status = true,
                data = patients
            });
        }

        [Route("[action]")]
        public async Task<IActionResult> DisplayAvalibaleTime(DateTime date, int Sr_Id, string hour, string minute)
        {
            var su = await _context.Surgeries.Where(s => s.Surgery_Room_Id == Sr_Id).ToListAsync();
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
                    temp = TimeSpan.FromHours(i.Hours + int.Parse(hour)) + TimeSpan.FromMinutes(i.Minutes + int.Parse(minute));
                    if (temp < TimeSpan.FromDays(1) && i.Hours > DateTime.Now.Hour)
                        d.Add(i.ToString("c"), i.Hours >= 12 ? i.Hours == 12 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours - 12 + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours == 0 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " AM" : i.Hours + ":" + (i.Minutes == 0 ? "00" : "30") + "AM");
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
                        temp = TimeSpan.FromHours(j.Hours + int.Parse(hour)) + TimeSpan.FromMinutes(j.Minutes + int.Parse(minute));
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
            if (d.Count == 0)
            {
                return Ok(new
                {
                    status = false,
                    message = "ليس هناك وقت متاح في هذا اليوم"
                });
            }
            return Ok(new
            {
                status = true,
                key = d.Select(d => d.Key).ToList(),
                value = d.Select(d => d.Value).ToList(),
            });
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateSurgery([FromForm] IFormCollection fc, [FromQuery] DateTime date)
        {
            var doctor = _context.Doctors.Find(int.Parse(fc["id"]));
            if (!doctor.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });
            }
            TimeSpan t = TimeSpan.FromHours(int.Parse(fc["hour"])) + TimeSpan.FromMinutes(int.Parse(fc["minute"]));
            Surgery sr = new Surgery()
            {
                Doctor_Id = int.Parse(fc["id"]),
                Patient_Id = int.Parse(fc["PatId"]),
                Surgery_Date = date,
                Surgery_Name = (fc["name"]).ToString(),
                Surgery_Room_Id = int.Parse(fc["srId"]),
                Surgery_Time = t,
            };
            var surgery = JsonConvert.SerializeObject(sr);
            var dept = await _context.Departments.Include(d => d.Dept_Manager).FirstOrDefaultAsync(d => d.Department_Id == doctor.Department_Id);
            var ho = await _context.Hospitals.FirstOrDefaultAsync(h => h.Ho_Id == dept.Ho_Id);
            var Mgr = ho.Ho_Mgr_Id;
            Request request = new Request
            {
                Accept = false,
                Patient_Id = sr.Patient_Id,
                Employee_Id = null,
                Request_Date = DateTime.Now,
                Request_Type = "Add Surgery",
                Request_Description = $"د. {doctor.Doctor_Full_Name} يريد عمل عملية في {DateTime.Now.ToString("g")} في الغرفة \" {_context.Surgery_Rooms.Find(sr.Surgery_Room_Id).Su_Room_Number}  ",
                Doctor_Id = Mgr,
                Request_Data = surgery,
            };
            _context.Add(request);
            await _context.SaveChangesAsync();
            #region send notification
            //==============================================================================================
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", request.Request_Description},
                        }
            };
            await FCMService.SendNotificationToUserAsync((int)Mgr, UserType.emp, message);
            //=========================================================================================
            #endregion

            return Ok(new
            {
                status = true,
                message = "تم ارسال الطلب، سوف نعلمك حين يتم قبوله"
            });

        }

        #endregion

        #region Preview For Doctor
        [Route("[action]")]
        public async Task<IActionResult> DeletePreview([FromQuery] int? id)
        {
            if (id == null)
            {
                return Ok(new
                {
                    Status = false,
                    Message = "فشلت العملية",
                });
            }
            var preview = await _context.Previews.FirstAsync(p => p.Preview_Id == id);
            if (preview == null)
            {
                return Ok(new
                {
                    Status = false,
                    Message = "فشلت العملية",
                });
            }
            _context.Previews.Remove(preview);
            await _context.SaveChangesAsync();

            #region send notification
            //==============================================================================================
            var doc = await _context.Doctors.FirstOrDefaultAsync(d => d.Doctor_Id == preview.Doctor_Id);
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    { "channelId","other" },
                    { "title",  "ألغي الموعد"},
                    { "body","تم إلغاء الموعد عند الطبيب " + doc.Doctor_Full_Name },
                }

            };
            await FCMService.SendNotificationToUserAsync((int)preview.Patient_Id, UserType.pat, message);
            //=========================================================================================
            #endregion

            return Ok(new
            {
                Status = true,
                Message = "تم إلغاء الموعد بنجاح!",
            });
        }

        [Route("[action]")]
        public async Task<IActionResult> DisplayPreviews([FromQuery] int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (!doctor.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });
            }
            // display all patients in the same hospital with a button to book a preview with the patient id 
            var doctorPreviews = await (from pre in _context.Previews
                                        join pat in _context.Patients
                                        on pre.Patient_Id equals pat.Patient_Id
                                        where pre.Doctor_Id == id && pre.Preview_Date.Date >= DateTime.Now.Date
                                        orderby pre.Preview_Date
                                        select new
                                        {
                                            PaId = pat.Patient_Id,
                                            PreviewId = pre.Preview_Id,
                                            PatientName = pat.Patient_First_Name + " " + pat.Patient_Last_Name,
                                            PreviewDate = pre.Preview_Date.ToString("dd/MM/yyyy"),
                                            PreviewHour = pre.Preview_Date.ToString("hh:mm tt"),
                                            exam = pre.ExaminationRecord,
                                            PatientPhoneNumber = _context.Patient_Phone_Numbers.Where(pn => pn.Patient_Id == pat.Patient_Id).ToList(),
                                            isToday = pre.Preview_Date.Date == DateTime.Now.Date
                                        }).ToListAsync();
            if (doctorPreviews.Count == 0)
                return Ok
                    (
                    new
                    {
                        Status = false,
                        Active = true,
                        Message = "ليس لديك معاينات"
                    }
                    );
            return Ok(new
            {
                Status = true,
                data = new { Previews = doctorPreviews, DoctorId = id }
            });
        }

        [Route("[action]")]
        public async Task<IActionResult> ValidateDate([FromQuery] DateTime date, [FromQuery] int id, [FromQuery] int PatId)//id(doctorId)
        {
            var doctor = _context.Doctors.Find(id);
            if (!doctor.Active)
            {
                return Ok(new { IsValid = false, Active = false, Message = "لقد تم حظرك من المشفى" });
            }
            var workDays = await _context.Work_Days.Where(w => w.Doctor_Id == id).ToListAsync();
            foreach (var item in workDays)
                if (date.DayOfWeek.CompareTo((DayOfWeek)((int)item.Day)) == 0)
                {
                    var DayHours = workDays.Where(w => date.DayOfWeek.CompareTo((DayOfWeek)((int)w.Day)) == 0).ToList()[0];
                    TimeSpan HalfHour = TimeSpan.FromMinutes(30);
                    List<TimeSpan> prev = await _context.Previews.Where(p => p.Doctor_Id == id && p.Preview_Date.Date == date.Date).Select(s => s.Preview_Date.TimeOfDay).ToListAsync();
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    for (TimeSpan i = DayHours.Start_Hour; i < DayHours.End_Hour; i += HalfHour)
                    {
                        if (date.DayOfWeek.CompareTo(DateTime.Now.DayOfWeek) == 0)
                        {
                            if (i.Hours <= DateTime.Now.Hour)
                            {
                                prev.Add(i);
                            }
                        }
                        if (!prev.Contains(i))
                            d.Add(i.ToString("c"), i.Hours >= 12 ? i.Hours == 12 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours - 12 + ":" + (i.Minutes == 0 ? "00" : "30") + " PM" : i.Hours == 0 ? "12" + ":" + (i.Minutes == 0 ? "00" : "30") + " AM" : i.Hours + ":" + (i.Minutes == 0 ? "00" : "30") + "AM");
                    }
                    if (d.Count == 0)
                    {
                        return Ok(new
                        {
                            IsValid = true,
                            empty = true,
                            message = "ليس لديك ساعات فارغة في هذا اليوم",
                        }
                            );
                    }
                    return Ok(new
                    {
                        IsValid = true,
                        empty = false,
                        PatientId = PatId,
                        Date = date.Date.ToString("dd-MM-yyyy"),
                        key = d.Select(d => d.Key).ToList(),
                        value = d.Select(d => d.Value).ToList(),

                    }); ;
                }

            return Ok(new { IsValid = false, Active = true, Message = "يرجى اختيار يوم من ايام دوامك." });
        }

        [Route("[action]")]
        // (حجز موعد) ==> (حجز موعد للمريض)
        public async Task<IActionResult> CheckTime([FromQuery] int PaId, [FromQuery] DateTime date)
        {
            var pre = await _context.Previews.Where(p => p.Patient_Id == PaId && p.Preview_Date == date).ToListAsync();
            if (pre.Count != 0)
            {
                return Ok(new
                {
                    Status = false,
                    Message = "لدى المريض موعد آخر في نفس التوقيت"
                }
                );
            }
            return Ok(new
            {
                Status = true,
            });
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreatePreview([FromForm] IFormCollection fc, [FromQuery] DateTime date)
        {
            var doctor = _context.Doctors.Find(int.Parse(fc["id"]));
            if (!doctor.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لقد تم حظرك من المشفى" });
            }
            Preview p = new Preview()
            {
                Caring = true,
                Doctor_Id = int.Parse(fc["id"]),
                Patient_Id = int.Parse(fc["PatId"]),
                Preview_Date = date /// can be changed
            };
            _context.Add(p);
            await _context.SaveChangesAsync();
            #region send notification
            //==============================================================================================
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title","موعد جديد"},
                            { "body","لديك موعد جديد عند الطبيب " + doctor.Doctor_Full_Name },
                        }
            };
            await FCMService.SendNotificationToUserAsync((int)p.Patient_Id, UserType.pat, message);
            //=========================================================================================
            #endregion

            return Ok(new
            {
                Status = true,
                Message = "تم حجز الموعد بنجاح"
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddExamination([FromForm] IFormCollection fc) //id(preview)
        {
            bool update = false;
            var preview = _context.Previews.Find(int.Parse(fc["id"]));
            if (preview.ExaminationRecord != null)
            {
                update = true;
            }
            preview.ExaminationRecord = fc["exam"].ToString();
            _context.Update(preview);
            await _context.SaveChangesAsync();
            if (update)
            {
                return Ok(new { status = true, message = "تم تعديل التشخيص بنجاح" });
            }
            return Ok(new { status = true, message = "تم إضافة التشخيص بنجاح" });
        }
        #endregion // I am heeeeeeeeeeereeeeeeeeeeeeee

        #region MedicalDetails

        [Route("[action]")]
        // (عرض التفاصيل الطبية)

        public async Task<IActionResult> GetMedicalDetails([FromQuery] int id, [FromQuery] int PaId)//docId
        {
            var doctor = _context.Doctors.Find(id);
            if (!doctor.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });
            }
            var medical = await _context.Medical_Details.Include(m => m.Patient).FirstOrDefaultAsync(m => m.Patient.Patient_Id == PaId);

            if (medical == null)
            {
                return Ok(new { status = false, active = true, Message = "لا يوجد ملف طبي", paId = PaId });
            }

            var allergies = await (from a in _context.Medical_Allergies
                                   join all in _context.Allergies
                                   on a.Allergy_Id equals all.Allergy_Id
                                   where medical.Medical_Details_Id == a.Medical_Detail_Id
                                   select all.Allergy_Name
                             ).ToListAsync();

            var family = await (from md in _context.Medical_Diseases
                                join d in _context.Diseases
                                on md.Disease_Id equals d.Disease_Id
                                where medical.Medical_Details_Id == md.Medical_Detail_Id && md.Family_Health_History == true
                                select d.Disease_Name
                             ).ToListAsync();

            var chronic = await (from md in _context.Medical_Diseases
                                 join d in _context.Diseases
                                 on md.Disease_Id equals d.Disease_Id
                                 where medical.Medical_Details_Id == md.Medical_Detail_Id && md.Chronic_Diseases == true
                                 select d.Disease_Name
                             ).ToListAsync();

            var examination = await (from d in _context.Doctors
                                     join p in _context.Previews
                                     on d.Doctor_Id equals p.Doctor_Id
                                     where p.Patient_Id == PaId && p.ExaminationRecord != null
                                     select new
                                     {
                                         doctorName = "د." + d.Doctor_First_Name + " " + d.Doctor_Last_Name,
                                         date = p.Preview_Date.ToString("dd-MM-yyyy"),
                                         examination = p.ExaminationRecord
                                     }
                                    ).ToListAsync();

            string PaName = medical.Patient.Patient_Full_Name;
            return Ok(new
            {
                status = true,
                data = new
                {
                    medicalId = medical.Medical_Details_Id,
                    name = PaName,
                    blood = medical.MD_Patient_Blood_Type,
                    need = medical.MD_Patient_Special_Needs,
                    plans = medical.MD_Patient_Treatment_Plans_And_Daily_Supplements,
                },
                allegies = allergies,
                family = family,
                chronic = chronic,
                exam = examination
            });
        }

        [Route("[action]")]
        public IActionResult DisplayAllAllergies()
        {
            var allergies = _context.Allergies.Select(a => new { id = a.Allergy_Id, name = a.Allergy_Name }).ToList();
            return Ok(allergies);
        }

        #region MedicalTest


        [Route("[action]")]
        public async Task<IActionResult> TestType()
        {
            var data = await _context.Test_Types.Select(x => new
            {
                TestlId = x.Test_Type_Id,
                TestName = x.Test_Type_Name
            }).ToListAsync();
            return Ok(data);
        }

        [Route("[action]")]
        public async Task<IActionResult> DisplayTestByTestType(string id)// testTypeId
        {
            var test = await (from t in _context.Tests
                              where t.Test_Type_Id == int.Parse(id)
                              select new
                              {
                                  id = t.Test_Id,
                                  name = t.Test_Name,
                              }).ToListAsync();
            return Ok(test);
        }

        [HttpPost("upload-mutiple")]
        [Route("[action]")]
        [Produces("application/json")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadMultipleTests(
            [FromForm(Name = "files")] List<IFormFile> files, [FromQuery] List<DateTime> dates, [FromQuery] List<string> type, [FromQuery] int medicalId
        )
        {
            if (files.Count == 0) return Ok(new
            {
                status = false,
                message = " الرجاء اختيار تحليل لاضافته "
            });
            List<Medical_Test> MedicalTests = new List<Medical_Test>();
            int i = 0;
            List<string> data = new List<string>();
            string fileName = string.Empty;

            string test = Path.Combine(_hosting.WebRootPath, "Medical_Test_Result");

            foreach (var file in files)
            {
                fileName = file.FileName;
                string fullPath = Path.Combine(test, fileName);
                FileStream f = new FileStream(fullPath, FileMode.OpenOrCreate);
                file.CopyTo(f);
                data.Add($"Filename: {file.FileName} || Type: {file.ContentType}");
                MedicalTests.Add(new Medical_Test()
                {
                    Test_Result = fileName,
                    Test_Date = dates[i],
                    Test_Id = int.Parse(type[i]),
                    Medical_Detail_Id = medicalId
                });
                i++;
                f.Close();
            }
            await _context.AddRangeAsync(MedicalTests);
            await _context.SaveChangesAsync();
            #region send notification
            //==============================================================================================
            var p = _context.Medical_Details.Include(p => p.Patient).FirstOrDefault(p => p.Medical_Details_Id == medicalId);
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تحليل جديد"},
                            { "body","تم إضافة  " + MedicalTests.Count + " تحاليل جديدة."},
                        }

            };
            await FCMService.SendNotificationToUserAsync(p.Patient.Patient_Id, UserType.pat, message);
            //=========================================================================================
            #endregion

            return Ok(new
            {
                status = true,
                message = "تم اضافة التحليل بنجاح",
            });
        }

        [Route("[action]")]
        public async Task<IActionResult> DisplayTest([FromQuery] int medicalId)
        {
            var test = await (from mt in _context.Medical_Tests
                              join t in _context.Tests
                              on mt.Test_Id equals t.Test_Id
                              where mt.Medical_Detail_Id == medicalId
                              select new
                              {
                                  testId = t.Test_Id,
                                  testDate = mt.Test_Date.ToString("dd-MM-yyyy"),
                                  testResult = mt.Test_Result,
                                  testName = t.Test_Name
                              }).ToListAsync();
            if (test.Count == 0)
            {
                return Ok(new { Status = false, Active = true, Message = "لا يوجد تحاليل" });

            }
            return Ok(new { status = true, tests = test });
        }


        #endregion

        #region Rays
        [Route("[action]")]
        public async Task<IActionResult> RayType()
        {
            var data = await _context.Ray_Types.Select(x => new
            {
                RayId = x.Ray_Type_Id,
                RayName = x.Ray_Type_Name
            }).ToListAsync();
            return Ok(data);
        }


        [HttpPost("upload-mutiple")]
        [Route("[action]")]
        [Produces("application/json")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadMultipleRays([FromForm(Name = "files")] List<IFormFile> files, [FromQuery] List<DateTime> dates, [FromQuery] List<string> type, [FromQuery] int medicalId)
        {
            if (files.Count == 0) return Ok(new
            {
                status = false,
                message = " الرجاء اختيار صورة الأشعة لاضافتها "
            });
            List<Medical_Ray> Rays = new List<Medical_Ray>();
            int i = 0;
            List<string> data = new List<string>();
            string fileName = string.Empty;

            string test = Path.Combine(_hosting.WebRootPath, "Ray_Result");

            foreach (var file in files)
            {
                fileName = file.FileName;
                string fullPath = Path.Combine(test, fileName);
                FileStream f = new FileStream(fullPath, FileMode.OpenOrCreate);

                file.CopyTo(f);
                data.Add($"Filename: {file.FileName} || Type: {file.ContentType}");
                Rays.Add(new Medical_Ray()
                {
                    Ray_Result = fileName,
                    Ray_Date = dates[i],
                    Ray_Type_Id = int.Parse(type[i]),
                    Medical_Detail_Id = medicalId
                });
                i++;
                f.Close();
            }
            await _context.AddRangeAsync(Rays);
            await _context.SaveChangesAsync();

            #region send notification
            //==============================================================================================
            var pat = await _context.Medical_Details.Include(p => p.Patient).FirstOrDefaultAsync(p => p.Medical_Details_Id == medicalId);
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                    {
                        { "channelId","other" },
                        { "title", "صورة أشعة جديدة"},
                        { "body","تم إضافة  "+ Rays.Count+" صورة جديدة" },
                    }
            };
            await FCMService.SendNotificationToUserAsync(pat.Patient.Patient_Id, UserType.pat, message);
            //=========================================================================================
            #endregion

            return Ok(new
            {
                status = true,
                message = "تم اضافة الأشعة بنجاح",
            });
        }

        [Route("[action]")]
        public async Task<IActionResult> DisplayRays([FromQuery] int medicalId)
        {
            var ray = await (from r in _context.Medical_Rays
                             join ry in _context.Ray_Types
                             on r.Ray_Type_Id equals ry.Ray_Type_Id
                             where r.Medical_Detail_Id == medicalId
                             select new
                             {
                                 rayId = r.Ray_Id,
                                 rayDate = r.Ray_Date.ToString("dd-MM-yyyy"),
                                 rayResult = r.Ray_Result,
                                 rayType = ry.Ray_Type_Name,
                             }).ToListAsync();
            if (ray.Count == 0)
            {
                return Ok(new { Status = false, Active = true, Message = "لا يوجد أشعة" });

            }
            return Ok(new { status = true, rays = ray });
        }

        #endregion

        #region External records

        [Route("[action]")]
        public async Task<IActionResult> DisplayExternalRecords([FromQuery] int medicalId)
        {
            var external = await _context.External_Records.Where(e => e.Medical_Detail_Id == medicalId).Select(e => new
            {
                externalId = e.External_Records_Id,
                externalDetails = e.Details,
            }).ToListAsync();
            if (external.Count == 0)
            {
                return Ok(new { Status = false, Active = true, Message = "لا يوجد تقارير خارجية" });

            }
            return Ok(new { status = true, externals = external });
        }

        #endregion

        #endregion

        #region Personal Info
        [Route("[action]")]
        // (تعديل البيانات الشخصية)
        //get
        public async Task<IActionResult> EditPersonalInfoForDoc([FromQuery] int id)  //docId
        {
            var doc = _context.Doctors.Find(id);
            if (!doc.Active)
            {
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });
            }
            var phone = await _context.Doctor_Phone_Numbers.Where(p => p.Doctor_Id == id).ToListAsync();
            var doctor = await (from d in _context.Doctors
                                join a in _context.Areas
                                on d.Area_Id equals a.Area_Id
                                where d.Doctor_Id == id
                                select new
                                {
                                    name = d.Doctor_First_Name + " " + d.Doctor_Last_Name,
                                    social = d.Doctor_Social_Status,
                                    family = d.Doctor_Family_Members,
                                    qual = d.Doctor_Qualifications,
                                    areaName = a.Area_Name,
                                    areaId = a.Area_Id,
                                    cityId = a.City_Id,
                                    phone = phone,
                                    areas = _context.Areas.Where(ar => ar.City_Id == a.City_Id).Select(ar => new { id = ar.Area_Id, name = ar.Area_Name }).ToList()
                                }).ToListAsync();

            var cities = await _context.Cities.Select(c => new { id = c.City_Id, name = c.City_Name }).ToListAsync();

            return Ok(new
            {
                status = true,
                data = doctor,
                cities = cities

            });
        }

        [Route("[action]")]
        [HttpPost]
        // (تعديل البيانات الشخصية)
        //post
        public async Task<IActionResult> EditPersonalInfoForDocPost([FromForm] IFormCollection fc, [FromQuery] List<string> phones)
        {
            var doctor = await _context.Doctors.FindAsync(int.Parse(fc["id"]));
            doctor.Area_Id = Convert.ToInt32(fc["area"]);
            doctor.Doctor_Family_Members = Convert.ToInt32(fc["family"]);
            doctor.Doctor_Social_Status = fc["social"].ToString();
            doctor.Doctor_Qualifications = fc["qual"].ToString();
            var phoneNumbers = await _context.Doctor_Phone_Numbers.Where(p => p.Doctor_Id == int.Parse(fc["id"])).ToListAsync();
            List<Doctor_Phone_Numbers> p = new List<Doctor_Phone_Numbers>();

            _context.Doctor_Phone_Numbers.RemoveRange(phoneNumbers);
            await _context.SaveChangesAsync();
            for (int i = 0; i < phones.Count; i++)
            {
                Doctor_Phone_Numbers temp = new Doctor_Phone_Numbers()
                {
                    Doctor_Id = int.Parse(fc["id"]),
                    Doctor_Phone_Number = phones[i]
                };
            }
            _context.AddRange(p);
            _context.Update(doctor);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "تم التعديل بنجاح"
                // phones = phones
            });
        }
        #endregion

        [Route("[action]")]
        public async Task<IActionResult> DisplayDiseasesTypes()
        {
            var diseasesType = await _context.Diseases_Types.Select(d => new
            {
                id = d.Disease_Type_Id,
                name = d.Disease_Type_Name
            }).ToListAsync();
            return Ok(diseasesType);
        }

        [Route("[action]")]
        //عرض الامراض حسب نوع المرض
        public async Task<IActionResult> DisplayDiseasesByType([FromQuery] string id)  //diseaseTypeId
        {
            var disease = await (_context.Diseases.Where(d => d.Disease_Type_Id == int.Parse(id))
                            .Select(a => new
                            {
                                id = a.Disease_Id,
                                name = a.Disease_Name
                            })).ToListAsync();
            return Ok(disease);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateMedicalDetails([FromForm] IFormCollection fc, [FromQuery] List<string> allergies, [FromQuery] List<string> family, [FromQuery] List<string> chronic)
        {
            var doc = _context.Doctors.Find(int.Parse(fc["docId"]));
            if (!doc.Active)
            {
                return Ok
                   (new
                   {
                       Status = false,
                       active = false,
                       Message = "لم يعد لديك حساب في هذه المشفى",
                   }
                   );
            }
            Medical_Detail md = new Medical_Detail()
            {
                MD_Patient_Blood_Type = fc["blood"].ToString(),
                MD_Patient_Treatment_Plans_And_Daily_Supplements = fc["plan"].ToString(),
                MD_Patient_Special_Needs = fc["need"].ToString()
            };
            md.Patient = _context.Patients.Find(int.Parse(fc["paId"]));
            _context.Add(md);
            await _context.SaveChangesAsync();

            if (allergies.Count != 0)
            {
                Medical_Allergy[] ma = new Medical_Allergy[allergies.Count];
                for (int i = 0; i < ma.Length; i++)
                {
                    ma[i] = new Medical_Allergy()
                    {
                        Medical_Detail_Id = md.Medical_Details_Id,
                        Allergy_Id = int.Parse(allergies[i])
                    };
                }
                await _context.AddRangeAsync(ma);
                await _context.SaveChangesAsync();
            }

            List<Medical_Disease> mds = new List<Medical_Disease>();
            for (int i = 0; i < family.Count; i++)
            {
                Medical_Disease temp = new Medical_Disease()
                {
                    Medical_Detail_Id = md.Medical_Details_Id,
                    Disease_Id = int.Parse(family[i]),
                    Family_Health_History = true,
                };
                if (chronic.Contains(family[i]))
                    temp.Chronic_Diseases = true;
                else
                    temp.Chronic_Diseases = false;
                mds.Add(temp);
            }

            for (int i = 0; i < chronic.Count; i++)
            {
                if (!family.Contains(chronic[i]))
                {
                    mds.Add(new Medical_Disease()
                    {
                        Medical_Detail_Id = md.Medical_Details_Id,
                        Disease_Id = int.Parse(chronic[i]),
                        Chronic_Diseases = true,
                        Family_Health_History = false
                    });
                }
            }

            await _context.AddRangeAsync(mds);
            await _context.SaveChangesAsync();
            #region send notification
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                    {
                        { "channelId","other" },
                        { "title","الملف الطبي" },
                        { "body","تم إضافة الملف الطبي الخاص بك " },
                    }
            };

            await FCMService.SendNotificationToUserAsync(int.Parse(fc["paId"]), UserType.pat, message);

            #endregion

            return Ok(new { status = true, message = "تم إضافة الملف الطبي بنجاح" });

        }


        [Route("[action]")]
        public async Task<IActionResult> GetMedicalDetailsForUpdate([FromQuery] int id, [FromQuery] int medicalId)//docId
        {
            var medical = _context.Medical_Details.Include(e => e.Patient).FirstOrDefault(m => m.Medical_Details_Id == medicalId);

            var allergies = await (from a in _context.Medical_Allergies
                                   join all in _context.Allergies
                                   on a.Allergy_Id equals all.Allergy_Id
                                   where medical.Medical_Details_Id == a.Medical_Detail_Id
                                   select all.Allergy_Id
                             ).ToListAsync();

            var family = await (from md in _context.Medical_Diseases
                                join d in _context.Diseases
                                on md.Disease_Id equals d.Disease_Id
                                where medical.Medical_Details_Id == md.Medical_Detail_Id && md.Family_Health_History == true
                                select new
                                {
                                    id = d.Disease_Id,
                                    type = d.Disease_Type_Id
                                }
                             ).ToListAsync();

            var chronic = await (from md in _context.Medical_Diseases
                                 join d in _context.Diseases
                                 on md.Disease_Id equals d.Disease_Id
                                 where medical.Medical_Details_Id == md.Medical_Detail_Id && md.Chronic_Diseases == true
                                 select new
                                 {
                                     id = d.Disease_Id,
                                     type = d.Disease_Type_Id
                                 }
                             ).ToListAsync();

            string PaName = medical.Patient.Patient_Full_Name;
            return Ok(new
            {
                status = true,
                data = new
                {
                    medicalId = medical.Medical_Details_Id,
                    name = PaName,
                    blood = medical.MD_Patient_Blood_Type,
                    need = medical.MD_Patient_Special_Needs,
                    plans = medical.MD_Patient_Treatment_Plans_And_Daily_Supplements,
                    paId = medical.Patient.Patient_Id
                },
                allegies = allergies,
                family = family,
                chronic = chronic,
            });
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateMedicalDetails([FromForm] IFormCollection fc, [FromQuery] List<string> allergies, [FromQuery] List<string> family, [FromQuery] List<string> chronic)
        {
            var doc = _context.Doctors.Find(int.Parse(fc["docId"]));
            if (!doc.Active)
            {
                return Ok
                   (new
                   {
                       Status = false,
                       active = false,
                       Message = "لم يعد لديك حساب في هذه المشفى",
                   }
                   );
            }
            var medical = _context.Medical_Details.Find(int.Parse(fc["medicalId"]));
            medical.MD_Patient_Blood_Type = fc["blood"].ToString();
            medical.MD_Patient_Treatment_Plans_And_Daily_Supplements = fc["plan"].ToString();
            medical.MD_Patient_Special_Needs = fc["need"].ToString();
            _context.Update(medical);
            await _context.SaveChangesAsync();
            var aller = _context.Medical_Allergies.Where(m => m.Medical_Detail_Id == medical.Medical_Details_Id).ToList();
            _context.Medical_Allergies.RemoveRange(aller);
            await _context.SaveChangesAsync();
            if (allergies.Count != 0)
            {
                Medical_Allergy[] ma = new Medical_Allergy[allergies.Count];
                for (int i = 0; i < ma.Length; i++)
                {
                    ma[i] = new Medical_Allergy()
                    {
                        Medical_Detail_Id = medical.Medical_Details_Id,
                        Allergy_Id = int.Parse(allergies[i])
                    };
                }
                await _context.AddRangeAsync(ma);
                await _context.SaveChangesAsync();
            }

            var MedicalDisease = _context.Medical_Diseases.Where(m => m.Medical_Detail_Id == medical.Medical_Details_Id).ToList();
            _context.Medical_Diseases.RemoveRange(MedicalDisease);
            await _context.SaveChangesAsync();
            List<Medical_Disease> mds = new List<Medical_Disease>();
            for (int i = 0; i < family.Count; i++)
            {
                Medical_Disease temp = new Medical_Disease()
                {
                    Medical_Detail_Id = medical.Medical_Details_Id,
                    Disease_Id = int.Parse(family[i]),
                    Family_Health_History = true,
                };
                if (chronic.Contains(family[i]))
                    temp.Chronic_Diseases = true;
                else
                    temp.Chronic_Diseases = false;
                mds.Add(temp);
            }

            for (int i = 0; i < chronic.Count; i++)
            {
                if (!family.Contains(chronic[i]))
                {
                    mds.Add(new Medical_Disease()
                    {
                        Medical_Detail_Id = medical.Medical_Details_Id,
                        Disease_Id = int.Parse(chronic[i]),
                        Chronic_Diseases = true,
                        Family_Health_History = false
                    });
                }
            }

            await _context.AddRangeAsync(mds);
            await _context.SaveChangesAsync();
            #region send notification
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تعديل على الملف الطبي"},
                            { "body","تم تعديل على الملف الطبي " },
                        }
            };
            var pat = await _context.Medical_Details.Include(p => p.Patient).FirstOrDefaultAsync(p => p.Medical_Details_Id == medical.Medical_Details_Id);
            await FCMService.SendNotificationToUserAsync(pat.Patient.Patient_Id, UserType.pat, message);

            #endregion

            return Ok(new { status = true, message = "تم تعديل الملف الطبي بنجاح" });

        }
        [Route("[action]")]
        public async Task<IActionResult> ShowRequestsForDeptManager([FromQuery] int id)
        {
            var Deptmanager = _context.Doctors.Find(id);
            var dept = _context.Departments.Where(d => d.Department_Id == Deptmanager.Department_Id).Include(d => d.Dept_Manager).ToArray()[0];
            if (!Deptmanager.Active && Deptmanager.Doctor_Id == dept.Dept_Manager.Doctor_Id)
                return Ok(new { status = false, Active = false, Message = "لم يعد لديك حساب في هذه المشفى" });


            var requests = await _context.Requests.Where(r => r.Doctor_Id == id && r.Accept == false)
                    .Select(r => new
                    {
                        description = r.Request_Description,
                        date = r.Request_Date.ToString("dd-MM-yyyy"),
                        type = r.Request_Type,
                        id = r.Request_Id
                    }).ToListAsync();
            if (requests.Count == 0)
            {
                return Ok(
                new
                {
                    status = false,
                    Active = true,
                    Message = "لا يوجد لديك طلبات",
                });
            }

            return Ok(
                new
                {
                    status = true,
                    Active = true,
                    data = requests
                });

        }

    }
}