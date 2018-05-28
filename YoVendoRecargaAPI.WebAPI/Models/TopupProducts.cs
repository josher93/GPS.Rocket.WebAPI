using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TopupProducts : IResponse
    {
        public string mno { get; set; }
        public List<Product> denomination { get; set; }
        public String currency { get; set; }
    }
}