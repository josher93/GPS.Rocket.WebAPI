using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class RocketSalesDetailResponse : IResponse
    {
        public SaleDetailListResponse SaleDetail { get; set; }
        public long count { get; set; }
        public String token { get; set; }
    }
}