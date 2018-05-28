using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class BankDepositBL
    {
        BankDepositDAL bankDepositDAL = new BankDepositDAL();

        //public BankDepositEN InsertDeposit(int pPersonID, int pBankID, decimal pAmount, DateTime pRegDate, int pCountryID, string pName, string pNumber, string pImgReference, DateTime pDepositDate, decimal pComissionClient, decimal pITBM, decimal pComission, int pBankReferenceID, decimal pTax, decimal pTaxFree, decimal pAirTime)
        //{
        //    BankDepositEN deposit = new BankDepositEN();

        //    try
        //    {
        //        deposit = bankDepositDAL.BankDeposit(pPersonID, pBankID, pAmount, pRegDate, pCountryID, pName, pNumber, pImgReference, pDepositDate, pComissionClient, pITBM, pComission, pBankReferenceID, pTax, pTaxFree, pAirTime);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException);
        //        EventViewerLoggerBL.LogError(ex.Message);
        //    }

        //    return deposit;
        //}
    }
}
