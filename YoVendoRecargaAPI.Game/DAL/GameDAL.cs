using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace YoVendoRecargaAPI.Game.DAL
{
    public class GameDAL
    {
        private Connection con;

        public GameDAL()
        {
            this.con = new Connection();
        }

        public AllLeaderBoards GetLeaderBoards()
        {
            AllLeaderBoards result = new AllLeaderBoards();

            try
            {
               con.Cnn.Open();

                using (var multi = con.Cnn.QueryMultiple("SP_GetLeaderBoards", commandType: CommandType.StoredProcedure))
                {
                    var ListLBToday = multi.Read<LeaderBoards>().ToList();
                    var ListLBWeek = multi.Read<LeaderBoards>().ToList();
                    var ListLBMonth = multi.Read<LeaderBoards>().ToList();
                    var ListLBOverAll = multi.Read<LeaderBoards>().ToList();

                    result.ListLeaderBoardsToday = ListLBToday;
                    result.ListLeaderBoardsWeek = ListLBWeek;
                    result.ListLeaderBoardsMonth = ListLBMonth;
                    result.ListLeaderBoardsOverAll = ListLBOverAll;
                } 


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public LastWinnerLeaderBoards GetLastWinner()
        {

            LastWinnerLeaderBoards result = new LastWinnerLeaderBoards();

            try
            {
                con.Cnn.Open();

                using (var multi = con.Cnn.QueryMultiple("SP_GetLastWinnerLeaderBoards", commandType: CommandType.StoredProcedure))
                {
                    var LastWinnerLBYesterday = multi.Read<LeaderBoards>().FirstOrDefault();
                    var LastWinnerLBWeek = multi.Read<LeaderBoards>().FirstOrDefault();
                    var LastWinnerLBMonth = multi.Read<LeaderBoards>().FirstOrDefault();

                    result.LastWinnerOnYesterday = LastWinnerLBYesterday;
                    result.LastWinnerOnWeek = LastWinnerLBWeek;
                    result.LastWinnerOnMonth = LastWinnerLBMonth;
                } 

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsToday()
        {
            List<LeaderBoards> result = new List<LeaderBoards>();

            try
            {
                con.Cnn.Open();

                result = con.Cnn.Query<LeaderBoards>(@"select * from dbo.VLeaderBoardsToday").ToList();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsWeek()
        {

            List<LeaderBoards> result = new List<LeaderBoards>();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<LeaderBoards>(@"select * from dbo.VLeaderBoardsWeek").ToList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsMonth()
        {

            List<LeaderBoards> result = new List<LeaderBoards>();

            try
            {

                result = Connection.Query<LeaderBoards>(@"select * from dbo.VLeaderBoardsMonth").ToList();



            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);

                throw;
            }
            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsOverAll()
        {

            List<LeaderBoards> result = new List<LeaderBoards>();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<LeaderBoards>(@"select * from dbo.VLeaderBoardsOverAll").ToList();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public List<PrizeHistoryEN> GetPrizeHistoryByConsumerID(int ConsumerID, ref string error)
        {

            List<PrizeHistoryEN> result = new List<PrizeHistoryEN>();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<PrizeHistoryEN>("SpGetPrizeHistoryByConsumerID", new { @ConsumerID = ConsumerID },
                    commandType: CommandType.StoredProcedure).ToList();

            }
            catch (Exception ex)
            {
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public LeaderBoards GetLastWinnerOnYesterday()
        {

            LeaderBoards result = new LeaderBoards();

            try
            {
                con.Cnn.Open();

                result = con.Cnn.Query<LeaderBoards>(@"select * from dbo.VLastWinnerOnYesterday").FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public LeaderBoards GetLastWinnerOnLastWeek()
        {

            LeaderBoards result = new LeaderBoards();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<LeaderBoards>(@"select * from dbo.VLastWinnerOnWeek").FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public LeaderBoards GetLastWinnerOnLastMonth()
        {

            LeaderBoards result = new LeaderBoards();

            try
            {
                result = Connection.Query<LeaderBoards>(@"select * from dbo.VLastWinnerOnMonth").FirstOrDefault();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);

                throw;
            }

            return result;
        }

        public WinPrizeEN ProcessToWinPrize(int consumerID, int CountryID, ref string error)
        {

            WinPrizeEN result = new WinPrizeEN();
            AchievementEN achievementEN = new AchievementEN();

            try
            {

                con.Cnn.Open();
                con.Tra = con.Cnn.BeginTransaction();

                result = con.Cnn.Query<WinPrizeEN>("SpProcessToWinPrize", new { ConsumerID = consumerID, CountryID = CountryID }, con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result.ResponseCode == "04")
                {
                    result = con.Cnn.Query<WinPrizeEN>("SpProcessToWinPrize", new { ConsumerID = consumerID, CountryID = CountryID }, con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (result.ResponseCode == "00")
                    {
                        result.Achievement = ProcessToWinAchievement(consumerID, 1, result.TotalWinCoins, 8, con.Tra);
                    }
                }
                else if (result.ResponseCode == "00")
                {
                    result.Achievement = ProcessToWinAchievement(consumerID, 1, result.TotalWinCoins, 8, con.Tra);
                }

                con.Tra.Commit();
            }
            catch (Exception ex)
            {
                con.Tra.Rollback();
                result.Code = "500";
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public WinPrizeEN GetLastWinPrize(int consumerID, ref string error)
        {

            WinPrizeEN result = new WinPrizeEN();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<WinPrizeEN>("SpGetLastWinPrize", new { ConsumerID = consumerID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public ShopPurchaseEN PurchaseAndGetSouvenir(int consumerID, int StoreId, ref string error, int LevelBarrel1, int LevelBarrel2)
        {

            ShopPurchaseEN result = new ShopPurchaseEN();
            AchievementEN achievement = new AchievementEN();
            try
            {

                con.Cnn.Open();
                con.Tra = con.Cnn.BeginTransaction();

                result = con.Cnn.Query<ShopPurchaseEN>("SpPurchaseStore", new
                {
                    ConsumerID = consumerID,
                    StoreId = StoreId,
                    LevelBarrel1 = LevelBarrel1,
                    LevelBarrel2 = LevelBarrel2
                }, con.Tra,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result.Code == "00")
                {
                    result.tracking = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                        con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    int TotalWinCoins = result.tracking.TotalWinCoins;


                    result.Achievement = ProcessToWinAchievement(consumerID, 1, TotalWinCoins, 7, con.Tra);



                }

                con.Tra.Commit();

            }
            catch (Exception ex)
            {
                con.Tra.Rollback();
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
                error = ex.StackTrace;
                result = null;
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public WinPrizeEN ProcessToExchangeSouvenirByPrize(int consumerID, int SouvenirID, int CountryID, ref string error)
        {

            WinPrizeEN result = new WinPrizeEN();
            AchievementEN achievement = new AchievementEN();
            int TotalWinCoins = 0;

            try
            {


                con.Cnn.Open();
                con.Tra = con.Cnn.BeginTransaction();

                result = con.Cnn.Query<WinPrizeEN>("SpExchangeSouvenirByPrize", new { ConsumerID = consumerID, SouvenirID = SouvenirID, CountryID = CountryID },
                    con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result.ResponseCode == "04")
                {
                    result = con.Cnn.Query<WinPrizeEN>("SpExchangeSouvenirByPrize", new { ConsumerID = consumerID, SouvenirID = SouvenirID, CountryID = CountryID },
                    con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }


                result.tracking = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                    con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();


                TotalWinCoins = result.tracking.TotalWinCoins;

                result.Achievement = ProcessToWinAchievement(consumerID, 1, TotalWinCoins, 8, con.Tra);

                con.Tra.Commit();

            }
            catch (Exception ex)
            {
                con.Tra.Rollback();
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
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

        public WinPrizeEN ProcessToWinPrizeWildcard(int consumerID, string LocationID, int AgeID, ref string error)
        {

            WinPrizeEN result = new WinPrizeEN();
            AchievementEN achievementEN = new AchievementEN();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<WinPrizeEN>("SpProcessToWinPrizeWildcard", new { ConsumerID = consumerID, LocationID = LocationID, AgeID = AgeID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

                result.tracking = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            catch (Exception ex)
            {
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public int InsertActivatePIN(int consumerID, string Phone, string PIN, string ProviderName, string ResponseProvider, ref string error)
        {
            EventViewerLoggerDAL.LogError("InsertActivatePIN... ConsumerID: " + consumerID.ToString() + " , Phone: " + Phone + " , PIN: " + PIN + " , Response WS Claro: " + ResponseProvider);
            int result = -1;

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<int>("InsertActivatePin", new { ConsumerID = consumerID, Phone = Phone, PIN = PIN, ProviderName = ProviderName, ResponseProvider = ResponseProvider },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                result = -1;
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }


        public bool IsValidAppVersion(string VersionCode, string Platform, ref string error, ref string versionRequired)
        {

            bool result = false;

            try
            {
                Platform = Platform.ToLower();

                con.Cnn.Open();

                var resultSP = con.Cnn.Query<RecargoAppVersions>("SP_RecarGOAppVersionValid", new { Platform = Platform },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();


                if(resultSP != null)
                {
                    decimal dbversionName = decimal.Parse(resultSP.VersionName);
                    decimal pVersionName = decimal.Parse(VersionCode);

                    if (dbversionName <= pVersionName)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                        versionRequired = dbversionName.ToString();
                    }
                }
                else
                {
                    result = false;
                }
                
            }
            catch (Exception ex)
            {
                result = false;
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public List<ComboSouvenirEN> GetCombosByConsumerIDAndAge(int consumerID, ref string error)
        {

            List<ComboSouvenirEN> result = new List<ComboSouvenirEN>();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<ComboSouvenirEN>("SPGetComboByConsumerID", new { ConsumerID = consumerID },
                    commandType: CommandType.StoredProcedure).ToList();



            }
            catch (Exception ex)
            {
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;
        }

        public WinPrizeEN ExchangeCombos(int consumerID, int ComboID, ref string error)
        {

            WinPrizeEN result = new WinPrizeEN();
            AchievementEN achievementEN = new AchievementEN();
            
            try
            {

                con.Cnn.Open();
                con.Tra = con.Cnn.BeginTransaction();

                result = con.Cnn.Query<WinPrizeEN>("SPExchangeCombo", new { ConsumerID = consumerID, ComboID = ComboID },
                    con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();

                if (result != null)
                {
                    result.tracking = con.Cnn.Query<PlayersTrackingEN>("SpGetProgressGameByConsumer", new { @ConsumerID = consumerID },
                    con.Tra, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    var TotalWinCoins = result.tracking.TotalWinCoins; 

                    
                    result.Achievement = (result.ResponseCode == "00") ? ProcessToWinAchievement(consumerID, 1, TotalWinCoins, 8, con.Tra) : null;

                    con.Tra.Commit();
                }
                

            }
            catch (Exception ex)
            {
                con.Tra.Rollback();
                error = ex.Message;
                Console.WriteLine("Error GameDAL: " + ex.Message);
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
