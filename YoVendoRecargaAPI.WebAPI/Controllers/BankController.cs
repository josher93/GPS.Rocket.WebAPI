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
    public class BankController : ApiController
    {
        BankBL bankBL = new BankBL();
        PersonBL personBL = new PersonBL();
        BankResponse bankResponse = new BankResponse();
        GenericApiResponse response = new GenericApiResponse();
        BankInteractor interactor = new BankInteractor();

        [HttpGet]
        [Route("banks")]
        public HttpResponseMessage GetBanks(HttpRequestMessage request)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token);

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if(token != null)
            {
                if(personVerified != null)
                {
                    if (personVerified.IsValidToken)
                    {
                        List<BankEN> theBanksList = bankBL.GetBankList(personVerified.CountryID);

                        if(theBanksList != null)
                        {
                            BankResponse bankResponse = interactor.CreateBankResponse(theBanksList);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, bankResponse);
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
