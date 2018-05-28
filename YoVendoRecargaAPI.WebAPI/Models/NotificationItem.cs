using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class NotificationItem
    {
        public int trackingID { get; set; }
        public int notificationID { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public bool seen { get; set; }
    }
}