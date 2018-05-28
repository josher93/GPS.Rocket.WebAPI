using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SouvenirsByConsumerResponse
    {
        public int SouvenirID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AgeID { get; set; }
        public string ImgUrl { get; set; }
        public int Level { get; set; }
        public int SouvenirsOwnedByConsumer { get; set; }
        public int Unlocked { get; set; }
    }

    public class SouvenirCollection : IResponse
    {
        public List<SouvenirsByConsumerResponse> listSouvenirsByConsumer { get; set; }
    }
}