using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class PendingTopupReq
    {
        public int PendingRequestID { get; set; }
        public bool ResponseToRequest { get; set; }
    }
}