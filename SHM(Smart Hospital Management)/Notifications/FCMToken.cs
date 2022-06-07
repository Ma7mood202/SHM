using FirebaseAdmin.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SHM_Smart_Hospital_Management_.Notifications
{
    
    public class FCMToken
    {
        public string id { get; set; }
        public string android { get; set; }
        public string web { get; set; }



    }

}
