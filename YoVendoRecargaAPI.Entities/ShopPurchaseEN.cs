using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class ShopPurchaseEN
    {
        public int PurchasedID { get; set; }
        public int StoreID { get; set; }
        public int TrackSouvenirID { get; set; }
        public DateTime RegDate { get; set; }
        public DateTime ModDate { get; set; }
        public int ConsumerID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public int Value { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public PlayersTrackingEN tracking { get; set; }
        public AchievementEN Achievement { get; set; }
    }
}
