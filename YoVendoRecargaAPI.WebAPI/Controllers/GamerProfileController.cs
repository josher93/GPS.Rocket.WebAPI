using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Controllers
{
    public class GamerProfileController : ApiController
    {
        PersonBL personBL = new PersonBL();
        GenericApiResponse response = new GenericApiResponse();

        [HttpPost]
        [Route("InsertGameProfile")]
        public HttpResponseMessage AddPersonGamerProfile(HttpRequestMessage pRequest, [FromBody] GamerProfileReq pGamerProfile)
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
                        if (!String.IsNullOrEmpty(pGamerProfile.Nickname))
                        {
                            SimpleTextResponse nicknameResponse = new SimpleTextResponse();
                            HttpResponseMessage responseMessage;

                            switch (personBL.AddPersonNickname(personVerified, pGamerProfile.Nickname).Message)
                            {
                                case "error":
                                    nicknameResponse.Message = "Something went wrong";
                                    nicknameResponse.result = false;
                                    responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, nicknameResponse);
                                    break;
                                case "inserted":
                                    nicknameResponse.Message = "Operation completed succesfully";
                                    nicknameResponse.result = true;
                                    responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.OK, nicknameResponse);
                                    break;
                                case "conflict":
                                    nicknameResponse.Message = "Nickname already exist";
                                    nicknameResponse.result = false;
                                    responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.NotAcceptable, nicknameResponse);
                                    break;
                                default:
                                    nicknameResponse.Message = "Something went wrong";
                                    nicknameResponse.result = false;
                                    responseMessage = Request.CreateResponse<IResponse>(HttpStatusCode.InternalServerError, nicknameResponse);
                                    break;
                            }

                            return responseMessage;
                        }
                        else
                        {
                            response.HttpCode = 400;
                            response.Message = "Nickname is required.";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
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
