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
    public class ProductDAL
    {
        Connection connection = new Connection();

        public List<ProductEN> GetProductsByCountryID(int pCountryID)
        {
            List<ProductEN> productsList = new List<ProductEN>();

            try
            {
                connection.Cnn.Open();
                productsList = connection.Cnn.Query<ProductEN>("SpGetProductsByCountryID", new { countryID = pCountryID },
                    commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ProductDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return productsList;
        }
    }
}
