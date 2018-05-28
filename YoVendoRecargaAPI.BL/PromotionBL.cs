using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    
    public class PromotionBL
    {
        PromotionDAL promoDAL = new PromotionDAL();

        public PromotionEN GetActivePromotion(int pCountryID)
        {
            PromotionEN promo = new PromotionEN();

            try
            {
                promo = promoDAL.GetActivePromotion(pCountryID);
            }
            catch (Exception ex)
            {
                promo = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("GetActivePromotion: " + ex.Message);
            }

            return promo;
        }

    }
}
