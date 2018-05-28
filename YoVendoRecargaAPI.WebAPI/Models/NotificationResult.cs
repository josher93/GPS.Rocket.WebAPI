using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class NotificationResult
    {
        public int Code { get; set; }
        public string Result { get; set; }
        public string Platform { get; set; }
    }
}