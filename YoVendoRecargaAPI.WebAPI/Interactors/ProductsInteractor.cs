using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class ProductsInteractor
    {
        ProductBL productBL = new ProductBL();

        public ProductsResponse CreateProductsResponse(List<ProductEN> pProducts)
        {
            ProductsResponse response = new ProductsResponse();
            response.products = new List<TopupProducts>();

            try
            {
                var operatorProducts = pProducts.GroupBy(pr => pr.Brand)
                        .Select(grp => grp.FirstOrDefault()).ToList().OrderBy(op => op.OperatorRelevance);

                foreach (var operatr in operatorProducts)
                {
                    TopupProducts topupProduct = new TopupProducts();
                    topupProduct.denomination = new List<Product>();
                    topupProduct.mno = operatr.Brand;

                    var products = pProducts.Where(br => String.Equals(br.Brand, operatr.Brand)).ToList();

                    foreach (var prod in products)
                    {
                        Product product = new Product();
                        product.Amount = Convert.ToString(prod.Amount);
                        product.Code = prod.Code;
                        product.Description = prod.Description;
                        product.PackageCode = prod.PackageCode;
                        product.Relevance = prod.Relevance;
                        topupProduct.denomination.Add(product);
                    }

                    response.products.Add(topupProduct);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return response;
        }
    }
}