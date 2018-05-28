using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class UserBagInteractor
    {
        public UserBagResponse CreateBagResponse(List<PersonBagOperatorEN> pBagsResult, string pNewToken, int pPersonID)
        {
            UserBagResponse response = new UserBagResponse();
            response.userBag = new UserBag();
            response.userBag.OperatorsBalance = new List<OperatorBalance>();

            decimal higherAmount = 0;

            try
            {
                foreach (var item in pBagsResult)
                {
                    OperatorBalance balance = new OperatorBalance();
                    balance.mobileOperator = item.MobileOperatorName;
                    balance.operatorId = item.MobileOperatorID;
                    balance.balance = item.UserBalance;
                    response.userBag.OperatorsBalance.Add(balance);
                    higherAmount = (item.UserBalanceAmount > higherAmount) ? item.UserBalanceAmount : higherAmount;
                }

                response.userBag.PersonID = pPersonID;
                response.userBag.AvailableAmount = Convert.ToString(higherAmount);
                response.token = pNewToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }
    }
}