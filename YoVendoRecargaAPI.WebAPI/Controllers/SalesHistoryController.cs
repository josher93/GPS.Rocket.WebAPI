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
    public class SalesHistoryController : ApiController
    {
        PersonBL personBL = new PersonBL();
        SaleBL saleBL = new SaleBL();
        GenericApiResponse response = new GenericApiResponse();
        SalesHistoryInteractor interactor = new SalesHistoryInteractor();

        [HttpGet]
        [Route("history/{when}")]
        public HttpResponseMessage GetSalesHistory(HttpRequestMessage request, string when)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (personVerified != null)
            {
                if (!String.IsNullOrEmpty(when))
                {
                    if (personVerified.IsValidToken)
                    {
                        List<SaleEN> salesList = saleBL.GetIntervalPersonSaleHistory(personVerified, when);

                        if (salesList != null)
                        {
                            var historyResult = interactor.createHistoryResultsResponse(salesList, personBL.RenewAuthToken(personVerified));
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, historyResult);
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
                    response.HttpCode = 400;
                    response.Message = "Time interval parameter must be not null.";
                    return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                }
            }
            else
            {
                response.HttpCode = 401;
                response.Message = "Token authorization has expired or is not valid";
                return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
            }
        }

        [HttpGet]
        [Route("history/gmt0/{when}")]
        public HttpResponseMessage GetSalesHistoryGMT(HttpRequestMessage request, string when)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (!String.IsNullOrEmpty(when))
            {
                if (personVerified.IsValidToken)
                {
                    List<SaleEN> salesList = saleBL.GetIntervalPersonSaleHistory(personVerified, when);

                    if (salesList != null)
                    {
                        var historyResult = interactor.createHistoryResultsResponse(salesList, personBL.RenewAuthToken(personVerified));
                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, historyResult);
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
                response.HttpCode = 400;
                response.Message = "Time interval parameter must not be null.";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpPost]
        [Route("topuppayment/single")]
        public HttpResponseMessage SalePayment(HttpRequestMessage request, [FromBody] SalePaymentReq pPayment)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (token != null)
            {
                if (personVerified.IsValidToken)
                {
                    if (!String.IsNullOrEmpty(pPayment.id))
                    {
                        SalePaymentResponse payment = new SalePaymentResponse();

                        payment.Status = saleBL.UpdateSalePayment(Int32.Parse(pPayment.id), pPayment.paid);

                        if (payment.Status)
                        {
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, payment);
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
                        response.Message = "TransactionID is required";
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
                response.HttpCode = 400;
                response.Message = "Authentication token is required";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        ///CLARO ROCKECT
        #region RocketPayment
        [HttpPost]
        [Route("RocketPayment")]
        public HttpResponseMessage RocketPayment(HttpRequestMessage request, [FromBody] RocketRequest pPayment)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (token != null)
            {
                if (personVerified.IsValidToken)
                {
                    if (pPayment.BalanceID > 0 && pPayment.SecurityPin != null)
                    {
                        SalePaymentResponse payment = new SalePaymentResponse();

                        var DealerPin = saleBL.GetRocketDealerPin(personVerified.PersonID);

                        if (DealerPin == pPayment.SecurityPin)
                        {
                            int pPaid = 1;
                            payment.Status = saleBL.UpdateRocketSalePayment(pPayment.BalanceID, pPaid);

                            if (payment.Status)
                            {
                                return Request.CreateResponse<IResponse>(HttpStatusCode.OK, payment);
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
                            //Pin erróneo
                            response.HttpCode = 400;
                            response.Message = "Invalid Security Pin";
                            return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
                        }
                    }
                    else
                    {
                        response.HttpCode = 400;
                        response.Message = "BalanceID is required";
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
                response.HttpCode = 400;
                response.Message = "Authentication token is required";
                return Request.CreateResponse<IResponse>(HttpStatusCode.BadRequest, response);
            }
        }

        [HttpGet]
        [Route("GetBalanceRocket")]
        public HttpResponseMessage GetBalanceRocket(HttpRequestMessage request)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (personVerified != null)
            {
                    if (personVerified.IsValidToken)
                    {
                        RocketBalanceEN RocketBalance = saleBL.GetBalanceRocket(personVerified.PersonID); //Envía Entidad BalanceRocket

                        if (RocketBalance != null)
                        {
                            var historyResult = interactor.CreateBalanceRocketResponse(RocketBalance);
                            return Request.CreateResponse<IResponse>(HttpStatusCode.OK, historyResult);
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
                response.Message = "Token authorization has expired or is not valid";
                return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
            }
        }

        [HttpGet]
        [Route("GetPaymentHistory")]
        public HttpResponseMessage GetPaymentHistory(HttpRequestMessage request)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (personVerified != null)
            {
                if (personVerified.IsValidToken)
                {
                    List<RocketBalanceEN> RocketBalance = saleBL.GetPaymentsHistoryRocket(personVerified.PersonID); 

                    if (RocketBalance != null)
                    {
                        var historyResult = interactor.createPaymentsHistoryRocket(RocketBalance, personBL.RenewAuthToken(personVerified));
                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, historyResult);
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
                response.Message = "Token authorization has expired or is not valid";
                return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
            }
        }

        [HttpGet]
        [Route("GetSaleDetail")]
        public HttpResponseMessage GetSaleDetail(HttpRequestMessage request)
        {
            IEnumerable<string> token = null;
            request.Headers.TryGetValues("Token-autorization", out token); //TODO: Corregir error ortográfico

            PersonEN personVerified = personBL.VerifyPersonAuthentication(token);

            if (personVerified != null)
            {
                if (personVerified.IsValidToken)
                {
                    personVerified.MasterID = 1019;
                    RocketSaleDetailEN SaleDetail = saleBL.GetSaleDetail(personVerified.MasterID, personVerified.PersonID);

                    if (SaleDetail != null)
                    {
                        var historyResult = interactor.createSaleDetailResponse(SaleDetail, personBL.RenewAuthToken(personVerified));
                        return Request.CreateResponse<IResponse>(HttpStatusCode.OK, historyResult);
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
                response.Message = "Token authorization has expired or is not valid";
                return Request.CreateResponse<IResponse>(HttpStatusCode.Unauthorized, response);
            }
        }

        #endregion
    }
}
