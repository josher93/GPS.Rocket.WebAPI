using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TrackingResponse : IResponse
    {
        public int TotalWinCoins { get; set; }
        public int TotalWinPrizes { get; set; }
        public int CurrentCoinsProgress { get; set; }
        public int TotalSouvenirs { get; set; }
        public int AgeID { get; set; }
        public string Nickname { get; set; }
    }
}