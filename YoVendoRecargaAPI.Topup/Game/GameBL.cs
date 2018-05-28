using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Topup.Game;

namespace YoVendoRecargaAPI.Topup
{
    public class GameBL
    {
        GameDAL gameDAL = new GameDAL();

        public bool AddCoinByTopup(int pConsumerID)
        {
            bool addedCoin = false;

            try
            {
                PlayersTrackingEN tracking = new PlayersTrackingEN();

                int coins = int.Parse(ConfigurationManager.AppSettings["CoinsByTopupSuccess"].ToString());

                var currentTracking = gameDAL.GetProgressGame(pConsumerID);

                if (currentTracking != null)
                {
                    tracking = currentTracking;
                    tracking.CurrentCoinsProgress = currentTracking.CurrentCoinsProgress + coins;
                    tracking.CurrentCoinsProgress = (tracking.CurrentCoinsProgress >= 20) ? 20 : tracking.CurrentCoinsProgress;
                    tracking.ModDate = DateTime.Now;
                    tracking.ConsumerID = pConsumerID;
                    tracking.PlayersTrackingID = currentTracking.PlayersTrackingID;

                    tracking.TotalWinCoins = currentTracking.TotalWinCoins + coins;

                    int UpdateResult = gameDAL.UpdatePlayerTracking(tracking);

                    //Updates current progress
                    if (UpdateResult > 0)
                    {
                        WinCoinEN winCoin = new WinCoinEN();

                        winCoin.ConsumerID = pConsumerID;
                        winCoin.ExchangeCoins = coins;
                        winCoin.RegDate = DateTime.Now;
                        winCoin.Longitude = 0;
                        winCoin.Latitude = 0;
                        winCoin.LocationID = "0";
                        winCoin.ChestType = 1;

                        var ResultInsert = gameDAL.InsertWinCoin(winCoin);

                        if (ResultInsert > 0)
                        {
                            addedCoin = true;
                        }
                    }
                }
                else
                {
                    tracking.CurrentCoinsProgress = coins;
                    tracking.CurrentCoinsProgress = (tracking.CurrentCoinsProgress >= 20) ? 20 : coins;
                    tracking.TotalWinCoins = coins;
                    tracking.TotalWinPrizes = 0;

                    int ResltInsert = gameDAL.InsertPlayerTracking(tracking.TotalWinCoins, tracking.CurrentCoinsProgress, pConsumerID, DateTime.Now);

                    //Inserts current progress
                    if (ResltInsert > 0)
                    {
                        WinCoinEN winCoin = new WinCoinEN();

                        winCoin.ConsumerID = pConsumerID;
                        winCoin.ExchangeCoins = coins;
                        winCoin.RegDate = DateTime.Now;
                        winCoin.Longitude = 0;
                        winCoin.Latitude = 0;
                        winCoin.LocationID = "0";
                        winCoin.ChestType = 0;

                        int InsertWinCoin = gameDAL.InsertWinCoin(winCoin);


                        if (InsertWinCoin > 0)
                        {
                            addedCoin = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("" + ex.Message);
            }

            return addedCoin;
        }
    }
}
