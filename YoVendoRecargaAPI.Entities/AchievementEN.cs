using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class AchievementEN
    {
        //This Entity is based on both tables: "Achivement" and "achievemntConsumer"
        public Int64 AchievementID { get; set; }
        public string Name { get; set; }
        public int FirstLevel { get; set; }
        public int SecondLevel { get; set; }
        public int ThirdLevel { get; set; }
        public int Status { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime ModDate { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public Int32 Won { get; set; }
        public int PrizeFirstLevel { get; set; }
        public int PrizeSecondLevel { get; set; }
        public int PrizeThirdLevel { get; set; }
        public string Description { get; set; }
        public int Prize { get; set; }
        //this properties used to new achievement
        public int ValueNextLevel { get; set; }
        public int NewLevel { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public PlayersTrackingEN Tracking { get; set; }
        
    }
}
