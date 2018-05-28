using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class NotificationContentRequest
    {
        public string countryIso2Code { get; set; }
        public string campaignTitle { get; set; }
        public string campaignDescription { get; set; }
        public string notificationTitle { get; set; }
        public string notificationMessage { get; set; }
        public string imageUrl { get; set; }
        public string SpecificUser { get; set; }
    }
}