using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class GameStoreInteractor : IResponse
    {
        //public List<StoreEN> listStore = new List<StoreEN>();

        public List<GameStoreResponse> listGameStoreResponse = new List<GameStoreResponse>();


        public List<GameStoreResponse> createStoreItemsResponse(List<StoreEN> pStoreList)
        {
            

            foreach (var item in pStoreList)
            {
                GameStoreResponse gameStoreResponse = new GameStoreResponse();
                gameStoreResponse.StoreID = item.StoreID;
                gameStoreResponse.Name = item.Name;
                gameStoreResponse.Description = item.Description;
                gameStoreResponse.ImgUrl = item.ImgUrl;
                gameStoreResponse.Value = item.Value;
                listGameStoreResponse.Add(gameStoreResponse);
            }

            return listGameStoreResponse;
        }
    }
}