using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class ProductBL
    {
        ProductDAL productDAL = new ProductDAL();

        public List<ProductEN> GetProductsByCountryID(PersonEN pPerson)
        {
            List<ProductEN> products = new List<ProductEN>();

            try
            {
                products = productDAL.GetProductsByCountryID(pPerson.CountryID);
            }
            catch (Exception ex)
            {
                products = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return products;
        }
    }
}
