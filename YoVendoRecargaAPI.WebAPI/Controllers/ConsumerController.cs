using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.WebAPI.Interactors;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class ConsumerController : ApiController
    {
        ConsumerBL consumerBL = new ConsumerBL();
        RegisterConsumerResponse responseRegister = new RegisterConsumerResponse();
        GenericApiResponse response = new GenericApiResponse();
        ConsumerInteractor interactor = new ConsumerInteractor();
        GameBL gameBL = new GameBL();

        [HttpPost]
        [Route("ConsumerSignin")]
        public HttpResponseMessage InsertConsumer(HttpRequestMessage request, [FromBody] ConsumerEN data)
        {
            IEnumerable<string> AppVersion = null;
            request.Headers.TryGetValues("AppVersion", out AppVersion);

            IEnumerable<string> Platform = null;
            Request.Headers.TryGetValues("Platform", out Platform);

            bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

            if (ApplyValidation == false || (AppVersion != null && Platform != null))
            {
                string versionRequired = "";
                string error = "";

                var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                if (isValidVersion)
                {

                    if (!string.IsNullOrEmpty(data.ProfileID) && (!string.IsNullOrEmpty(data.UserID)))
                    {
                        bool ConsumerExists = consumerBL.SelectConsumerProfile(data.ProfileID);

                        if (ConsumerExists)
                        {
                            ConsumerEN updateConsumer = consumerBL.UpdateConsumerProfile(data.Phone, data.CountryID, data.DeviceID, data.URL, data.Email, data.ProfileID, data.UserID, data.FirstName, data.MiddleName, data.LastName, data.ConsumerAuthKey);

                            if (updateConsumer != null)
                            {
                                responseRegister = interactor.CreatorUpdateResponse(updateConsumer);
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseRegister);
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
                            ConsumerEN registerConsumer = consumerBL.InsertConsumer(data.Phone, data.CountryID, data.DeviceID, data.URL, data.Email, data.ProfileID, data.UserID, data.FirstName, data.MiddleName, data.LastName, data.Nickname, data.ConsumerAuthKey);

                            if (registerConsumer != null)
                            {
                                responseRegister = interactor.CreatorRegisterResponse(registerConsumer);
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseRegister);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "Something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }
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
        [Route("InsertNicknameConsumer")]
        public HttpResponseMessage InsertNicknameConsumer(HttpRequestMessage pRequest, [FromBody] NicknameReq data)
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
                                if (String.IsNullOrEmpty(consumerVerified.Nickname))
                                {
                                    if (String.IsNullOrEmpty(data.Nickname))
                                    {
                                        response.HttpCode = 400;
                                        response.Message = "Nickname is required";
                                        return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                                    }
                                    else
                                    {
                                        SimpleTextResponse response = new SimpleTextResponse();
                                        HttpResponseMessage responseMessage;

                                        switch (consumerBL.AddConsumerNickname(consumerVerified, data.Nickname).Message)
                                        {
                                            case "error":
                                                response.Message = "Something went wrong";
                                                response.result = false;
                                                responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                                                break;
                                            case "updated":
                                                response.Message = "Operation completed succesfully";
                                                response.result = true;
                                                responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.OK, response);
                                                break;
                                            case "conflict":
                                                response.Message = "Nickname already exist";
                                                response.result = false;
                                                responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.NotAcceptable, response);
                                                break;
                                            case "forbidden":
                                                response.Message = "Forbidden nickname";
                                                response.result = false;
                                                responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.Forbidden, response);
                                                break;
                                            default:
                                                response.Message = "Something went wrong";
                                                response.result = false;
                                                responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                                                break;
                                        }

                                        return responseMessage;
                                    }
                                }
                                else
                                {
                                    response.HttpCode = 401;
                                    response.Message = "User has already a Nickname";
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
    }
}
