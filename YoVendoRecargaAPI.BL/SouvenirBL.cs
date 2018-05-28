using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class SouvenirBL
    {
        SouvenirDAL souvenirDAL = new SouvenirDAL();
        public List<SouvenirEN> GetSouvenirsOwnedByConsumer(int consumerID, ref string error)
        {
            var result = souvenirDAL.GetSouvenirsOwnedByConsumer(consumerID, ref error);
            return result;
        }
    }
}
