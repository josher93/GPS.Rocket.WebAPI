using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class OperatorBL
    {
        OperatorDAL operatorDAL = new OperatorDAL();
        ProductDAL productDAL = new ProductDAL();

        public List<OperatorEN> GetOperatorList(int pCountryID)
        {
            List<OperatorEN> operatorsList = new List<OperatorEN>();

            try
            {
                operatorsList = operatorDAL.GetOperators(pCountryID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return operatorsList;
        }

        public List<OperatorEN> GetOperatorProducts(int pCountryID)
        {
            List<OperatorEN> operatorsList = new List<OperatorEN>();

            try
            {
                operatorsList = operatorDAL.GetOperatorsYCR(pCountryID);
                
                var products = productDAL.GetProductsByCountryID(pCountryID);

                foreach (var ope in operatorsList)
                {
                    ope.Products = new List<ProductEN>();

                    var operatorProducts = products.Where(p => p.OperatorID == ope.OperatorID).ToList(); ;

                    foreach (var prod in operatorProducts)
                    {
                        ope.Products.Add(prod);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("GetOperatorProducts: " + ex.Message);
            }

            return operatorsList;
        }
    }
}
