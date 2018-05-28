using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class StoreBL
    {
        StoreDAL storeDAL = new StoreDAL();
        public int? AirTimeReporting(string pStoreName, string pAddressStore, decimal pLongitude, decimal pLatitude, string pFirebaseID, int pConsumerID, DateTime pRegDate, DateTime pModDate, ref string error)
        {
            var result = storeDAL.AirTimeReporting(pStoreName, pAddressStore, pLongitude, pLatitude, pFirebaseID, pConsumerID, pRegDate, pModDate, ref error);
            return result;
        }

        public List<StoreEN> GetStoreItems(int ConsumerID)
        {
            var result = storeDAL.GetStoreItems(ConsumerID);
            return result;
        }
    }
}
