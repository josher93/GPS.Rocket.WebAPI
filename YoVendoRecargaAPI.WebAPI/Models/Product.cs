using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class Product
    {
        public String Code { get; set; }
        public String Amount { get; set; }
        public String Description { get; set; }
        public int PackageCode { get; set; }
        public int Relevance { get; set; }
    }
}