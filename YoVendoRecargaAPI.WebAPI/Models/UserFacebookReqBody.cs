using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class UserFacebookReqBody
    {
        public long ID { get; set; }
        public string profileID { get; set; }
        public string userID { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string facebookURL { get; set; }
        public string phoneNumber { get; set; }
        public int? PersonID { get; set; }
    }
}