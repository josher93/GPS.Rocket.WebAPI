using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class PendingRequests
    {
        public List<TopupRequestItem> PendingRequestsList { get; set; }
    }
}