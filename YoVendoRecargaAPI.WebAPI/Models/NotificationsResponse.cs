using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class NotificationsResponse : IResponse
    {
        public NotificationMessages notifications { get; set; }
        public int count { get; set; }
    }
}