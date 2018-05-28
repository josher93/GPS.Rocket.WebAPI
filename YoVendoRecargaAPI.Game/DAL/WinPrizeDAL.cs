using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;

namespace YoVendoRecargaAPI.Game.DAL
{
    public class WinPrizeDAL
    {
         private Connection con { get; set; }

        public WinPrizeDAL()
        {
            this.con = new Connection();
        }

        public WinPrizeEN IsAvailableWinPrize(int consumerID, ref string error)
        {

            WinPrizeEN result = new WinPrizeEN();

            try
            {
                con.Cnn.Open();

                result = con.Cnn.Query<WinPrizeEN>("GetPrizeByConsumerID", new { @ConsumerID = consumerID },
                   commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error WinPrizeDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }
    }
}
