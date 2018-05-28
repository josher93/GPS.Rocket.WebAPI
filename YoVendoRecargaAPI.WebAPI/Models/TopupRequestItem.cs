using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TopupRequestItem
    {
        public int TopUpRequestID { get; set; }
        public string Nickname { get; set; }
        public string PhoneNumber { get; set; }
        public string OperatorName { get; set; }
        public string Amount { get; set; }
        public DateTime Date { get; set; }
        public string DateGMT { get; set; }
    }
}