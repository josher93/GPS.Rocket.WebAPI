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
    public class BankReferenceController : ApiController
    {
        PersonBL personBL = new PersonBL();
        PurchaseBL purchaseBL = new PurchaseBL();

        GenericApiResponse response = new GenericApiResponse();
        DepositInteractor interactor = new DepositInteractor();

        [HttpPost]
        [Route("deposito")]
        public HttpResponseMessage InsertBankDeposit(HttpRequestMessage pRequest, [FromBody] BankDepositoReqBody pDepositData)
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
                        PurchaseEN purchase = new PurchaseEN();
                        purchase.Amount = pDepositData.monto;
                        purchase.BankID = Convert.ToInt32(pDepositData.banco);

                        var result = purchaseBL.InsertPurchase(purchase, personVerified, pDepositData.nombre, pDepositData.comprobante, pDepositData.fecha);

                        if (result)
                        {
                            string newToken = personBL.RenewAuthToken(personVerified);
                            var depositResponse = interactor.CreatorDepositResponse(result, newToken);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, depositResponse);
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
