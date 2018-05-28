using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.WebAPI.Interactors;
using YoVendoRecargaAPI.Game;
using System.Configuration;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class GameStoreController : ApiController
    {
        GameBL gameBL = new GameBL();

        [HttpGet]
        [Route("StoreItems")]
        public HttpResponseMessage GetStoreItems(HttpRequestMessage request)
        {
            string error = "";
            PersonBL ConsumerAuth = new PersonBL();
            GameStoreResponse storeResponse = new GameStoreResponse();
            GameStoreInteractor storeInteractor = new GameStoreInteractor();
            GenericApiResponse response = new GenericApiResponse();
            StoreBL storeBL = new StoreBL();

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
                            var storeItems = storeBL.GetStoreItems(consumerFb.ConsumerID);
                            if (storeItems != null)
                            {
                                storeInteractor.listGameStoreResponse = storeInteractor.createStoreItemsResponse(storeItems);

                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, storeInteractor);
                            }
                            else
                            {
                                response.HttpCode = 400;
                                response.Message = "invalid parameters";
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
    }
}
