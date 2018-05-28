using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.Entities;


namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class AchievementInteractor : IResponse
    {
        public List<AchievementsByConsumerResponse> listAchievementsByConsumer = new List<AchievementsByConsumerResponse>();
    
        public List<AchievementsByConsumerResponse> createAchievementsByConsumerResponse(List<AchievementEN> pAchievementsConsumer)
        {
            foreach (var item in pAchievementsConsumer)
            {
                AchievementsByConsumerResponse achievementResponse = new AchievementsByConsumerResponse();
                achievementResponse.Name = item.Name;
                achievementResponse.AchievementID = item.AchievementID;
                achievementResponse.Level = item.Level;
                achievementResponse.Score = item.Score;
                achievementResponse.Description = item.Description;
                if (item.Won >= 1)
                {
                    achievementResponse.Won = true;
                }
                else
                {
                    achievementResponse.Won = false;
                }

                switch (item.Level)
                {
                    case 0:
                        achievementResponse.NextPrize = item.PrizeFirstLevel;
                        achievementResponse.NextLevel = item.FirstLevel;
                        break;
                    case 1:
                        achievementResponse.NextPrize = item.PrizeSecondLevel;
                        achievementResponse.NextLevel = item.SecondLevel;
                        break;
                    case 2:
                        achievementResponse.NextPrize = item.PrizeThirdLevel;
                        achievementResponse.NextLevel = item.ThirdLevel;
                        break;
                    default:
                        achievementResponse.NextPrize = 0;
                        achievementResponse.NextLevel = 0;
                        break;
                }

                listAchievementsByConsumer.Add(achievementResponse);
            }

            return listAchievementsByConsumer;
        }

    }
}