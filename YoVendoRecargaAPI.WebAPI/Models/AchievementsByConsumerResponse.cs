using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class AchievementsByConsumerResponse
    {
        //Achievements
        public Int64 AchievementID { get; set; }
        public string Name { get; set; }
        //AchievementsByConsumer
        public int Score { get; set; }
        public int Level { get; set; }
        public bool Won { get; set; }

        public int NextPrize { get; set; }
        public int NextLevel { get; set; }
        public string Description { get; set; }
        
    }
}