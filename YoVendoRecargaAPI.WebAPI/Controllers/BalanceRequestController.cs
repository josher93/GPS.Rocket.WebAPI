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
    public class BalanceRequestController : ApiController
    {
        PersonBL personBL = new PersonBL();
        GenericApiResponse response = new GenericApiResponse();
        BalanceRequestBL balanceRequestBL = new BalanceRequestBL();
        BalanceRequestInteractor interactor = new BalanceRequestInteractor();

        [HttpPost]
        [Route("BalanceRequest")]
        public HttpResponseMessage Post(HttpRequestMessage request, [FromBody] BalanceRequest requestBody)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (!string.IsNullOrEmpty(requestBody.VendorEmail))
            {
                if (personVerified.IsValidToken)
                {
                    var inf = balanceRequestBL.RequestBalance(requestBody.VendorEmail);

                    if (inf != null)
                    {
                        var responseSucces = interactor.createSuccessResponse(inf);

                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, responseSucces);
                    }
                    else
                    {
                        response.HttpCode = 404;
                        response.Message = "Seller not found";
                        return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
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
                response.HttpCode = 400;
                response.Message = "Email parameter must not be null.";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }
    }
}
