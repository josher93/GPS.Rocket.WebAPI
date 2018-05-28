using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class BagBL
    {
        BagDAL bagDAL = new BagDAL();

        public List<PersonBagOperatorEN> GetUserBags(int pPersonID)
        {
            List<PersonBagOperatorEN> personBags = new List<PersonBagOperatorEN>();

            try
            {
                personBags = bagDAL.GetUserOperatorBag(pPersonID);
            }
            catch (Exception ex)
            {
                personBags = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("GetUserBags: " + ex.Message);
            }

            return personBags;
        }
    }
}
