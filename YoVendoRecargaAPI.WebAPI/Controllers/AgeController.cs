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
using YoVendoRecargaAPI.Game;
using YoVendoRecargaAPI.Game.DAL;
using System.Configuration;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class AgeController : ApiController
    {
        PersonBL personBL = new PersonBL();
        AgeBL ageBL = new AgeBL();
        PersonBL ConsumerAuth = new PersonBL();
        AgeResponse ageResponse = new AgeResponse();
        GenericApiResponse response = new GenericApiResponse();
        AgeInteractor interactor = new AgeInteractor();
        GameBL gameBL = new GameBL();
        string error = "";

        [HttpGet]
        [Route("GetAges")]
        public HttpResponseMessage GetAges(HttpRequestMessage request)
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

                    if (key != null)
                    {
                        List<AgeEN> theAgesList = ageBL.GetAges(ref error);

                        if (theAgesList != null)
                        {
                            AgeResponse ageResponse = interactor.CreateAgeResponse(theAgesList);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, ageResponse);
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
        [Route("SelectAge")]
        public HttpResponseMessage GetAgeImages(HttpRequestMessage request, [FromBody] AgeEN Age)
        {
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

                        if (key != null)
                        {
                            if (Age != null)
                            {
                                var AgeImages = ageBL.GetAgeImages(Age.AgeID, consumerFb.ConsumerID, ref error);

                                if (AgeImages.SouvenirByConsumer >= AgeImages.RequiredSouvenir)
                                {
                                    var AgeImagesList = ageBL.GetAgeImagesList(Age.AgeID, ref error);


                                    ChangeAgeResponse changeAge = new ChangeAgeResponse();

                                    changeAge = interactor.AgeImagesResponse(AgeImages, AgeImagesList);

                                    ageBL.UpdatePlayerAge(consumerFb.ConsumerID, Age.AgeID);

                                    return Request.CreateResponse<IResponse>(HttpStatusCode.OK, changeAge);
                                }
                                else
                                {
                                    response.HttpCode = 400;
                                    response.InternalCode = "01";
                                    response.Message = AgeImages.RequiredSouvenir.ToString();
                                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                                }

                            }
                            else
                            {
                                response.HttpCode = 400;
                                response.Message = "Invalid Parameters";
                                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                            }
                        }
                        else
                        {
                            response.HttpCode = 404;
                            response.Message = "Facebook information cannot be found";
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
