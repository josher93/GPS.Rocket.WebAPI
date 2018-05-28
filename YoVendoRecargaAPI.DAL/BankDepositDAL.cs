using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class BankDepositDAL
    {
        private Connection cnn = new Connection();

        public PurchaseEN BankDeposit(int pPersonID, int pBankID, decimal pAmount, DateTime pRegDate, int pCountryID, string pName, string pNumber, string pImgReference, DateTime pDepositDate, decimal pComissionClient, decimal pITBM, decimal pComission, int pBankReferenceID, decimal pTax, decimal pTaxFree, decimal pAirTime)
        {
            PurchaseEN bankDeposit = new PurchaseEN();

            try
            {
                bankDeposit = cnn.Cnn.Query<PurchaseEN>("SpBankDeposit",
                    new { PersonID = pPersonID, 
                        BankID = pBankID, 
                        Amount = pAmount, 
                        RegDate = pRegDate, 
                        CountryID = pCountryID, 
                        Name = pName, 
                        Number = pNumber, 
                        ImgReference = pImgReference, 
                        DepositDate = pDepositDate, 
                        comision_cliente = pComissionClient, 
                        itbm = pITBM, 
                        Comisssion = pComission, 
                        BankReferenceID = pBankReferenceID, 
                        Tax = pTax, 
                        TaxFree = pTaxFree, 
                        AirTime = pAirTime }, commandType: CommandType.StoredProcedure).SingleOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error BankDepositDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return bankDeposit;
        }
    }
}
