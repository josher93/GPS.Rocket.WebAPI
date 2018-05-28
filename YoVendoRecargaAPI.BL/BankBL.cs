using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class BankBL
    {
        BankDAL bankDAL = new BankDAL();

        public List<BankEN> GetBankList(int pUserCountryID)
        {
            List<BankEN> bankList = new List<BankEN>();

            try
            {
                bankList = bankDAL.GetBanks(pUserCountryID);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return bankList;
        }
    }
}
