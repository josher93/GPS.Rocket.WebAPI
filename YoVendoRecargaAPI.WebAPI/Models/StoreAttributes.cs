using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class StoreAttributes
    {
        public string StoreName { get; set; }
        public string AddressStore { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string FirebaseID { get; set; }
        public int ConsumerID { get; set; }
    }
}