using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.Topup;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Interactors;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class TopupController : ApiController
    {
        GenericApiResponse response = new GenericApiResponse();
        TopupBL topupBL = new TopupBL();
        PersonBL personBL = new PersonBL();
        ConsumerBL consumerBL = new ConsumerBL();

        #region YoVendoRecarga

        [HttpPost]
        [Route("topup/{data}/{amount:decimal}/{code}")]
        public HttpResponseMessage Topup(HttpRequestMessage request, [FromBody] TopupRequest pOperatorData, string data, decimal amount, int code)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token);

            if (token == null)
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
            else
            {
                if (!String.IsNullOrEmpty(pOperatorData.Operador) && pOperatorData.IdCountry <= 0)
                {
                    response.HttpCode = 400;
                    response.Message = "Operator name and ID must be provided";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
                else if (String.IsNullOrEmpty(data))
                {
                    response.HttpCode = 400;
                    response.Message = "Malformed URL: Phone number must be provided";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
                else if (amount == 0)
                {
                    response.HttpCode = 400;
                    response.Message = "Malformed URL: Amount must be provided";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
                else
                {
                    PersonEN person = personBL.VerifyPersonAuthentication(token);

                    if (person != null)
                    {
                        TopupInteractor interactor = new TopupInteractor();
                        TopupTransactionEN result = topupBL.SendTopup(person, pOperatorData.Operador, amount, data, code);
                        var bags = topupBL.GetUserBags(person.PersonID);

                        var responseTopup = interactor.CreateTopupResponse(result, bags);


                        if (String.Equals(result.Message, Values.Ok))
                        {
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseTopup);
                        }
                        else if (String.Equals(result.Message, Values.Success))
                        {
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseTopup);
                        }
                        else if (String.Equals(result.Message, Values.NoCreditLeft))
                        {
                            return Request.CreateResponse<IResponse>(HttpStatusCode.ServiceUnavailable, responseTopup);
                        }
                        else if (String.Equals(result.Message, Values.InvalidProduct))
                        {
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, responseTopup);
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
                        response.HttpCode = 500;
                        response.Message = "Something went wrong";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                    }

                }
            }
        }

        #endregion

        #region YoComproRecarga

        [HttpGet]
        [Route("GetPendingTopUps")]
        public HttpResponseMessage GetPendingTopUps(HttpRequestMessage request)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token);

            if (token == null)
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
            else
            {
                PersonEN person = personBL.VerifyPersonAuthentication(token);

                if (person != null)
                {
                    if (person.IsValidToken)
                    {
                        TopupInteractor interactor = new TopupInteractor();
                        var topupRequests = topupBL.GetTopupRequestsByVendor(person.VendorCode);
                        var requestsApiResponse = interactor.CreatePendingTopupsResponse(topupRequests);
                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, requestsApiResponse);
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Expired token or invalid credentials.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.HttpCode = 500;
                    response.Message = "Something went wrong";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                }
            }
        }


        [HttpPost]
        [Route("requestTopup")]
        public async Task<HttpResponseMessage> requestTopup(HttpRequestMessage pRequest, [FromBody] TopupRequestReq data)
        {
            IEnumerable<string> authKey = null;
            pRequest.Headers.TryGetValues("authenticationKey", out authKey);

            if (authKey != null)
            {
                var consumerVerified = consumerBL.AuthenticateConsumer(authKey.FirstOrDefault());

                if (consumerVerified != null)
                {
                    if (consumerVerified.IsValidKey)
                    {
                        TopupInteractor interactor = new TopupInteractor();
                        var validation = interactor.ValidateTopupRequest(data);

                        if (!validation.result)
                        {
                            response.HttpCode = 400;
                            response.Message = validation.Message;
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            bool result = await topupBL.RequestTopup(consumerVerified.ConsumerID,  consumerVerified.Nickname, data.targetPhoneNumber, data.vendorCode, data.amount, data.operatorID, data.CategoryID);

                            SimpleTextResponse textResponse = new SimpleTextResponse();
                            textResponse.result = result;

                            textResponse.Message = (result) ? "Succes" : "Request was made sucesfully, but notification failed";

                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, textResponse);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Authentication key is not valid";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "Invalid authentication key";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 401;
                response.Message = "Authentication key is required";
                return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
            }
        }

        [HttpPost]
        [Route("ResponseToPendingRequest")]
        public HttpResponseMessage TopupRequestResponse(HttpRequestMessage pRequest, [FromBody] PendingTopupReq data)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token);

            if (token != null)
            {
                PersonEN person = personBL.VerifyPersonAuthentication(token);

                if (person != null)
                {
                    if (person.IsValidToken)
                    {
                        if (data.PendingRequestID > 0)
                        {
                            SimpleTextResponse answer = new SimpleTextResponse();

                            if (topupBL.AnswerTopupRequest(person, data.PendingRequestID, data.ResponseToRequest))
                            {
                                answer.result = true;
                                answer.Message = "Operation completed succesfully";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, answer);
                            }
                            else
                            {
                                answer.result = false;
                                answer.Message = "Something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, answer);
                            }
                        }
                        else
                        {
                            response.HttpCode = 400;
                            response.Message = "Topup Request ID must be greater than 0";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Invalid token";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
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
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        #endregion

    }
}
