using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TopupRequestsResponse : IResponse
    {
        public PendingRequests pendingRequests { get; set; }
        public int count { get; set; }
    }
}