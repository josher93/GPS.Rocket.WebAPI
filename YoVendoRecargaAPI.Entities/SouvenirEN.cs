using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class SouvenirEN
    {
        public int SouvenirID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime ModDate { get; set; }
        public string ImgUrl { get; set; }
        public int Value { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public int AgeID { get; set; }
        public int Level { get; set; }
        public int FoundByConsumer { get; set; }
        public int Unlocked { get; set; }


        //this properties used to new achievement

        public AchievementEN Achievement { get; set; }

        public PlayersTrackingEN tracking { get; set; }
    }
}
