using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class ComboSouvenirEN
    {
        public int ComboID { get; set; }
        public int PacketID { get; set; }
        public int AgeID { get; set; }
        public string Description { get; set; }
        public int Available { get; set; }
        public DateTime RegDate { get; set; }
        public int Secuence { get; set; }
        public string PrizeTitle { get; set; }
        public string PrizeDescription { get; set; }



        //This used to Souvenir
        public string ImgUrl { get; set; }
        public int Level { get; set; }
        public string Title { get; set; }
        public int FoundByConsumer { get; set; }


        //this properties used to new achievement

        public AchievementEN Achievement { get; set; }

        public PlayersTrackingEN tracking { get; set; }
    }
}
