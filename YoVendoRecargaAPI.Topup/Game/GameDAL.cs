using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Topup.Utils;

namespace YoVendoRecargaAPI.Topup.Game
{
    public class GameDAL
    {
        Connection connection = new Connection();

        public PlayersTrackingEN GetProgressGame(int consumerID)
        {
            PlayersTrackingEN result = new PlayersTrackingEN();

            try
            {
                connection.Cnn.Open();

                result = connection.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                result = null;
                Console.WriteLine("GetProgressGameDAL: " + ex.Message);
                EventViewerLogger.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return result;
        }

        public int InsertPlayerTracking(int pTotalWinCoins, int pCurrentCoinsProgress, int pConsumerID, DateTime pDate)
        {
            int result = default(int);

            try
            {
                connection.Cnn.Open();
                using (SqlTransaction tran = connection.Cnn.BeginTransaction())
                {
                    result = connection.Cnn.Query<int>(@"INSERT INTO [Consumer].[PlayersTracking](TotalWinCoins, CurrentCoinsProgress, TotalWinPrizes, ConsumerID, RegDate)
                                                    VALUES(@TotalWinCoins,
                                                           @CurrentCoinsProgress,
                                                           @TotalWinPrizes,
                                                           @ConsumerID,
                                                           @RegDate)", new
                                                                     {
                                                                         TotalWinCoins = pTotalWinCoins,
                                                                         CurrentCoinsProgress = pCurrentCoinsProgress,
                                                                         TotalWinPrizes = 0,
                                                                         ConsumerID = pConsumerID,
                                                                         RegDate = pDate
                                                                     }, tran).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                result = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("InsertPlayerTracking: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return result;
        }

        public int UpdatePlayerTracking(PlayersTrackingEN tracking)
        {
            int? result = default(int);

            try
            {
                connection.Cnn.Open();
                result = connection.Cnn.Update(tracking);

            }
            catch (Exception ex)
            {
                result = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("UpdatePlayerTracking: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return Convert.ToInt32(result);
        }

        public int InsertWinCoin(WinCoinEN WinCoin) 
        {
            int? inserted = default(int);

            try
            {
                connection.Cnn.Open();

                inserted = connection.Cnn.Insert(WinCoin);

            }
            catch (Exception ex)
            {
                inserted = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("InsertWinCoin: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return Convert.ToInt32(inserted);
        }
    }
}
