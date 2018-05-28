using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.WebAPI.Interactors;
using System.Configuration;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class PrizeController : ApiController
    {

        [HttpPost]
        [Route("Prize/History")]
        public HttpResponseMessage History(HttpRequestMessage request)
        {
            PersonBL ConsumerAuth = new PersonBL();
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            PrizeHistoryResponse historyResponse = new PrizeHistoryResponse();
            string error = "";

            try
            {
                IEnumerable<string> AppVersion = null;
                request.Headers.TryGetValues("AppVersion", out AppVersion);

                IEnumerable<string> Platform = null;
                request.Headers.TryGetValues("Platform", out Platform);

                bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

                if (ApplyValidation == false || (AppVersion != null && Platform != null))
                {
                    string versionRequired = "";

                    var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                    if (isValidVersion)
                    {
                        IEnumerable<string> key = null;
                        request.Headers.TryGetValues("authenticationKey", out key);
                        var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                        if (consumerFb != null)
                        {
                            historyResponse.data = gameBL.GetPrizeHistoryByConsumerID(consumerFb.ConsumerID, ref error);

                            if (error == "")
                            {
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, historyResponse);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }
                        }

                        else
                        {
                            response.HttpCode = 404;
                            response.Message = "Facebook information not found";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
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
            catch (Exception)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }


        }

        [HttpPost]
        [Route("WinPrize")]
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            PersonBL ConsumerAuth = new PersonBL();
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            string error = "";

            try
            {
                IEnumerable<string> AppVersion = null;
                request.Headers.TryGetValues("AppVersion", out AppVersion);

                IEnumerable<string> Platform = null;
                request.Headers.TryGetValues("Platform", out Platform);

                bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

                if (ApplyValidation == false || (AppVersion != null && Platform != null))
                {
                    string versionRequired = "";

                    var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                    if (isValidVersion)
                    {
                        IEnumerable<string> key = null;
                        request.Headers.TryGetValues("authenticationKey", out key);
                        var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                        if (consumerFb != null)
                        {
                            var process = gameBL.ProcessToValidateAndWinPrize(consumerFb.ConsumerID, consumerFb.CountryID, ref error);

                            if (error == "" && (process.ResponseCode == "00" || process.ResponseCode == "02"))
                            {
                                WinPrizeInteractor interactor = new WinPrizeInteractor();

                                var responseSuccess = interactor.createWinPrizeResultsResponse(process, error);
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }

                        }
                        else
                        {
                            response.HttpCode = 404;
                            response.Message = "Facebook information not found";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
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
            catch (Exception en)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }

        }

        [HttpPost]
        [Route("ExchangeSouvenir")]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] ExchangeSouvenirRequest data)
        {
            PersonBL ConsumerAuth = new PersonBL();
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            string error = "";

            try
            {
                IEnumerable<string> AppVersion = null;
                request.Headers.TryGetValues("AppVersion", out AppVersion);

                IEnumerable<string> Platform = null;
                request.Headers.TryGetValues("Platform", out Platform);

                bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

                if (ApplyValidation == false || (AppVersion != null && Platform != null))
                {
                    string versionRequired = "";

                    var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                    if (isValidVersion)
                    {
                        IEnumerable<string> key = null;
                        request.Headers.TryGetValues("authenticationKey", out key);
                        var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                        if (consumerFb != null)
                        {
                            var process = gameBL.ProcessToExchangeSouvenirByPrize(consumerFb.ConsumerID, data.SouvenirID, consumerFb.CountryID, ref error);

                            if (error == "" && (process.ResponseCode == "00" || process.ResponseCode == "02"))
                            {
                                WinPrizeInteractor interactor = new WinPrizeInteractor();

                                var responseSuccess = interactor.createExchangeSouvenirResponse(process, error);
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }

                        }
                        else
                        {
                            response.HttpCode = 404;
                            response.Message = "Facebook information not found";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
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
            catch (Exception en)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }

        }


        [HttpPost]
        [Route("ActivatePrize")]
        public HttpResponseMessage ActivatePrize(HttpRequestMessage request, [FromBody] ActivatePrizeRequest data)
        {
            PersonBL ConsumerAuth = new PersonBL();
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            string error = "";

            try
            {
                IEnumerable<string> AppVersion = null;
                request.Headers.TryGetValues("AppVersion", out AppVersion);

                IEnumerable<string> Platform = null;
                request.Headers.TryGetValues("Platform", out Platform);

                bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

                if (ApplyValidation == false || (AppVersion != null && Platform != null))
                {
                    string versionRequired = "";

                    var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                    if (isValidVersion)
                    {
                        if (String.IsNullOrEmpty(data.Phone) || String.IsNullOrEmpty(data.PIN))
                        {
                            string errorMessage = (String.IsNullOrEmpty(data.Phone)) ? "Phone can not be empty" : "PIN can not be empty";

                            response.HttpCode = 404;
                            response.Message = errorMessage;
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            IEnumerable<string> key = null;
                            request.Headers.TryGetValues("authenticationKey", out key);
                            var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                            if (consumerFb != null)
                            {
                                ActivatePrizeInteractor interactor = new ActivatePrizeInteractor();

                                var responseSuccess = interactor.SuccessActivatePrizeResponse(consumerFb.ConsumerID, data.Phone, data.PIN, ref error);
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);


                            }
                            else
                            {
                                response.HttpCode = 404;
                                response.Message = "Facebook information not found";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                            }
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
            catch (Exception en)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }

        }


        [HttpPost]
        [Route("ExchangeCombo")]
        public HttpResponseMessage ExchangeCombo(HttpRequestMessage request, [FromBody] ExchangeComboRequest data)
        {
            PersonBL ConsumerAuth = new PersonBL();
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            string error = "";

            try
            {
                IEnumerable<string> AppVersion = null;
                request.Headers.TryGetValues("AppVersion", out AppVersion);

                IEnumerable<string> Platform = null;
                request.Headers.TryGetValues("Platform", out Platform);

                bool ApplyValidation = bool.Parse(ConfigurationManager.AppSettings["ApplyValidationAppVersion"].ToString());

                if (ApplyValidation == false || (AppVersion != null && Platform != null))
                {
                    string versionRequired = "";

                    var isValidVersion = (ApplyValidation == false) ? true : gameBL.IsValidAppVersion(AppVersion.FirstOrDefault(), Platform.FirstOrDefault(), ref error, ref versionRequired);

                    if (isValidVersion)
                    {
                        IEnumerable<string> key = null;
                        request.Headers.TryGetValues("authenticationKey", out key);
                        var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                        if (consumerFb != null)
                        {
                            var process = gameBL.ExchangeCombos(consumerFb.ConsumerID, data.ComboID, ref error);

                            if (error == "" && (process.ResponseCode == "00" || process.ResponseCode == "02"))
                            {
                                WinPrizeInteractor interactor = new WinPrizeInteractor();

                                var responseSuccess = interactor.createExchangeComboResponse(process, error);
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                            }
                            else if (process.ResponseCode == "03")
                            {
                                response.HttpCode = 202;
                                response.Message = "You need some Souvenir";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.Accepted, response);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
                            }

                        }
                        else
                        {
                            response.HttpCode = 404;
                            response.Message = "Facebook information not found";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
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
            catch (Exception en)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }

        }
    }
}
