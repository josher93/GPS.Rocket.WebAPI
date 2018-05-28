using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.WebAPI.Interactors;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class BuildValidatorController : ApiController
    {
        BuildVersionBL buildBL = new BuildVersionBL();
        PersonBL personBL = new PersonBL();
        GenericApiResponse response = new GenericApiResponse();

        [HttpPost]
        [Route("ValidateVersion")]
        public HttpResponseMessage Validate(HttpRequestMessage request, [FromBody] BuildValidationReqBody pBuildValidation)
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
                if (!String.IsNullOrEmpty(pBuildValidation.appVersionName) && !String.IsNullOrEmpty(pBuildValidation.platformName))
                {
                    PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

                    if (personVerified != null)
                    {
                        if (!personVerified.IsValidToken)
                        {
                            response.HttpCode = 401;
                            response.Message = "Authentication token has expired.";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                        }
                        else
                        {
                            var build = buildBL.BuildVersion(pBuildValidation.appVersionName, pBuildValidation.platformName);
                            BuildValidatorInteractor interactor = new BuildValidatorInteractor();
                            BuildResponse buildResponse = interactor.CreateBuildResponse(build);
                            if (buildResponse.Valid)
                            {
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, buildResponse);
                            }
                            else
                            {
                                return Request.CreateResponse<IResponse>(HttpStatusCode.UpgradeRequired, buildResponse);
                            }
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Token authorization expired or not valid";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.HttpCode = 400;
                    response.Message = "Client build version and platform name are required";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
            }
        }
    }
}
