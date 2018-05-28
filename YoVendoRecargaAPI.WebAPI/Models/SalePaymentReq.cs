using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SalePaymentReq 
    {
        public string id { get; set; }
        public string transaction_id { get; set; }
        public bool paid { get; set; }
    }
}