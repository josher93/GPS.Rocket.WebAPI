using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class RegisterPhoneReqBody
    {
        public String phone { get; set; }
        public String countryID { get; set; }
        public String deviceID { get; set; }
    }
}