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
    public class SouvenirInteractor
    {

        public List<SouvenirsByConsumerResponse> createSouvenirByConsumerResponse(List<SouvenirEN> pSouvenirs)
        {
            List<SouvenirsByConsumerResponse> list = new List<SouvenirsByConsumerResponse>();
            foreach (var item in pSouvenirs)
            {
                SouvenirsByConsumerResponse souvenirByConsumerRes = new SouvenirsByConsumerResponse();
                souvenirByConsumerRes.SouvenirID = item.SouvenirID;
                souvenirByConsumerRes.Description = item.Description;
                souvenirByConsumerRes.Title = item.Title;
                souvenirByConsumerRes.AgeID = item.AgeID;
                souvenirByConsumerRes.ImgUrl = item.ImgUrl;
                souvenirByConsumerRes.Level = item.Level;
                souvenirByConsumerRes.SouvenirsOwnedByConsumer = item.FoundByConsumer;
                souvenirByConsumerRes.Unlocked = item.Unlocked;
                list.Add(souvenirByConsumerRes);
            }

            return list;
        }

        public List<CombosResponse> createComboResponse(List<ComboSouvenirEN> pCombo)
        {
            List<CombosResponse> responseList = new List<CombosResponse>();
            try
            {
                int ComboID = 0;
                foreach (var item in pCombo)
                {
                    if (item.ComboID != ComboID)
                    {
                        CombosResponse response = new CombosResponse();
                        response.ComboID = item.ComboID;
                        response.Description = item.Description;
                        response.Title = item.PrizeTitle;
                        response.PrizeDescription = item.PrizeDescription;
                        response.Souvenir = new List<CombosSouvenirResponse>();

                        var query = from c in pCombo
                                    where c.ComboID == item.ComboID
                                    select c;

                        foreach (var itemQ in query)
                        {
                            CombosSouvenirResponse Souvenir = new CombosSouvenirResponse();
                            Souvenir.ImgUrl = itemQ.ImgUrl;
                            Souvenir.Level = itemQ.Level;
                            Souvenir.Exchangeable = itemQ.FoundByConsumer;
                            Souvenir.Title = itemQ.Title;
                            response.Souvenir.Add(Souvenir);

                        }

                        responseList.Add(response);
                        ComboID = item.ComboID;
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }



            return responseList;

        }
    }
}
