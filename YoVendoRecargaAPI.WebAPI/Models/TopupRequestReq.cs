using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TopupRequestReq
    {
        public int consumerId { get; set; }
        public string targetPhoneNumber { get; set; }
        public int vendorCode { get; set; }
        public int operatorID { get; set; }
        public decimal amount { get; set; }
        public int statusCode { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime ModDate { get; set; }
        public int CategoryID { get; set; }
    }
}