using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class PaymentsListResponse : IResponse
    {
        public List<RocketBalance> RocketBalanceList { get; set; }
    }
}