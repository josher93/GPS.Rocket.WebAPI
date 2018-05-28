using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.Game;
using System.Configuration;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class TokenController : ApiController
    {
        ConsumerBL consumerBL = new ConsumerBL();
        ConsumerTokenBL tokenBL = new ConsumerTokenBL();
        GenericApiResponse response = new GenericApiResponse();
        GameBL gameBL = new GameBL();


        [HttpPost]
        [Route("registerPhoneConsumer")]
        public HttpResponseMessage RegisterConsumer(HttpRequestMessage pRequest, [FromBody] RegisterPhoneReqBody data)
        {
            IEnumerable<string> AppVersion = null;
            pRequest.Headers.TryGetValues("AppVersion", out AppVersion);

            IEnumerable<string> Platform = null;
            pRequest.Headers.TryGetValues("Platform", out Platform);

            bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

            if (ApplyValidation == false || (AppVersion != null && Platform != null))
            {
                string versionRequired = "";
                string error = "";

                var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                if (isValidVersion)
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
                                if (String.IsNullOrEmpty(data.deviceID) || String.IsNullOrEmpty(data.phone) || String.IsNullOrEmpty(data.countryID))
                                {
                                    response.HttpCode = 400;
                                    response.Message = "Values for 'phone', 'countryID' and 'deviceID' are required.";
                                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                                }
                                else
                                {
                                    PhoneConsumerResponse phoneResponse = new PhoneConsumerResponse();

                                    var RegisterPhoneResult = tokenBL.RegisterPhoneConsumer(consumerVerified, data.phone, data.deviceID, Convert.ToInt32(data.countryID));
                                    if (RegisterPhoneResult.registeredAndSent != false)
                                    {
                                        phoneResponse.result = true;
                                        phoneResponse.consumerID = consumerVerified.ConsumerID;
                                        phoneResponse.message = "Operation succeeded";
                                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, phoneResponse);
                                    }
                                    else if (RegisterPhoneResult.TimeRemaining != null)
                                    {
                                        phoneResponse.result = false;
                                        phoneResponse.consumerID = consumerVerified.ConsumerID;
                                        phoneResponse.message = "Last confirmation message was sended in less than 1.5 minutes ago";
                                        phoneResponse.SecondsRemaining = RegisterPhoneResult.TimeRemaining;
                                        return Request.CreateResponse<IResponse>(HttpStatusCode.Created, phoneResponse);
                                    }
                                    else
                                    {
                                        phoneResponse.result = false;
                                        phoneResponse.consumerID = consumerVerified.ConsumerID;
                                        phoneResponse.message = "Something went wrong";
                                        return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, phoneResponse);
                                    }
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
                            response.Message = "Authentication key is required";
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
                else
                {
                    response.HttpCode = 426;
                    response.InternalCode = versionRequired;
                    response.Message = "Upgrade required";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.UpgradeRequired, response);
                }

            }
            else
            {
                response.HttpCode = 404;
                response.Message = "Version or Platform parameter can not be null";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }

        }

        [HttpPost]
        [Route("ValidateToken")]
        public HttpResponseMessage ValidateToken(HttpRequestMessage pRequest, [FromBody] TokenValidationReq data)
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
                        if (!String.IsNullOrEmpty(data.token))
                        {
                            bool verified = tokenBL.VerifyConsumerToken(data.token, consumerVerified.ConsumerID);

                            if (verified)
                            {
                                response.HttpCode = 200;
                                response.Message = "Success";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, response);
                            }
                            else
                            {
                                response.HttpCode = 200;
                                response.Message = "No records found or token has expired";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }
                        }
                        else
                        {
                            response.HttpCode = 400;
                            response.Message = "Validation token is required";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
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
                    response.Message = "Authentication key is required";
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
    }
}
