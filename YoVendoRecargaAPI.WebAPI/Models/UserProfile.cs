using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class UserProfile
    {
        public long Id { get; set; }
        public DateTime birthday { get; set; }
        public String first_name { get; set; }
        public String last_name { get; set; }
        public String middle_name { get; set; }
        public String gender { get; set; }
        public String verified { get; set; }
        public String email { get; set; }
        public String NickName { get; set; }
        public String category { get; set; }
        public int vendorId { get; set; }
        public String Iso2code { get; set; }
        public String Symbol { get; set; }
        public String Personphone { get; set; }
        public string LastSale { get; set; }
    }
}