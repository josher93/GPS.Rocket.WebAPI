using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class BankDAL
    {
        private Connection cnn = new Connection();

        public List<BankEN> GetBanks(int pUserCountryID)
        {
            List<BankEN> banksList = new List<BankEN>();

            try
            {
                banksList = cnn.Cnn.Query<BankEN>("SpGetBanks",
                    new { userCountryID = pUserCountryID }, commandType: CommandType.StoredProcedure).AsList();

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error BankDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return banksList;
        }
    }
}
