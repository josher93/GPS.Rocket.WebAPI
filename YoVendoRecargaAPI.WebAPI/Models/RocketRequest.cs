using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class RocketRequest
    {
        public int id { get; set; }
        public int BalanceID { get; set; }
        public string SecurityPin { get; set; }
    }
}