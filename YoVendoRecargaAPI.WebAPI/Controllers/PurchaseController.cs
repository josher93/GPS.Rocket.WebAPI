using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.WebAPI.Interactors;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class PurchaseController : ApiController
    {
        [HttpPost]
        [Route("Purchase")]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] PurchaseRequest data)
        {
            int type = 0;
            GenericApiResponse response = new GenericApiResponse();
            GameBL gameBL = new GameBL();
            PersonBL ConsumerAuth = new PersonBL();
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
                        if (data.StoreId == 0 || data.StoreId == null)
                        {
                            response.HttpCode = 400;
                            response.Message = "StoreId cannot be empty";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            IEnumerable<string> key = null;
                            request.Headers.TryGetValues("authenticationKey", out key);
                            var consumerFb = ConsumerAuth.authenticateConsumer(key.FirstOrDefault().ToString());

                            if (consumerFb != null)
                            {

                                var result = gameBL.PurchaseAndGetSouvenir(consumerFb.ConsumerID, data.StoreId, ref error);

                                if (result != null && error == "")
                                {
                                    ShopPurchaseInteractor interactor = new ShopPurchaseInteractor();

                                    var responseSuccess = interactor.createPurchaseResponse(result, error);
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
