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
    public class SouvenirController : ApiController
    {
        GameBL gameBL = new GameBL();
        [HttpGet]
        [Route("SouvenirsByConsumer")]
        public HttpResponseMessage GetSouvenirsByConsumer(HttpRequestMessage request)
        {
            string error = "";
            PersonBL ConsumerAuth = new PersonBL();
            SouvenirInteractor souvenirInteractor = new SouvenirInteractor();

            GenericApiResponse response = new GenericApiResponse();
            SouvenirBL souvenirBL = new SouvenirBL();

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
                            var souvenirsByConsumer = souvenirBL.GetSouvenirsOwnedByConsumer(consumerFb.ConsumerID, ref error);
                            if (souvenirsByConsumer != null)
                            {
                                SouvenirCollection SouvenirCollectionResponse = new SouvenirCollection();
                                SouvenirCollectionResponse.listSouvenirsByConsumer = souvenirInteractor.createSouvenirByConsumerResponse(souvenirsByConsumer);

                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, SouvenirCollectionResponse);
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


        [HttpGet]
        [Route("GetCombos")]
        public HttpResponseMessage GetCombos(HttpRequestMessage request)
        {
            string error = "";
            PersonBL ConsumerAuth = new PersonBL();
            SouvenirInteractor souvenirInteractor = new SouvenirInteractor();

            GenericApiResponse response = new GenericApiResponse();
            SouvenirBL souvenirBL = new SouvenirBL();

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
                            var souvenirsByConsumer = gameBL.GetCombosByConsumerIDAndAge(consumerFb.ConsumerID, ref error); 
                            if (souvenirsByConsumer != null)
                            {
                                CombosSResponse combosResponse = new CombosSResponse();
                                combosResponse.response = souvenirInteractor.createComboResponse(souvenirsByConsumer);

                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, combosResponse);
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
