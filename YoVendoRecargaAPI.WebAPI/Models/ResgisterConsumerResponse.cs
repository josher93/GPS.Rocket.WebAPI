using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class RegisterConsumerResponse: IResponse
    {
        public String firstName { get; set; }
        public String middleName { get; set; }
        public String lastName { get; set; }
        public String nickname { get; set; }
        public String phone { get; set; }
        public String email { get; set; }
        public int countryID { get; set; }
        public String authenticationKey { get; set; }
    }
}