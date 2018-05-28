using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SigninReqBody
    {
        public string email { get; set; }
        public string password { get; set; }
        public string deviceInfo { get; set; }
        public string deviceIP { get; set; }
        public string deviceID { get; set; }
        public string buildVersion { get; set; }
    }
}