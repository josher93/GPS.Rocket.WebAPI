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
    public class ExchangeCoinsBL
    {
        ExchangeCoinsDAL exchangeCoinsDAL = new ExchangeCoinsDAL();

        public WinCoinEN ProcessExchangeCoins(string LocationID, decimal Longitude, decimal Latitude, int ChestType, int ConsumerID, ref int coins, int AgeID)
        {
            int CurrentCoinsProgress = 0;
            DateTime RegDate = DateTime.Now;
            Random random = new Random();
            WinCoinEN response = new WinCoinEN();
            string error = "";
            try
            {

                var currentProgress = exchangeCoinsDAL.GetProgressGameIsNotExistTracking(ConsumerID, LocationID, RegDate, ref error);

                if (currentProgress != null && error == "")
                {
                    string ChestkeyValue = ConfigurationManager.AppSettings[ChestType.ToString()].ToString();

                    var range = ChestkeyValue.Split(',');
                    int minValue = int.Parse(range[0]);
                    int maxValue = int.Parse(range[1]);
                    coins = random.Next(minValue, maxValue);

                    CurrentCoinsProgress = (currentProgress != null) ? currentProgress.CurrentCoinsProgress + coins : coins;

                    int limitCoins = int.Parse(ConfigurationManager.AppSettings["limitCoins"].ToString());

                    CurrentCoinsProgress = (CurrentCoinsProgress >= limitCoins) ? limitCoins : CurrentCoinsProgress;

                    int insert = (currentProgress.TotalWinCoins == 0) ? 1 : 0;

                    int TotalWinCoins = (currentProgress != null) ? currentProgress.TotalWinCoins + coins : coins;
                    int TotalWinPrizes = (currentProgress != null) ? currentProgress.TotalWinPrizes : 0;


                    var resultSaveData = exchangeCoinsDAL.SaveData(ConsumerID, LocationID, Longitude, Latitude, RegDate, ChestType, coins, TotalWinCoins, CurrentCoinsProgress, TotalWinPrizes, ref error, currentProgress.AgeID);

                    if (error == "" && (resultSaveData.Code == "00" || resultSaveData.Code == "01"))
                    {
                        //success
                        response.Tracking = new PlayersTrackingEN();
                        response.Tracking.ExchangeCoins = coins;
                        response.Tracking.CurrentCoinsProgress = resultSaveData.Tracking.CurrentCoinsProgress;
                        response.Tracking.TotalWinCoins = resultSaveData.Tracking.TotalWinCoins;
                        response.Tracking.TotalWinPrizes = resultSaveData.Tracking.TotalWinPrizes;
                        response.Tracking.TotalSouvenirs = resultSaveData.Tracking.TotalSouvenirs;
                        response.Tracking.AgeID = resultSaveData.Tracking.AgeID;
                        response.Achievement = new AchievementEN();
                        if (resultSaveData.Code == "00")
                        {

                            response.Achievement.Name = resultSaveData.Name;
                            response.Achievement.FirstLevel = resultSaveData.FirstLevel;
                            response.Achievement.SecondLevel = resultSaveData.SecondLevel;
                            response.Achievement.ThirdLevel = resultSaveData.ThirdLevel;
                            response.Achievement.Level = resultSaveData.Level;
                            response.Achievement.NewLevel = resultSaveData.NewLevel;
                            response.Achievement.Score = resultSaveData.Score;
                            response.Achievement.ValueNextLevel = resultSaveData.ValueNextLevel;
                            response.Achievement.Prize = resultSaveData.Prize;
                        }

                        response.Achievement.Code = "00";
                    }
                    else
                    {
                        response.Achievement = new AchievementEN();
                        response.Achievement.Code = resultSaveData.Code;
                        response.Achievement.Message = resultSaveData.Message + " consumerID: " + ConsumerID.ToString();

                    }

                }
                else
                {
                    response.Achievement = new AchievementEN();
                    response.Achievement.Code = "05";
                    response.Achievement.Message = "Solo lo puedes canjear una vez al dia";
                }

            }
            catch (Exception ex)
            {
                response.Achievement = new AchievementEN();
                response.Achievement.Code = "06";
                response.Achievement.Message = ex.Message;
                error = ex.Message;
            }

            return response;
        }

        public SouvenirEN ProcessExchangeSouvenir(string LocationID, decimal Longitude, decimal Latitude, int ChestType, int ConsumerID, ref string error, int AgeID)
        {
            DateTime RegDate = DateTime.Now;
            Random random = new Random();
            SouvenirEN Souvenir = new SouvenirEN();
            int Level = 0;
            int souvRandomResult = 0;

            try
            {

                var currentProgress = exchangeCoinsDAL.GetProgressGameIsNotExistTracking(ConsumerID, LocationID, RegDate, ref error);

                if (currentProgress != null)
                {
                    string RandomSouvenirValue = ConfigurationManager.AppSettings["RandomSouvenir"].ToString();

                    var range = RandomSouvenirValue.Split(',');
                    int minValue = int.Parse(range[0]);
                    int maxValue = int.Parse(range[1]);
                    souvRandomResult = random.Next(minValue, maxValue);

                    if (souvRandomResult <= 30)
                        Level = 1;
                    else if (souvRandomResult > 31 && souvRandomResult <= 49)
                        Level = 2;
                    else
                        Level = 3;


                    var resultSaveData = exchangeCoinsDAL.SpSaveSouvenirAndProgressGame(ConsumerID, LocationID, Longitude, Latitude, RegDate, ChestType, ref error, AgeID, Level, currentProgress.TotalWinCoins);


                    return resultSaveData;

                }
                else
                {
                    Souvenir.Code = "01";
                    error = "You can get one coin every day";
                    return Souvenir;
                }

            }
            catch (Exception ex)
            {
                Souvenir.Code = "02";
                error = ex.Message;
                return Souvenir;
            }


        }

        public WinCoinEN AddOrDiscountCoinsWildCard(string LocationID, decimal Longitude, decimal Latitude, int ConsumerID, ref int coins, int AgeID, int type)
        {
            int CurrentCoinsProgress = 0;
            DateTime RegDate = DateTime.Now;
            Random random = new Random();
            WinCoinEN response = new WinCoinEN();
            string error = "";
            try
            {

                var currentProgress = exchangeCoinsDAL.GetProgressGameIsNotExistTracking(ConsumerID, LocationID, RegDate, ref error);

                if (currentProgress != null && error == "")
                {
                    string ChestkeyValue = ConfigurationManager.AppSettings["CoinsWildCard"].ToString();

                    var range = ChestkeyValue.Split(',');
                    int minValue = int.Parse(range[0]);
                    int maxValue = int.Parse(range[1]);
                    coins = random.Next(minValue, maxValue);

                    int negativo = -1;
                    int limitCoins = int.Parse(ConfigurationManager.AppSettings["limitCoins"].ToString());
                    int TotalWinCoins = 0;
                    int TotalWinPrizes = 0;

                    if (type == 1)
                    {
                        CurrentCoinsProgress = 0;

                        TotalWinCoins = (currentProgress != null) ? currentProgress.TotalWinCoins - coins : 0;
                        TotalWinPrizes = (currentProgress != null) ? currentProgress.TotalWinPrizes : 0;

                        coins = coins * negativo;
                    }
                    else if (type == 2)
                    {
                        CurrentCoinsProgress = (currentProgress != null) ? currentProgress.CurrentCoinsProgress + coins : coins;

                        CurrentCoinsProgress = (CurrentCoinsProgress >= limitCoins) ? limitCoins : CurrentCoinsProgress;

                        TotalWinCoins = (currentProgress != null) ? currentProgress.TotalWinCoins + coins : coins;
                        TotalWinPrizes = (currentProgress != null) ? currentProgress.TotalWinPrizes : 0;
                    }
                    else  //Prize not found
                    {
                        coins = 100;
                        CurrentCoinsProgress = (currentProgress != null) ? currentProgress.CurrentCoinsProgress + coins : coins;

                        CurrentCoinsProgress = (CurrentCoinsProgress >= limitCoins) ? limitCoins : CurrentCoinsProgress;

                        TotalWinCoins = (currentProgress != null) ? currentProgress.TotalWinCoins + coins : coins;
                        TotalWinPrizes = (currentProgress != null) ? currentProgress.TotalWinPrizes : 0;
                    }


                    var resultSaveData = exchangeCoinsDAL.SaveCoinsWildCard(ConsumerID, LocationID, Longitude, Latitude, RegDate, coins, TotalWinCoins, CurrentCoinsProgress, TotalWinPrizes, ref error, AgeID);

                    if (error == "" && (resultSaveData.Code == "00" || resultSaveData.Code == "01"))
                    {
                        //success
                        response.Tracking = new PlayersTrackingEN();
                        response.Tracking.ExchangeCoins = coins;
                        response.Tracking.CurrentCoinsProgress = resultSaveData.Tracking.CurrentCoinsProgress;
                        response.Tracking.TotalWinCoins = resultSaveData.Tracking.TotalWinCoins;
                        response.Tracking.TotalWinPrizes = resultSaveData.Tracking.TotalWinPrizes;
                        response.Tracking.TotalSouvenirs = resultSaveData.Tracking.TotalSouvenirs;
                        response.Tracking.AgeID = resultSaveData.Tracking.AgeID;

                        response.Achievement = new AchievementEN();
                        response.Achievement.Code = "00";

                    }
                    else
                    {
                        response.Achievement = new AchievementEN();
                        response.Achievement.Code = resultSaveData.Code;
                        response.Achievement.Message = resultSaveData.Message + " consumerID: " + ConsumerID.ToString();

                    }

                }
                else
                {
                    response.Achievement = new AchievementEN();
                    response.Achievement.Code = "05";
                    response.Achievement.Message = "Solo lo puedes canjear una vez al dia";
                }

            }
            catch (Exception ex)
            {
                response.Achievement = new AchievementEN();
                response.Achievement.Code = "06";
                response.Achievement.Message = ex.Message;
                error = ex.Message;
            }

            return response;
        }
    }
}
