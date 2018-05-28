using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Game.DAL;
using System.Configuration;

namespace YoVendoRecargaAPI.Game
{
    public class GameBL
    {
        GameDAL gameDAL = new GameDAL();
        ExchangeCoinsDAL exchangeDAL = new ExchangeCoinsDAL();

        public AllLeaderBoards GetLeaderBoards()
        {
            var result = gameDAL.GetLeaderBoards();
            return result;
        }

        public LastWinnerLeaderBoards GetLastWinner()
        {
            var result = gameDAL.GetLastWinner();
            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsToday()
        {
            var result = gameDAL.GetLeaderBoardsToday();
            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsWeek()
        {

            var result = gameDAL.GetLeaderBoardsWeek();
            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsMonth()
        {
            var result = gameDAL.GetLeaderBoardsMonth();
            return result;
        }

        public List<LeaderBoards> GetLeaderBoardsOverAll()
        {
            var result = gameDAL.GetLeaderBoardsOverAll();
            return result;
        }

        public LeaderBoards GetLastWinnerOnYesterday()
        {
            var result = gameDAL.GetLastWinnerOnYesterday();
            return result;
        }

        public LeaderBoards GetLastWinnerOnLastWeek()
        {
            var result = gameDAL.GetLastWinnerOnLastWeek();
            return result;
        }

        public LeaderBoards GetLastWinnerOnLastMonth()
        {
            var result = gameDAL.GetLastWinnerOnLastMonth();
            return result;
        }

        public PlayersTrackingEN GetProgressGame(int consumerID, ref string error)
        {
            var result = exchangeDAL.GetProgressGame(consumerID, ref error);
            return result;
        }

        public List<PrizeHistoryEN> GetPrizeHistoryByConsumerID(int ConsumerID, ref string error)
        {
            var result = gameDAL.GetPrizeHistoryByConsumerID(ConsumerID, ref error);
            return result;
        }

        public WinPrizeEN ProcessToValidateAndWinPrize(int ConsumerID, int CountryID, ref string error)
        {
            WinPrizeEN wPrize = new WinPrizeEN();
            wPrize.RegDate = DateTime.Now;
            try
            {
                var getLastWinPrize = gameDAL.GetLastWinPrize(ConsumerID, ref error);

                if (error == "")
                {
                    int tcPrize = int.Parse(ConfigurationManager.AppSettings["TimeToChangePrize"].ToString());

                    TimeSpan difference = (getLastWinPrize != null && error == "") ? wPrize.RegDate - getLastWinPrize.RegDate : new TimeSpan(tcPrize, 0, 0);

                    if (difference.Days > 0 || difference.Hours >= tcPrize)
                    {
                        wPrize = gameDAL.ProcessToWinPrize(ConsumerID, CountryID, ref error);
                    }
                    else
                    {
                        string timer = Convert.ToString(tcPrize - difference.Hours);
                        wPrize.ResponseCode = "02";
                        wPrize.Message = "It is not possible to change the prize, you have to wait " + timer + " hours";
                        wPrize.WaitTime = timer;
                    }
                }
            }
            catch (Exception ex)
            {
                wPrize.Code = "500";
                error = ex.Message;
            }

            return wPrize;
        }

        public ShopPurchaseEN PurchaseAndGetSouvenir(int consumerID, int StoreId, ref string error)
        {
            int resultRandomB1 = 0;
            int resultRandomB2 = 0;
            int LevelB1 = 0;
            int LevelB2 = 0;

            Random random = new Random();
            GetLevelBarrel1(ref resultRandomB1, ref LevelB1, random);

            GetLevelBarrel2(ref resultRandomB2, ref LevelB2, random);


            var result = gameDAL.PurchaseAndGetSouvenir(consumerID, StoreId, ref error, LevelB1, LevelB2);
            return result;
        }

        private static void GetLevelBarrel2(ref int resultRandomB2, ref int LevelB2, Random random)
        {
            var rBarrel2 = ConfigurationManager.AppSettings["RandomBarrel2"].ToString();
            var rangeBarrel2 = rBarrel2.Split(',');
            int minValueB2 = int.Parse(rangeBarrel2[0]);
            int maxValueB2 = int.Parse(rangeBarrel2[1]);
            resultRandomB2 = random.Next(minValueB2, maxValueB2);

            if (resultRandomB2 >= 1 && resultRandomB2 <= 49)
                LevelB2 = 2;
            else if (resultRandomB2 == 50)
                LevelB2 = 3;
            else
                LevelB2 = 2;
        }

        private static void GetLevelBarrel1(ref int resultRandomB1, ref int LevelB1, Random random)
        {
            var rBarrel1 = ConfigurationManager.AppSettings["RandomBarrel1"].ToString();


            var rangeBarrel1 = rBarrel1.Split(',');
            int minValueB1 = int.Parse(rangeBarrel1[0]);
            int maxValueB1 = int.Parse(rangeBarrel1[1]);
            resultRandomB1 = random.Next(minValueB1, maxValueB1);

            if (resultRandomB1 <= 9)
                LevelB1 = 1;
            else if (resultRandomB1 == 10)
                LevelB1 = 2;
            else
                LevelB1 = 1;
        }

        public WinPrizeEN ProcessToExchangeSouvenirByPrize(int consumerID, int SouvenirID, int CountryID, ref string error)
        {
            WinPrizeEN wPrize = new WinPrizeEN();
            wPrize.RegDate = DateTime.Now;

            var getLastWinPrize = gameDAL.GetLastWinPrize(consumerID, ref error);

            if (error == "")
            {
                int tcPrize = int.Parse(ConfigurationManager.AppSettings["TimeToChangePrize"].ToString());

                TimeSpan difference = (getLastWinPrize != null && error == "") ? wPrize.RegDate - getLastWinPrize.RegDate : new TimeSpan(tcPrize, 0, 0);

                if (difference.Days > 0 || difference.Hours >= tcPrize)
                {
                    wPrize = gameDAL.ProcessToExchangeSouvenirByPrize(consumerID, SouvenirID, CountryID, ref error);
                }
                else
                {
                    string timer = Convert.ToString(tcPrize - difference.Hours);
                    wPrize.ResponseCode = "02";
                    wPrize.Message = "It is not possible to change the prize, you have to wait " + timer + " hour";
                    wPrize.WaitTime = timer;
                }
            }

            return wPrize;
        }

        public WinPrizeEN ProcessToValidateAndWinPrizeWildCard(int ConsumerID, string LocationID, int AgeID, ref string error)
        {
            WinPrizeEN wPrize = new WinPrizeEN();
            wPrize.RegDate = DateTime.Now;
            try
            {
                var getLastWinPrize = gameDAL.GetLastWinPrize(ConsumerID, ref error);

                if (error == "")
                {
                    int tcPrize = int.Parse(ConfigurationManager.AppSettings["TimeToChangePrize"].ToString());

                    TimeSpan difference = (getLastWinPrize != null && error == "") ? wPrize.RegDate - getLastWinPrize.RegDate : new TimeSpan(24, 0, 0);

                    if (difference.Days > 1 || difference.Hours > 0)
                    {
                        var process = gameDAL.ProcessToWinPrizeWildcard(ConsumerID,LocationID, AgeID, ref error);

                        if (process.ResponseCode == "00")
                        {
                            wPrize.ResponseCode = process.ResponseCode;
                            wPrize.Message = process.Message;
                            wPrize.Code = process.Code;
                            wPrize.Title = process.Title;
                            wPrize.Dial = process.Dial;
                            wPrize.RGBColor = process.RGBColor;
                            wPrize.HexColor = process.HexColor;
                            wPrize.logoUrl = process.logoUrl;
                            wPrize.Description = process.Description;

                            wPrize.CurrentCoinsProgress = process.tracking.CurrentCoinsProgress;
                            wPrize.TotalWinCoins = process.tracking.TotalWinCoins;
                            wPrize.TotalWinPrizes = process.tracking.TotalWinPrizes;
                            wPrize.TotalSouvenirs = process.tracking.TotalSouvenirs;
                            wPrize.AgeID = process.tracking.AgeID;
                        }
                        else
                        {
                            wPrize.ResponseCode = process.ResponseCode;
                            wPrize.Message = process.Message;
                        }
                    }
                    else
                    {
                        string timer = Convert.ToString(tcPrize - difference.Hours);
                        wPrize.ResponseCode = "02";
                        wPrize.Message = "It is not possible to change the prize, you have to wait " + timer + " hours";
                        wPrize.WaitTime = timer;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return wPrize;
        }


        public int InsertActivatePIN(int consumerID, string Phone, string PIN, string ProviderName, string ResponseProvider, ref string error)
        {
            var result = gameDAL.InsertActivatePIN(consumerID, Phone, PIN, ProviderName, ResponseProvider, ref error);
            return result;
        }
        public bool IsValidAppVersion(string VersionCode, string Platform, ref string error, ref string versionRequired)
        {
            var result = gameDAL.IsValidAppVersion(VersionCode, Platform, ref error, ref versionRequired);
            return result;
        }

        public List<ComboSouvenirEN> GetCombosByConsumerIDAndAge(int consumerID, ref string error) 
        {
            var result = gameDAL.GetCombosByConsumerIDAndAge(consumerID, ref error);
            return result;
        }

        public WinPrizeEN ExchangeCombos(int consumerID, int ComboID, ref string error)
        {
            WinPrizeEN wPrize = new WinPrizeEN();
            wPrize.RegDate = DateTime.Now;

            var getLastWinPrize = gameDAL.GetLastWinPrize(consumerID, ref error);

            if (error == "")
            {
                int tcPrize = int.Parse(ConfigurationManager.AppSettings["TimeToChangePrize"].ToString());

                TimeSpan difference = (getLastWinPrize != null && error == "") ? wPrize.RegDate - getLastWinPrize.RegDate : new TimeSpan(tcPrize, 0, 0);

                if (difference.Days > 0 || difference.Hours >= tcPrize)
                {
                    wPrize = gameDAL.ExchangeCombos(consumerID, ComboID, ref error);
                }
                else
                {
                    string timer = Convert.ToString(tcPrize - difference.Hours);
                    wPrize.ResponseCode = "02";
                    wPrize.Message = "It is not possible to change the prize, you have to wait " + timer + " hour";
                    wPrize.WaitTime = timer;
                }
            }
            return wPrize;
        }
    }
}
