using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class WinPrizeEN
    {
        public int WinPrizeID { get; set; }
        public int PrizeID { get; set; }
        public int ConsumerID { get; set; }
        public DateTime RegDate { get; set; }

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
        public int TotalWinCoins { get; set; }
        public int TotalWinPrizes { get; set; }
        public int CurrentCoinsProgress { get; set; }
        public int AgeID { get; set; }
        public int TotalSouvenirs { get; set; }
        public PlayersTrackingEN tracking { get; set; }
        public string WaitTime { get; set; }

        //this properties used to new achievement

        public AchievementEN Achievement { get; set; }
    }
}
