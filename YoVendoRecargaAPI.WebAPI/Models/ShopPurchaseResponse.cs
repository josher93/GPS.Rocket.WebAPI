using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ShopPurchaseResponse : IResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int Value { get; set; }
        public TrackingResponse tracking { get; set; }
        public NewAchievement Achievement { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}