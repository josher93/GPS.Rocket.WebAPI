using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class NotificationTrackingReq
    {
        public string trackingID { get; set; }
        public string AmeDeviceID { get; set; }
    }
}