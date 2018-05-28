using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class ShopPurchaseInteractor
    {
        public IResponse createPurchaseResponse(ShopPurchaseEN pSouvenir, string error)
        {
            ShopPurchaseResponse response = new ShopPurchaseResponse();

            if (pSouvenir == null)
            {
                response.Code = "04";
                response.Message = error;
            }
            else
            {
                response.Title = pSouvenir.Title;
                response.Description = pSouvenir.Description;
                response.ImgUrl = pSouvenir.ImgUrl;
                response.Value = pSouvenir.Value;
                response.tracking = new TrackingResponse();
                response.tracking.TotalWinCoins = pSouvenir.tracking.TotalWinCoins;
                response.tracking.TotalWinPrizes = pSouvenir.tracking.TotalWinPrizes;
                response.tracking.TotalSouvenirs = pSouvenir.tracking.TotalSouvenirs;
                response.tracking.AgeID = pSouvenir.tracking.AgeID;

                if(pSouvenir.Achievement.NewLevel>0)
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