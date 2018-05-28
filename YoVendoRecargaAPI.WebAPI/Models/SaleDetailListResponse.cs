using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SaleDetailListResponse : IResponse
    {
        public RocketSaleDetail AllSales { get; set; }
    }
}