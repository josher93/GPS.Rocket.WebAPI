using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class WinPrizeResponse : IResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string logoUrl { get; set; }
        public string HexColor { get; set; }
        public string RGBColor { get; set; }
        public string Dial { get; set; }
        public string Code { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public int PrizeLevel { get; set; }
        public TrackingResponse tracking { get; set; }
        public string WaitTime { get; set; }

        public NewAchievement Achievement { get; set; }
    }
}