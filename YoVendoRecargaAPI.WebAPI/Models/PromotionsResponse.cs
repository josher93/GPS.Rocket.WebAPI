using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class PromotionsResponse : IResponse
    {
        public String Operator { get; set; }
        public String Tittle { get; set; }
        public String description { get; set; }
        public String URL { get; set; }
        public String Method { get; set; }
    }
}