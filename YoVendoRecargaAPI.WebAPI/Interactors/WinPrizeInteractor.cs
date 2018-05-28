using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.Game;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class WinPrizeInteractor
    {
        public IResponse createWinPrizeResultsResponse(WinPrizeEN pWinPrize, string error)
        {
            WinPrizeResponse response = new WinPrizeResponse();

            if (pWinPrize.ResponseCode == "00")
            {
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

        public IResponse createExchangeSouvenirResponse(WinPrizeEN pWinPrize, string error)
        {
            WinPrizeResponse response = new WinPrizeResponse();

            if (pWinPrize.ResponseCode == "00")
            {
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
                response.tracking.CurrentCoinsProgress = pWinPrize.tracking.CurrentCoinsProgress;
                response.tracking.TotalWinCoins = pWinPrize.tracking.TotalWinCoins;
                response.tracking.TotalWinPrizes = pWinPrize.tracking.TotalWinPrizes;
                response.tracking.TotalSouvenirs = pWinPrize.tracking.TotalSouvenirs;
                response.tracking.AgeID = pWinPrize.tracking.AgeID;
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

        public IResponse createExchangeComboResponse(WinPrizeEN pWinPrize, string error)
        {
            WinPrizeResponse response = new WinPrizeResponse();

            if (pWinPrize.ResponseCode == "00")
            {
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
                response.tracking.CurrentCoinsProgress = pWinPrize.tracking.CurrentCoinsProgress;
                response.tracking.TotalWinCoins = pWinPrize.tracking.TotalWinCoins;
                response.tracking.TotalWinPrizes = pWinPrize.tracking.TotalWinPrizes;
                response.tracking.TotalSouvenirs = pWinPrize.tracking.TotalSouvenirs;
                response.tracking.AgeID = pWinPrize.tracking.AgeID;
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