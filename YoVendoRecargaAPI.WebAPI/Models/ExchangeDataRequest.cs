using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ExchangeDataRequest
    {
        public string LocationID { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int ChestType { get; set; }
        public int AgeID { get; set; }
    }
}