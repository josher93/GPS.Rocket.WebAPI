using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Interactors;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class OperatorsController : ApiController
    {

        OperatorBL operatorBL = new OperatorBL();
        PersonBL personBL = new PersonBL();
        OperatorsResponse operatorResponse = new OperatorsResponse();
        GenericApiResponse response = new GenericApiResponse();
        OperatorsInteractors interactor = new OperatorsInteractors();

        #region YoVendoRecarga


        [HttpGet]
        [Route("Operators")]
        public HttpResponseMessage GetOperators(HttpRequestMessage request)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token);

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (token != null)
            {
                if (personVerified != null)
                {
                    if (personVerified.IsValidToken)
                    {
                        List<OperatorEN> theOperatorsList = operatorBL.GetOperatorList(personVerified.CountryID);

                        if (theOperatorsList != null)
                        {
                            OperatorsResponse operatorsResponse = interactor.CreateOperatorResponse(theOperatorsList);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, operatorsResponse);
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


        #endregion

        #region YoComproRecarga

        [HttpGet]
        [Route("reqTopupOperators")]
        public HttpResponseMessage GetOperatorsAndProducts(HttpRequestMessage request)
        {
            HttpResponseMessage ResponseMsg;

            try
            {
                IEnumerable<string> headerCountryID = null;
                request.Headers.TryGetValues("Country-Type", out headerCountryID);
                var headerItem = headerCountryID.ToList().Take(1).ToArray();

                if (headerItem != null)
                {
                    string id = headerItem[0];
                    int countryID = int.Parse(id);

                    var operatorsData = operatorBL.GetOperatorProducts(countryID);

                    var operatorsResponse = interactor.CreateOperatorProductsResponse(operatorsData);

                    ResponseMsg = request.CreateResponse<IResponse>(HttpStatusCode.OK, operatorsResponse);
                }
                else
                {
                    response.HttpCode = 400;
                    response.Message = "'Country-Type' header must be provided";
                    ResponseMsg = request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException)
                {
                    response.HttpCode = 400;
                    response.Message = "'Country-Type' header must be provided";
                    ResponseMsg = request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    response.HttpCode = 500;
                    response.Message = "Something went wrong";
                    ResponseMsg = request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                }
            }

            return ResponseMsg;
        
        }
        
        
        #endregion

    }
}
