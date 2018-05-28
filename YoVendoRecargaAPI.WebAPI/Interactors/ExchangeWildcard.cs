using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class ExchangeWildcardInteractor
    {
        public IResponse createExchangeCoinsResultsResponse(WinCoinEN pProgressGame, int coins, int type)
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
                response.Code = pProgressGame.Achievement.Code;
                response.Type = type;
            }

            return response;
        }

        public IResponse createWinPrizeResultsResponse(WinPrizeEN pWinPrize, string error)
        {
            WinPrizeWildcardResponse response = new WinPrizeWildcardResponse();

            if (pWinPrize.ResponseCode == "00")
            {
                response.Type = 3;
                response.ResponseCode = pWinPrize.ResponseCode;
                response.Message = pWinPrize.Message;
                response.Code = pWinPrize.Code;
                response.Title = pWinPrize.Title;
                response.Dial = pWinPrize.Dial;
                response.RGBColor = pWinPrize.RGBColor;
                response.HexColor = pWinPrize.HexColor;
                response.logoUrl = pWinPrize.logoUrl;
                response.Description = pWinPrize.Description;

                response.tracking = new TrackingResponse();
                response.tracking.CurrentCoinsProgress = pWinPrize.CurrentCoinsProgress;
                response.tracking.TotalWinCoins = pWinPrize.TotalWinCoins;
                response.tracking.TotalWinPrizes = pWinPrize.TotalWinPrizes;
                response.tracking.TotalSouvenirs = pWinPrize.TotalSouvenirs;
                response.tracking.AgeID = pWinPrize.AgeID;
                response.PrizeLevel = pWinPrize.PrizeLevel;

                if (pWinPrize.Achievement.NewLevel > 0)
                {
                    response.Achievement = new NewAchievement();
                    response.Achievement.Name = pWinPrize.Achievement.Name;
                    response.Achievement.Level = pWinPrize.Achievement.Level;
                    response.Achievement.Score = pWinPrize.Achievement.Score;
                    response.Achievement.ValueNextLevel = pWinPrize.Achievement.ValueNextLevel;
                    response.Achievement.Prize = pWinPrize.Achievement.Prize;
                }

            }
            else
            {
                if (pWinPrize.ResponseCode == "02")
                {
                    response.ResponseCode = pWinPrize.ResponseCode;
                    response.Message = pWinPrize.Message;
                    response.WaitTime = pWinPrize.WaitTime;
                }
                else
                {
                    response.ResponseCode = pWinPrize.ResponseCode;
                    response.Message = pWinPrize.Message;
                }

            }

            return response;
        }
    }
}