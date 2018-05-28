using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.WebAPI.Models;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class SigninInteractor
    {
        PersonBL personBL = new PersonBL();

        public IResponse createSuccessResponse(PersonEN pPersonAuthenticated)
        {
            SigninResponse response = new SigninResponse();
            try
            {
                
                response.token = pPersonAuthenticated.CurrentToken;
                response.AvailableAmount = Decimal.Round(pPersonAuthenticated.SingleBagValue, 2).ToString();
                response.SesionID = pPersonAuthenticated.SessionID;
                response.VendorM = pPersonAuthenticated.VendorM;
                response.CountryID = pPersonAuthenticated.CountryID.ToString();
                response.ISO3Code = pPersonAuthenticated.ISO3Code;
                response.PhoneCode = pPersonAuthenticated.PhoneCode;
                response.VendorCode = pPersonAuthenticated.VendorCode;
                response.ProfileCompleted = pPersonAuthenticated.ProfileCompleted;
                response.OperatorsBalance = new List<OperatorBalance>();

                foreach (var item in pPersonAuthenticated.OperatorsBalance)
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
    }
}