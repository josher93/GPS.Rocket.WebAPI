using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class TrackingController : ApiController
    {
        GameBL gameBL = new GameBL();

        [HttpGet]
        [Route("Tracking")]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            string error = "";
            PersonBL ConsumerAuth = new PersonBL();
            TrackingResponse responseApi = new TrackingResponse();
            GenericApiResponse response = new GenericApiResponse();

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

                        var tracking = gameBL.GetProgressGame(consumerFb.ConsumerID, ref error);

                        if (tracking != null)
                        {
                            responseApi.TotalWinCoins = tracking.TotalWinCoins;
                            responseApi.TotalWinPrizes = tracking.TotalWinPrizes;
                            responseApi.CurrentCoinsProgress = tracking.CurrentCoinsProgress;
                            responseApi.TotalSouvenirs = tracking.TotalSouvenirs;
                            responseApi.AgeID = tracking.AgeID;
                            responseApi.Nickname = tracking.Nickname;
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseApi);
                        }
                        else
                        {
                            if (error == "")
                            {
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseApi);
                            }
                            else
                            {
                                response.HttpCode = 500;
                                response.Message = "something went wrong";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, response);
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
