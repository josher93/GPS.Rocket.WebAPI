using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class Sale
    {
        public String id { get; set; }
        public String transaction_id { get; set; }
        public String msisdn { get; set; }
        public Decimal Amount { get; set; }
        public String FormattedAmount { get; set; }
        public DateTime date { get; set; }
        public String serverDateGMT { get; set; }
        public String salesman { get; set; }
        public String Pais { get; set; }
        public String Operator { get; set; }
        public string currency { get; set; }
        public String status { get; set; }
        public bool paid { get; set; }
    }
}