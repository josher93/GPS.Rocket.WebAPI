using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SouvenirResponse : IResponse
    {
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int Value { get; set; }
        public TrackingResponse tracking { get; set; }
        public NewAchievement Achievement { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class CombosSouvenirResponse
    {
        public string ImgUrl { get; set; }
        public int Level { get; set; }
        public int Exchangeable { get; set; }
        public string Title { get; set; }

    }
}