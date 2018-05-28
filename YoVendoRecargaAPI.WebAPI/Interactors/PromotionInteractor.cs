using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class PromotionInteractor
    {
        public PromotionsResponse CreatePromosResponse(PromotionEN pPromo)
        {
            PromotionsResponse response = new PromotionsResponse();

            try
            {
                response.description = pPromo.Description;
                response.Method = pPromo.HttpMethod;
                response.Operator = pPromo.OperatorBrand;
                response.Tittle = pPromo.Title;
                response.URL = pPromo.URL;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
    }
}