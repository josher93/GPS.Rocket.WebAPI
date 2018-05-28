using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class RocketBalance : IResponse
    {
        public int  balanceID { get; set; }
        public decimal balanceAmount { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public int status { get; set; }
        public DateTime conciliationDate { get; set; }
        public decimal profit { get; set; }
    }
}