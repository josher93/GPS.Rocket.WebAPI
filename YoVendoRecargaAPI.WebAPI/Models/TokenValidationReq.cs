using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TokenValidationReq
    {
        public int consumerID { get; set; }
        public string token { get; set; }
    }
}