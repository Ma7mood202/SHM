using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SHM_Smart_Hospital_Management_.Data;
using SHM_Smart_Hospital_Management_.Models;
using SHM_Smart_Hospital_Management_.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Controllers
{
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Send Nurse
        [Authorize(Roles = "HeadNurse")]
        public async Task<IActionResult> ShowRequestsForHeadNurse(int id)
        {
            var HeadNurse = await _context.Employees.FindAsync(id);
            if (!HeadNurse.Active)
                return RedirectToAction("LogOut", "Employee");
            return View(await _context.Requests.Where(r => r.Employee_Id == id && r.Accept == false).ToListAsync());
        }
        [Authorize(Roles = "HeadNurse")]
        public async Task<IActionResult> AcceptPatientRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            var HeadNurse = await _context.Employees.FindAsync(request.Employee_Id);
            if (!HeadNurse.Active)
            {
                #region send notification
                //==============================================================================================
                var message2 = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "عذراً لم تتم الموافقة على طلبك"},
                            { "body","لم نتمكن من الموافقة على طلبك يرجى إعادة المحاولة" },
                        }
                };
                await FCMService.SendNotificationToUserAsync((int)request.Patient_Id, UserType.pat, message2);
                //=========================================================================================
                #endregion
                return RedirectToAction("LogOut", "Employee");
            }
            request.Accept = true;
            await _context.SaveChangesAsync();
            #region send notification
            //==============================================================================================
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    { "channelId","other" },
                    { "title", "تمت الموافقة على طلبك"},
                    { "body","سنفوم بإرسال ممرض في أقرب وقت" },
                }
            };
            await FCMService.SendNotificationToUserAsync((int)request.Patient_Id, UserType.pat, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("ShowRequestsForHeadNurse", new { id = HeadNurse.Employee_Id });
        }
        [Authorize(Roles = "HeadNurse")]
        public async Task<IActionResult> DenyPatientRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            var HeadNurse = await _context.Employees.FindAsync(request.Employee_Id);
            if (!HeadNurse.Active)
            {
                #region send notification
                //==============================================================================================
                var message2 = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                {
                    { "channelId","other" },
                    { "title", "عذراً لم تتم الموافقة على طلبك"},
                    { "body"," " },
                }

                };
                await FCMService.SendNotificationToUserAsync((int)request.Patient_Id, UserType.pat, message2);
                //=========================================================================================
                #endregion
                return RedirectToAction("LogOut", "Employee");
            }
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            #region send notification
            //==============================================================================================
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    { "channelId","other" },
                    { "title", "عذراً لم تتم الموافقة على طلبك"},
                    { "body"," " },
                }

            };
            await FCMService.SendNotificationToUserAsync((int)request.Patient_Id, UserType.pat, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("ShowRequestsForHeadNurse", new { id = HeadNurse.Employee_Id });

        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> SendNurse(int id) // Paitent(id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (!patient.Active)
                return RedirectToAction("LogOut", "Patient");
            if (patient.Sent)
            {
                #region send notification
                //==============================================================================================
                var message2 = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title","لا يمكن إرسال اكثر من طلب في نفس اليوم" },
                            { "body"," " },
                        }
                };
                await FCMService.SendNotificationToUserAsync((int)patient.Patient_Id, UserType.pat, message2);
                //=========================================================================================
                #endregion
                return RedirectToAction("Master", "Patient", new { id });
            }
            patient.Sent = true;
            var HeadNurse = await _context.Employees.FirstOrDefaultAsync(e => e.Ho_Id == patient.Ho_Id && e.Active && e.Employee_Job == "HeadNurse");
            Request request = new Request
            {
                Accept = false,
                Patient_Id = id,
                Employee_Id = HeadNurse.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "SendNurse",
                Request_Description = $" يرجى إرسال ممرض للمريض {patient.Patient_Full_Name} ",
                Doctor_Id = null,
                Request_Data = patient.Patient_X_Y + "," + patient.Patient_Full_Name
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
                            { "title",request.Request_Description},
                            { "body"," " },
                        }
            };
            await FCMService.SendNotificationToUserAsync((int)request.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Patient", new { id });
        }
        #endregion

        [Authorize(Roles = "Doctor,DeptManager")]
        public async Task<IActionResult> AddSurgery(int id, int HoId, string surgery)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (!doctor.Active)
                return RedirectToAction("LogOut", "Doctor");
            var Surgery = JsonConvert.DeserializeObject<Surgery>(surgery);
            var hospital = await _context.Hospitals.Include(h=>h.Manager).FirstOrDefaultAsync(h=>h.Ho_Id == HoId);
            var Mgr = hospital.Manager;

            Request request = new Request
            {
                Accept = false,
                Patient_Id = Surgery.Patient_Id,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "AddSurgery",
                Request_Description = $"د. {doctor.Doctor_Full_Name} يريد عمل عملية في {DateTime.Now.ToString("g")} في الغرفة \" {_context.Surgery_Rooms.Find(Surgery.Surgery_Room_Id).Su_Room_Number}  ",
                Doctor_Id = null,
                Request_Data = surgery
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Doctor", new { id, HoId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> AddDepartment(int EmpId, string department)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var dept = JsonConvert.DeserializeObject<Department>(department);
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "AddDepartment",
                Request_Description = $"موظف ال \"IT\" : {employee.Employee_Full_Name} يريد إضافة قسم {_context.Specializations.Find(dept.Department_Name).Specialization_Name}",
                Doctor_Id = null,
                Request_Data = department
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeleteDepartment(int id, int EmpId)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var dept = _context.Departments.Find(id);
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "DeleteDepartment",
                Request_Description = $"موظف ال \"IT\" : {employee.Employee_Full_Name} يريد إزالة قسم {_context.Specializations.Find(dept.Department_Name).Specialization_Name}",
                Doctor_Id = null,
                Request_Data = id.ToString()
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }

        [Authorize(Roles = "IT")]
        public async Task<IActionResult> AddRoom(int EmpId, string room)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var Room = JsonConvert.DeserializeObject<Room>(room);
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "AddRoom",
                Request_Description = $"موظف ال \"IT\" : يريد إنشاء الغرفة رقم {Room.Room_Number} ",
                Doctor_Id = null,
                Request_Data = room
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeleteRoom(int id, int EmpId)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var Room = _context.Rooms.Find(id);
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "DeleteRoom",
                Request_Description = $"موظف ال \"IT\" : يريد إزالة الغرفة رقم {Room.Room_Number} ", // كتبولي رسالة ما طلع معي شي 
                Doctor_Id = null,
                Request_Data = id.ToString()
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> AddSurgeryRoom(int EmpId, string surgeryRoom)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var SurgeryRoom = JsonConvert.DeserializeObject<Surgery_Room>(surgeryRoom);
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "AddSurgeryRoom",
                Request_Description = $"موظف ال \"IT\" : يريد إنشاء غرفة ", // كتبولي رسالة ما طلع معي شي 
                Doctor_Id = null,
                Request_Data = surgeryRoom
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DeleteSurgeryRoom(int id, int EmpId)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var SurgeryRoom = _context.Surgery_Rooms.Find(id);
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "DeleteSurgeryRoom",
                Request_Description = $"موظف ال \"IT\" : يريد إزالة الغرفة رقم {SurgeryRoom.Su_Room_Number} ", // كتبولي رسالة ما طلع معي شي 
                Doctor_Id = null,
                Request_Data = id.ToString()
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> AddDeptManager(int EmpId, string doctor)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var deptManager = JsonConvert.DeserializeObject<Doctor>(doctor);
            var dept = await _context.Departments.FindAsync(deptManager.Department_Id);
            var deptName = _context.Specializations.Find(dept.Department_Name).Specialization_Name;
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "AddDeptManager",
                Request_Description = $"موظف ال \"IT\" {employee.Employee_Full_Name} : يريد إضافة الدكتور {deptManager.Doctor_Full_Name} للقسم {deptName} وجعله رئيسه",
                Doctor_Id = null,
                Request_Data = doctor
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> UpdateDeptManager(int id, int EmpId)
        {
            var employee = await _context.Employees.FindAsync(EmpId);
            var Hospital = _context.Hospitals.Where(h => h.Ho_Id == employee.Ho_Id).Include(h => h.Manager).ToArray()[0];
            var Mgr = Hospital.Manager;
            var deptManager = _context.Doctors.Find(id);
            var dept = await _context.Departments.FindAsync(deptManager.Department_Id);
            var deptName = _context.Specializations.Find(dept.Department_Name).Specialization_Name;
            Request request = new Request
            {
                Accept = false,
                Patient_Id = null,
                Employee_Id = Mgr.Employee_Id,
                Request_Date = DateTime.Now,
                Request_Type = "UpdateDeptManager",
                Request_Description = $"موظف ال \"IT\" {employee.Employee_Full_Name} يريد تعيين د.{deptManager.Doctor_Full_Name} للقسم {deptName} وجعله رئيسه",
                Doctor_Id = null,
                Request_Data = id.ToString()
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
            await FCMService.SendNotificationToUserAsync((int)Mgr.Employee_Id, UserType.emp, message);
            //=========================================================================================
            #endregion
            return RedirectToAction("Master", "Employee", new { id = EmpId });
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> AcceptRequest(int id, int mgrId)
        {
            var request = await _context.Requests.FindAsync(id);
            var hoId = _context.Employees.Find(mgrId).Ho_Id;
            var itIds = await _context.Employees.Where(e => e.Employee_Job == "IT" && e.Ho_Id == hoId).Select(e => e.Employee_Id).ToListAsync();
            if (request.Request_Type == "AddDepartment")
            {
                request.Accept = true;
                Department department = JsonConvert.DeserializeObject<Department>(request.Request_Data);
                _context.Add(department);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تمت الموافقة على إضافة قسم" +department.Department_Name},
                        }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "DeleteDepartment")
            {
                request.Accept = true;
                var department = _context.Departments.Find(int.Parse(request.Request_Data));
                department.Active = false;
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تمت الموافقة على إزالة قسم" + department.Department_Name},
                        }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "AddRoom")
            {
                request.Accept = true;
                Room room = JsonConvert.DeserializeObject<Room>(request.Request_Data);
                _context.Add(room);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { "channelId","other" },
                        { "title", "تمت الموافقة على إضافة الغرفة" + room.Room_Number},
                    }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "DeleteRoom")
            {
                request.Accept = true;
                var room = _context.Rooms.Find(int.Parse(request.Request_Data));
                room.Active = false;
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تمت الموافقة على إزالة الغرفة" + room.Room_Number},
                        }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "AddSurgeryRoom")
            {
                request.Accept = true;
                Surgery_Room room = JsonConvert.DeserializeObject<Surgery_Room>(request.Request_Data);
                _context.Add(room);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تمت الموافقة على إضافة غرفة العمليات" + room.Su_Room_Number},
                        }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "DeleteSurgeryRoom")
            {
                request.Accept = true;
                var room = _context.Surgery_Rooms.Find(int.Parse(request.Request_Data));
                room.Active = false;
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تمت الموافقة على إزالة غرفة العمليات" + room.Su_Room_Number},
                        }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "AddDeptManager")
            {
                request.Accept = true;
                Doctor doctor = JsonConvert.DeserializeObject<Doctor>(request.Request_Data);
                _context.Add(doctor);
                await _context.SaveChangesAsync();
                var dep = await _context.Departments.FindAsync(doctor.Department_Id);
                dep.Dept_Manager = doctor;
                _context.Update(dep);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تمت الموافقة على تعيين د. " + doctor.Doctor_Full_Name+" رئيس قسم "+dep.Department_Name},
                        }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "UpdateDeptManager")
            {
                request.Accept = true;
                var doctor = _context.Doctors.Find(int.Parse(request.Request_Data));
                var department = _context.Departments.Find(doctor.Department_Id);
                department.Dept_Manager = doctor;
                _context.Update(department);
                await _context.SaveChangesAsync();
                #region send notification
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title", "تمت الموافقة على تعيين د. " + doctor.Doctor_Full_Name + " رئيس قسم " + department.Department_Name},
                        }
                };
                await FCMService.SendNotificationsToItAsync(itIds, UserType.emp, message);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }
            if (request.Request_Type == "AddSurgery")
            {
                request.Accept = true;
                Surgery surgery = JsonConvert.DeserializeObject<Surgery>(request.Request_Data);
                _context.Add(surgery);
                await _context.SaveChangesAsync();
                #region send notification to doctor
                //==============================================================================================
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                        {
                            { "channelId","other" },
                            { "title","تمت الموافقة على عملية المريض "+_context.Patients.Find(surgery.Patient_Id).Patient_Full_Name},
                        }
                };
                await FCMService.SendNotificationToUserAsync((int)surgery.Doctor_Id, UserType.doc, message);
                //=========================================================================================
                #endregion

                #region send notification to patient
                //==============================================================================================
                var message2 = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                {
                    { "channelId","other" },
                    { "title", "تم حجز العملية عند الطبيب "+_context.Doctors.Find(surgery.Doctor_Id).Doctor_Full_Name},
                }
                };
                await FCMService.SendNotificationToUserAsync((int)surgery.Patient_Id, UserType.pat, message2);
                //=========================================================================================
                #endregion
                return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
            }

                return Ok();
        }
        [Authorize(Roles = "IT")]
        public async Task<IActionResult> DenyRequest(int id, int mgrId)
        {
            var request = _context.Requests.Find(id);
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            //#region send notification to doctor
            ////==============================================================================================
            //Surgery surgery = JsonConvert.DeserializeObject<Surgery>(request.Request_Data);
            //var message = new MulticastMessage()
            //{
            //    Data = new Dictionary<string, string>()
            //            {
            //                { "channelId","other" },
            //                { "title", "لم تتم الموافقة على عملية المريض " + _context.Patients.Find(surgery.Patient_Id).Patient_Full_Name},
            //            }
            //};
            //await FCMService.SendNotificationToUserAsync((int)surgery.Doctor_Id, UserType.doc, message);
            ////=========================================================================================
            //#endregion
            return RedirectToAction("MasterHoMgr", "Employee", new { id = mgrId });
        }
        public async Task<IActionResult> EmergencyAlert(int id)
        {
            var hoId = _context.Employees.Find(id).Ho_Id;
            var dept = _context.Departments.FirstOrDefault(d => d.Ho_Id == hoId).Department_Id;
            var DocIds = await _context.Doctors.Where(e => e.Department_Id == dept).Select(e => e.Doctor_Id).ToListAsync();
            await FCMService.SendEmergencyNotificationsToDoctorsAsync(DocIds);
            return RedirectToAction("MasterHoMgr", "Employee", new { id = id });
        }
    }
}
