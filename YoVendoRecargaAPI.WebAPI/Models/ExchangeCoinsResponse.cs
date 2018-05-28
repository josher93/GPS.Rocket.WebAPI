using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ExchangeCoinsResponse : IResponse
    {
        public int Type { get; set; }
        public int ExchangeCoins { get; set; }
        public TrackingResponse tracking { get; set; }
        public NewAchievement Achievement { get; set; }
        public string Code { get; set; }
        public String Message { get; set; }
    }
}