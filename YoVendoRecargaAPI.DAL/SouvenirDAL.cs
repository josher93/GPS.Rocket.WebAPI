using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class SouvenirDAL
    {
        private Connection con { get; set; }
        private SqlConnection connection { get; set; }

        public SouvenirDAL()
        {
            this.con = new Connection();
        }

        #region GetStoreItems
        public List<SouvenirEN> GetSouvenirsOwnedByConsumer(int consumerID, ref string error)
        {
            List<SouvenirEN> result = new List<SouvenirEN>();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<SouvenirEN>("SpGetSouvenirsOwnedByConsumer", new { ConsumerID = consumerID }, commandType: CommandType.StoredProcedure).ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SouvenirDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;

        }
        #endregion
    }
}
