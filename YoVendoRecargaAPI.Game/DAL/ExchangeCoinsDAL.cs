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
    public class ExchangeCoinsDAL
    {
        private Connection con { get; set; }
        GameDAL gameDal = new GameDAL();
        SqlTransaction transaction;

        public ExchangeCoinsDAL()
        {
            this.con = new Connection();
        }
        public WinCoinEN Exist(int consumerID, string locationID, DateTime regDate, ref string error)
        {

            WinCoinEN result = new WinCoinEN();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<WinCoinEN>("SpExistLocationByConsumerAndDate", new { @ConsumerID = consumerID, @LocationID = locationID, @RegDate = regDate },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                EventViewerLoggerDAL.LogError(ex.Message);
                result = null;
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public PlayersTrackingEN GetProgressGame(int consumerID, ref string error)
        {

            PlayersTrackingEN result = new PlayersTrackingEN();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ExchangeCoinsDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public AchievementEN SaveData(int consumerID, string locationID, decimal Longitude, decimal Latitude, DateTime regDate, int ChestType, int ExchangeCoins, int TotalWinCoins, int CurrentCoinsProgress, int TotalWinPrizes, ref string error, int AgeID)
        {
            AchievementEN result = new AchievementEN();
            
            try
            {

                con.Cnn.Open();
                con.Tra = con.Cnn.BeginTransaction();
                result = con.Cnn.Query<AchievementEN>("SpSaveExchangeCoinsAndProgressGame", new
                {
                    @ConsumerID = consumerID,
                    @LocationID = locationID,
                    @Longitude = Longitude,
                    @Latitude = Latitude,
                    @RegDate = regDate,
                    @ChestType = ChestType,
                    @ExchangeCoins = ExchangeCoins,
                    @TotalWinCoins = TotalWinCoins,
                    @CurrentCoinsProgress = CurrentCoinsProgress,
                    @TotalWinPrizes = TotalWinPrizes,
                    @AgeID = AgeID
                }, con.Tra,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();


                if (result.Code == "00")
                {
                    result = ProcessToWinAchievement(consumerID, ExchangeCoins, TotalWinCoins, 1, con.Tra);



                    result.Tracking = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID }, con.Tra,
                   commandType: CommandType.StoredProcedure).FirstOrDefault();

                }


                con.Tra.Commit();

            }
            catch (Exception ex)
            {
                con.Tra.Rollback();
                Console.WriteLine("Error ExchangeCoinsDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public PlayersTrackingEN GetProgressGameIsNotExistTracking(int consumerID, string LocationID, DateTime RegDate, ref string error)
        {

            PlayersTrackingEN result = new PlayersTrackingEN();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumerIsNotExistTracking", new { ConsumerID = consumerID, LocationID = LocationID, RegDate = RegDate },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ExchangeCoinsDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
                result = null;
                error = ex.Message;
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public SouvenirEN SpSaveSouvenirAndProgressGame(int consumerID, string locationID, decimal Longitude, decimal Latitude, DateTime regDate, int ChestType, ref string error, int AgeID, int Level, int TotalWinCoins)
        {

            SouvenirEN result = new SouvenirEN();
            AchievementEN achievementEN = new AchievementEN();
            GameDAL gameDal = new GameDAL();
            try
            {

                con.Cnn.Open();
                con.Tra = con.Cnn.BeginTransaction();

                result = con.Cnn.Query<SouvenirEN>("SpSaveSouvenirAndProgressGame", new { ConsumerID = consumerID, LocationID = locationID, Longitude = Longitude, Latitude = Latitude, RegDate = regDate, ChestType = ChestType, AgeID = AgeID, Level = Level, TotalWinCoins = TotalWinCoins }, 
                    con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();


                if (result.Code == "00")
                {
                    result.Achievement = ProcessToWinAchievement(consumerID, 1,TotalWinCoins, 7, con.Tra);
                }

                result.tracking = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                    con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();

                con.Tra.Commit();

            }
            catch (Exception ex)
            {
                con.Tra.Rollback();
                Console.WriteLine("Error ExchangeCoinsDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public AchievementEN SaveCoinsWildCard(int consumerID, string locationID, decimal Longitude, decimal Latitude, DateTime regDate, int ExchangeCoins, int TotalWinCoins, int CurrentCoinsProgress, int TotalWinPrizes, ref string error, int AgeID)
        {
            AchievementEN result = new AchievementEN();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<AchievementEN>("SpSaveCoinsWildCard", new
                {
                    @ConsumerID = consumerID,
                    @LocationID = locationID,
                    @Longitude = Longitude,
                    @Latitude = Latitude,
                    @RegDate = regDate,
                    @ExchangeCoins = ExchangeCoins,
                    @TotalWinCoins = TotalWinCoins,
                    @CurrentCoinsProgress = CurrentCoinsProgress,
                    @TotalWinPrizes = TotalWinPrizes,
                    @AgeID = AgeID
                },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                result.Tracking = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                   commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ExchangeCoinsDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public AchievementEN ProcessToWinAchievement(int consumerID, int Value, int TotalWinCoins, int AchievementID, SqlTransaction transaction)
        {
            try
            {
                AchievementEN achievement = new AchievementEN();

                var p = new DynamicParameters();
                p.Add("AchievementID", AchievementID);
                p.Add("ConsumerID", consumerID);
                p.Add("Value", Value);
                p.Add("CurrentCoinsProgress", 20);
                p.Add("TotalWinCoins", TotalWinCoins);


                achievement = con.Cnn.Query<AchievementEN>("SpProcessToWinAchievement", p, transaction, commandType: CommandType.StoredProcedure).FirstOrDefault();



                return achievement;
            }
            catch (Exception ex)
            {
                EventViewerLoggerDAL.LogError("GameDAL... SpProcessToWinAchievement - Error: " + ex.StackTrace);
                throw;
            }
        }
    }
}
