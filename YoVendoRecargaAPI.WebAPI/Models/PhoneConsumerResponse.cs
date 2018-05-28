using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class PhoneConsumerResponse : IResponse
    {
        public bool result { get; set; }
        public int consumerID { get; set; }
        public string message { get; set; }
        public string SecondsRemaining { get; set; }
    }
}