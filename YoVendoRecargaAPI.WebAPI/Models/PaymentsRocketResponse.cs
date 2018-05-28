using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class PaymentsRocketResponse : IResponse
    {
        public PaymentsListResponse History { get; set; }
        public long count { get; set; }
        public String token { get; set; }
    }
}