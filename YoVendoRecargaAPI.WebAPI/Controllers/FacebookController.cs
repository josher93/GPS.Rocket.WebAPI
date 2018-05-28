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

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class FacebookController : ApiController
    {
        PersonBL personBL = new PersonBL();
        FacebookBL facebookBL = new FacebookBL();
        FacebookInteractor interactor = new FacebookInteractor();
        GenericApiResponse response = new GenericApiResponse();

        [HttpPost]
        [Route("InsertUser")]
        public HttpResponseMessage AssociateFacebook(HttpRequestMessage pRequest, [FromBody] UserFacebookReqBody pFacebookData)
        {
            IEnumerable<string> token = null;
            pRequest.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (token != null)
            {
                if (personVerified != null)
                {
                    if (personVerified.IsValidToken)
                    {
                        var validateRequest = interactor.ValidateRequest(pFacebookData);

                        if (validateRequest.result)
                        {
                            SimpleTextResponse simpleResponse = new SimpleTextResponse();
                            HttpResponseMessage httpResponse = new HttpResponseMessage();

                            FacebookEN facebookData = new FacebookEN();
                            facebookData.Email = pFacebookData.email;
                            facebookData.FacebookProfileID = pFacebookData.profileID;
                            facebookData.FacebookUserID = pFacebookData.userID;
                            facebookData.Firstname = pFacebookData.firstname;
                            facebookData.Lastname = pFacebookData.lastname;
                            facebookData.MiddleName = pFacebookData.middlename;
                            facebookData.PersonID = personVerified.PersonID;
                            facebookData.Phone = pFacebookData.phoneNumber;
                            facebookData.URL = pFacebookData.facebookURL;

                            switch (facebookBL.AssociateFacebook(facebookData))
                            {
                                case "updated":
                                    simpleResponse.result = true;
                                    simpleResponse.Message = "Facebook Profile has been Updated";
                                    httpResponse = pRequest.CreateResponse<IResponse>(HttpStatusCode.OK, simpleResponse);
                                    break;
                                case "inserted":
                                    simpleResponse.result = true;
                                    simpleResponse.Message = "Operation completed succesfully";
                                    httpResponse = pRequest.CreateResponse<IResponse>(HttpStatusCode.OK, simpleResponse);
                                    break;
                                case "error":
                                    simpleResponse.result = false;
                                    simpleResponse.Message = "Something went wrong";
                                    httpResponse = pRequest.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, simpleResponse);
                                    break;
                                default:
                                    simpleResponse.result = false;
                                    simpleResponse.Message = "Something went wrong";
                                    httpResponse = pRequest.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, simpleResponse);
                                    break;
                            }

                            return httpResponse;
                        }
                        else
                        {
                            response.HttpCode = 400;
                            response.Message = "Authentication token has expired.";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, validateRequest);
                        }
                    }
                    else
                    {
                        response.HttpCode = 401;
                        response.Message = "Authentication token has expired.";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                    }
                }
                else
                {
                    response.HttpCode = 401;
                    response.Message = "Credentials are not valid.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
                }
            }
            else
            {
                response.HttpCode = 400;
                response.Message = "Authorization token must be provided";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }
    }
}
