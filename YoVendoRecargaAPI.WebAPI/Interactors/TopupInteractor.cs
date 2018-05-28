using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Topup;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class TopupInteractor
    {
        public TopupResultResponse CreateTopupResponse(TopupTransactionEN pTransaction, List<PersonBagOperatorEN> pBags)
        {
            TopupResultResponse response = new TopupResultResponse();

            try
            {
                response.id = pTransaction.ServiceTransactionID;
                response.code = pTransaction.Code;
                response.message = pTransaction.Message;
                response.info = new Info();
                response.info.Mno = pTransaction.Mno;
                response.info.Msisdn = pTransaction.Msisdn;
                response.OperatorsBalance = new List<OperatorBalance>();

                foreach (var item in pBags)
                {
                    OperatorBalance balance = new OperatorBalance();
                    balance.mobileOperator = item.MobileOperatorName;
                    balance.operatorId = item.MobileOperatorID;
                    balance.balance = item.UserBalance;
                    response.OperatorsBalance.Add(balance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }

        public TopupRequestsResponse CreatePendingTopupsResponse(List<TopupRequestEN> requests)
        {
            TopupRequestsResponse response = new TopupRequestsResponse();
            PendingRequests requestsList = new PendingRequests();
            requestsList.PendingRequestsList = new List<TopupRequestItem>();

            try
            {
                foreach (var item in requests)
                {
                    TopupRequestItem req = new TopupRequestItem();
                    req.Amount = Convert.ToString(item.Amount);
                    req.Date = item.RequestDate;
                    req.DateGMT = item.RequestDateISO;
                    req.Nickname = item.ConsumerNickname;
                    req.OperatorName = item.OperatorName;
                    req.PhoneNumber = item.TargetPhone;
                    req.TopUpRequestID = item.TopupRequestID;
                    requestsList.PendingRequestsList.Add(req);
                }

                response.pendingRequests = requestsList;
                response.count = requestsList.PendingRequestsList.Count;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }

        public SimpleTextResponse ValidateTopupRequest(TopupRequestReq pRequest)
        {
            SimpleTextResponse response = new SimpleTextResponse();

            try
            {
                if (pRequest.amount <= 0)
                {
                    response.result = false;
                    response.Message = "Amount must be greater than zero";
                }
                else if (pRequest.operatorID <= 0)
                {
                    response.result = false;
                    response.Message = "Operator ID is not valid";
                }
                else if (String.IsNullOrEmpty(pRequest.targetPhoneNumber))
                {
                    response.result = false;
                    response.Message = "Target phone numner is required";
                }
                else if (pRequest.vendorCode <= 0)
                {
                    response.result = false;
                    response.Message = "Vendor code is not valid";
                }
                else
                {
                    response.result = true;
                    response.Message = "Ok";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return response;
        }
    }
}