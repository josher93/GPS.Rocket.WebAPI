using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class ExchangeCoinsInteractor
    {
        public IResponse createExchangeCoinsResultsResponse(WinCoinEN pProgressGame, int coins)
        {
            ExchangeCoinsResponse response = new ExchangeCoinsResponse();

            if (pProgressGame.Achievement.Code != "00" && pProgressGame.Achievement.Code != "01")
            {
                response.Code = pProgressGame.Achievement.Code;
                response.Message = pProgressGame.Achievement.Message;
            }
            else
            {
                response.tracking = new TrackingResponse();
                response.ExchangeCoins = coins;
                response.tracking.CurrentCoinsProgress = pProgressGame.Tracking.CurrentCoinsProgress;
                response.tracking.TotalWinCoins = pProgressGame.Tracking.TotalWinCoins;
                response.tracking.TotalWinPrizes = pProgressGame.Tracking.TotalWinPrizes;
                response.tracking.TotalSouvenirs = pProgressGame.Tracking.TotalSouvenirs;
                response.tracking.AgeID = pProgressGame.Tracking.AgeID;

                if (pProgressGame.Achievement != null)
                {
                    if (pProgressGame.Achievement.NewLevel > 0)
                    {
                        response.Achievement = new NewAchievement();
                        response.Achievement.Name = pProgressGame.Achievement.Name;
                        response.Achievement.Level = pProgressGame.Achievement.Level;
                        response.Achievement.Score = pProgressGame.Achievement.Score;
                        response.Achievement.ValueNextLevel = pProgressGame.Achievement.ValueNextLevel;
                        response.Achievement.Prize = pProgressGame.Achievement.Prize;
                    }
                }
                response.Code = "00";
                
                response.Type = 1;
            }

            return response;
        }

        public IResponse createExchangeSouvewnorResultsResponse(SouvenirEN pSouvenir, string error)
        {
            SouvenirResponse response = new SouvenirResponse();

            if (pSouvenir.Code != "00" && pSouvenir.Achievement.Code != "00")
            {
                response.Code = (pSouvenir.Code != null) ? pSouvenir.Code : pSouvenir.Achievement.Code;
                response.Message = error;
            }
            else
            {
                response.Type = 2;
                response.Title = pSouvenir.Title;
                response.Description = pSouvenir.Description;
                response.ImgUrl = pSouvenir.ImgUrl;
                response.Value = pSouvenir.Value;
                response.tracking = new TrackingResponse();
                response.tracking.TotalWinCoins = pSouvenir.tracking.TotalWinCoins;
                response.tracking.TotalWinPrizes = pSouvenir.tracking.TotalWinPrizes;
                response.tracking.TotalSouvenirs = pSouvenir.tracking.TotalSouvenirs;
                response.tracking.CurrentCoinsProgress = pSouvenir.tracking.CurrentCoinsProgress;
                response.tracking.AgeID = pSouvenir.tracking.AgeID;


                if (pSouvenir.Achievement.NewLevel > 0)
                {
                    response.Achievement = new NewAchievement();
                    response.Achievement.Name = pSouvenir.Achievement.Name;
                    response.Achievement.Level = pSouvenir.Achievement.Level;
                    response.Achievement.Score = pSouvenir.Achievement.Score;
                    response.Achievement.ValueNextLevel = pSouvenir.Achievement.ValueNextLevel;
                    response.Achievement.Prize = pSouvenir.Achievement.Prize;
                }

                response.Code = pSouvenir.Code;
                response.Message = (error == "") ? pSouvenir.Message : error;
            }

            return response;
        }
    }
}