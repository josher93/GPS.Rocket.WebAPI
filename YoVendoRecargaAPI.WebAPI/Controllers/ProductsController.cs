using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.WebAPI.Interactors;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class ProductsController : ApiController
    {
        PersonBL personBL = new PersonBL();
        ProductBL productsBL = new ProductBL();

        GenericApiResponse response = new GenericApiResponse();
        ProductsInteractor interactor = new ProductsInteractor();

        [HttpGet]
        [Route("products/{data}/")]
        public HttpResponseMessage Get(HttpRequestMessage pRequest, string data)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (token != null)
            {
                if (personVerified != null)
                {
                    if (personVerified.IsValidToken)
                    {
                        List<ProductEN> products = productsBL.GetProductsByCountryID(personVerified);

                        if (products != null)
                        {
                            ProductsResponse productsResponse = interactor.CreateProductsResponse(products);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, productsResponse);
                        }
                        else
                        {
                            response.HttpCode = 500;
                            response.Message = "Something went wrong";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Authentication token has expired.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "Credentials are not valid.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }
    }
}
