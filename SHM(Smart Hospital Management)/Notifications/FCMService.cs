using FirebaseAdmin.Messaging;
using SHM_Smart_Hospital_Management_.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Notifications
{
    public enum UserType
    {
        doc,
        pat,
        emp,
    }
    public enum Platform
    {
        Android,
        Web,
    }


    public class FCMService
    {

        #region storing tokens

        static public void AddToken(int id, UserType user) //this to be used when creating new user
        {
            StreamReader reader = new StreamReader(@"wwwroot\FCMTokens.json");
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            List<FCMToken> fcmtokens = JsonConvert.DeserializeObject<List<FCMToken>>(jsonValue);
            FCMToken newToken = new FCMToken();
            newToken.id = user + "-" + id;
            newToken.web = "";
            newToken.android = "";

            fcmtokens.Add(newToken);
            var json = JsonConvert.SerializeObject(fcmtokens, Formatting.Indented);
            StreamWriter writer = new StreamWriter(@"wwwroot\FCMTokens.json", false);

            writer.Write(json);
            writer.Close();
        }

        static public void UpdateToken(string token, int id, UserType user, Platform platform) //this to be used after login
        {
            // Save FCM Token
            StreamReader reader = new StreamReader(@"wwwroot\FCMTokens.json");

            string jsonValue = reader.ReadToEnd();
            reader.Close();
            List<FCMToken> fcmtokens = JsonConvert.DeserializeObject<List<FCMToken>>(jsonValue);
            Console.WriteLine(fcmtokens.ToString());
            foreach (var record in fcmtokens)
            {
                if (record.id == user + "-" + id)
                {
                    if (platform == Platform.Android)
                    {
                        record.android = token;
                    }
                    else if (platform == Platform.Web)
                    {
                        record.web = token;
                    }
                    break;
                }
            }
            var json = JsonConvert.SerializeObject(fcmtokens, Formatting.Indented);
            StreamWriter writer = new StreamWriter(@"wwwroot\FCMTokens.json", false);

            writer.Write(json);
            writer.Close();
            // End save FCM Token
        }

        static public void RemoveUnusedToken(int id, UserType user, Platform platform) //this is to be used after logout
        {
            // Save FCM Token
            StreamReader reader = new StreamReader(@"wwwroot\FCMTokens.json");
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            List<FCMToken> fcmtokens = JsonConvert.DeserializeObject<List<FCMToken>>(jsonValue);
            Console.WriteLine(fcmtokens.ToString());
            foreach (var record in fcmtokens)
            {
                if (record.id == user + "-" + id)
                {
                    if (platform == Platform.Android)
                    {
                        record.android = "";
                    }
                    else if (platform == Platform.Web)
                    {
                        record.web = "";
                    }
                    break;
                }
            }
            var json = JsonConvert.SerializeObject(fcmtokens, Formatting.Indented);
            StreamWriter writer = new StreamWriter(@"wwwroot\FCMTokens.json", false);

            writer.Write(json);
            writer.Close();
            // End save FCM Token
        }

        static public void RemoveToken(int id, UserType user) //this is to be used when deleting user
        {
            StreamReader reader = new StreamReader(@"wwwroot\FCMTokens.json");
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            List<FCMToken> fcmtokens = JsonConvert.DeserializeObject<List<FCMToken>>(jsonValue);
            var t = fcmtokens.Find(t => t.id == user + "-" + id);
            fcmtokens.Remove(t);
            var json = JsonConvert.SerializeObject(fcmtokens, Formatting.Indented);
            StreamWriter writer = new StreamWriter(@"wwwroot\FCMTokens.json", false);

            writer.Write(json);
            writer.Close();
        }

        #endregion


        static public async Task SendNotificationToUserAsync(int id, UserType user, MulticastMessage message)
        {
            //
            //you can specify the message like this
            //

            //var message = new Message()
            //{
            //    //optional but prefer to leave it 
            //    Notification = new Notification()
            //    {
            //        Title = "$GOOG up 1.43% on the day",
            //        Body = "$GOOG gained 11.80 points to close at 835.67, up 1.43% on the day.",
            //    },
            //    //optional
            //    Data = new Dictionary<string, string>()
            //        {
            //            { "score", "850" },
            //            { "time", "2:45" },
            //        },
            //};
            List<string> tokens = new List<string>();

            tokens = GetToken(id,user);

            if ((tokens.Count != 0))
            {
                message.Tokens = tokens;
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return;
            }
            return;
            
        }

        static public async Task SendNotificationsToItAsync(List<int> ids, UserType user, MulticastMessage message)
        {
            List<string> tokens = new List<string>();
            tokens = GetAllITTokens(ids,user);
            if ((tokens.Count != 0))
            {
                message.Tokens = tokens;
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return;
            }
            return;
        }
        static public async Task SendEmergencyNotificationsToDoctorsAsync(List<int> ids)
        {
            List<string> tokens = new List<string>();
            tokens = GetAllITTokens(ids,UserType.doc);
            if ((tokens.Count != 0))
            {
                var message = new MulticastMessage()
                {
                    Data = new Dictionary<string, string>()
                    {
                        { 
                            "Emergency", "true" 
                        },
                           { "title","حالة طوارئ!!!!" },
                           { "body","الرجاء القدوم الى المشفى حالاً!!" },
                    },

                    Tokens = tokens,
                    Android=new AndroidConfig()
                    {
                        CollapseKey= "Emergency",
                        Priority=Priority.High,
                    }
                };
                var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
                return;
            }
            return;
        }


        static List<string> GetToken(int id, UserType user)
        {
            StreamReader reader = new StreamReader(@"wwwroot\FCMTokens.json");
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            List<FCMToken> fcmtokens = JsonConvert.DeserializeObject<List<FCMToken>>(jsonValue);
            var token = fcmtokens.Find(t => t.id == user + "-" + id.ToString());
            List<string> tokens = new List<string>();

            if (!String.IsNullOrEmpty(token.android))
            {
                tokens.Add(token.android);

            }
            if (!String.IsNullOrEmpty(token.web))
            {
                tokens.Add(token.web);

            }
            return tokens;
        }


        static List<string> GetAllITTokens(List<int> ids,UserType user)
        {
            StreamReader reader = new StreamReader(@"wwwroot\FCMTokens.json");
            string jsonValue = reader.ReadToEnd();
            reader.Close();
            List<FCMToken> fcmtokens = JsonConvert.DeserializeObject<List<FCMToken>>(jsonValue);
            List<string> tokens = new List<string>();
            foreach (var item in ids)
            {
                var token = fcmtokens.Find(t => t.id == user+"-" + item.ToString());
                if (!String.IsNullOrEmpty(token.android))
                {
                    tokens.Add(token.android);

                }
                if (!String.IsNullOrEmpty(token.web))
                {
                    tokens.Add(token.web);

                }
            }
            return tokens;
        }

    }
}
