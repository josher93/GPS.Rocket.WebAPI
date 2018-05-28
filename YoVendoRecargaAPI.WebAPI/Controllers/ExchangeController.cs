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
    public class ExchangeController : ApiController
    {
        [HttpPost]
        [Route("Exchange")]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] ExchangeDataRequest data)
        {
            int type = 0;
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            PersonBL ConsumerAuth = new PersonBL();
            ExchangeCoinsBL exchangeBL = new ExchangeCoinsBL();
            int coins = 0;
            string error = "";
            string code = "";
            Random random = new Random();

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
                        if (string.IsNullOrEmpty(data.LocationID) && (data.ChestType == 0 || data.ChestType == 0))
                        {
                            response.HttpCode = 400;
                            response.Message = "LocationID or ChestType cannot be empty";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            IEnumerable<string> key = null;
                            request.Headers.TryGetValues("authenticationKey", out key);
                            var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                            if (consumerFb != null)
                            {
                                string RandomCoinsOrSouvenir = ConfigurationManager.AppSettings["RandomCoinOrSouvenir"].ToString();

                                var range = RandomCoinsOrSouvenir.Split(',');
                                int minValue = int.Parse(range[0]);
                                int maxValue = int.Parse(range[1]);
                                int RandomResult = random.Next(minValue, maxValue);

                                if (RandomResult <= 9)
                                    type = 1;
                                else
                                    type = 2;

                                if (type == 1)
                                {
                                    var result = exchangeBL.ProcessExchangeCoins(data.LocationID, data.Longitude, data.Latitude, data.ChestType, consumerFb.ConsumerID, ref coins, data.AgeID);

                                    if (error == "")
                                    {
                                        ExchangeCoinsInteractor interactor = new ExchangeCoinsInteractor();

                                        var responseSuccess = interactor.createExchangeCoinsResultsResponse(result, coins);
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
                                    var result = exchangeBL.ProcessExchangeSouvenir(data.LocationID, data.Longitude, data.Latitude, data.ChestType, consumerFb.ConsumerID, ref error, data.AgeID);

                                    ExchangeCoinsInteractor interactor = new ExchangeCoinsInteractor();

                                    var responseSuccess = interactor.createExchangeSouvewnorResultsResponse(result, error);
                                    return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                                }


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
            catch (Exception)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost]
        [Route("ExchangeWildcard")]
        public HttpResponseMessage exchangeWildcard(HttpRequestMessage request, [FromBody] ExchangeDataRequest data)
        {
            int type = 0;
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            PersonBL ConsumerAuth = new PersonBL();
            ExchangeCoinsBL exchangeBL = new ExchangeCoinsBL();
            int coins = 0;
            string error = "";
            string code = "";
            Random random = new Random();

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
                        if (string.IsNullOrEmpty(data.LocationID) && (data.ChestType == 0 || data.ChestType == 0))
                        {
                            response.HttpCode = 400;
                            response.Message = "LocationID or ChestType cannot be empty";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            IEnumerable<string> key = null;
                            request.Headers.TryGetValues("authenticationKey", out key);
                            var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                            if (consumerFb != null)
                            {
                                string RandomCoinsOrSouvenir = ConfigurationManager.AppSettings["RandomWildCard"].ToString();

                                var range = RandomCoinsOrSouvenir.Split(',');
                                int minValue = int.Parse(range[0]);
                                int maxValue = int.Parse(range[1]);
                                int RandomResult = random.Next(minValue, maxValue);

                                if (RandomResult >= 55 && RandomResult <= 99)
                                    type = 1;
                                else if (RandomResult < 55)
                                    type = 2;
                                else
                                    type = 3;

                                if (type == 1 || type == 2)
                                {
                                    var result = exchangeBL.AddOrDiscountCoinsWildCard(data.LocationID, data.Longitude, data.Latitude, consumerFb.ConsumerID, ref coins, data.AgeID, type);

                                    if (error == "")
                                    {
                                        ExchangeWildcardInteractor interactor = new ExchangeWildcardInteractor();

                                        var responseSuccess = interactor.createExchangeCoinsResultsResponse(result, coins, type);
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
                                    var result = gameBL.ProcessToValidateAndWinPrizeWildCard(consumerFb.ConsumerID, data.LocationID, data.AgeID, ref error);

                                    if (result.ResponseCode == "04")
                                    {

                                        var resultInsertCoins = exchangeBL.AddOrDiscountCoinsWildCard(data.LocationID, data.Longitude, data.Latitude, consumerFb.ConsumerID, ref coins, data.AgeID, 4);

                                        if (error == "")
                                        {
                                            ExchangeWildcardInteractor interactor = new ExchangeWildcardInteractor();
                                            type = 2;
                                            var responseSuccess = interactor.createExchangeCoinsResultsResponse(resultInsertCoins, coins, type);
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
                                        ExchangeWildcardInteractor interactor = new ExchangeWildcardInteractor();

                                        var responseSuccess = interactor.createWinPrizeResultsResponse(result, error);
                                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSuccess);
                                    }

                                }


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
            catch (Exception)
            {
                response.HttpCode = 500;
                response.Message = "something went wrong";
                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}
