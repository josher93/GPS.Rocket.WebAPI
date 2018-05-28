using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class DepositValidationRequest
    {
        public int userId { get; set; }
        public string userEmail { get; set; }
        public string userFirstname { get; set; }
        public string title { get; set; }
        public string message { get; set; }
    }
}