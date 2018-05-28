using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class PromotionDAL
    {
        Connection connection = new Connection();

        public PromotionEN GetActivePromotion(int pCountryID)
        {
            PromotionEN promo = new PromotionEN();

            try
            {
                connection.Cnn.Close();
                promo = connection.Cnn.Query<PromotionEN>("SpGetPromotions", new { countryID = pCountryID }, 
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                promo = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetActivePromotion: " + ex.Message); 
            }
            finally
            {
                connection.Cnn.Close();
            }

            return promo;
        }
    }
}
