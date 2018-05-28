using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class PurchaseBL
    {
        PurchaseDAL purchaseDAL = new PurchaseDAL();
        BankDAL bankDAL = new BankDAL();
        EmailSender emailSender = new EmailSender();

        public bool InsertPurchase(PurchaseEN pPurchase, PersonEN pPerson, string pDepositor, string pBankReference, string pDepositDate)
        {
            bool result = false;

            try
            {
                DateTime date = Convert.ToDateTime(pDepositDate);
                var bank = bankDAL.GetBanks(pPerson.CountryID).Where(b => b.BankID == pPurchase.BankID).FirstOrDefault();
                string amount = Convert.ToString(pPurchase.Amount);
                string fullName = pPerson.Firstname + " " + pPerson.Lastname;
                string depositDate = date.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-MX")).ToUpper();

                result = purchaseDAL.InsertPurchase(pPerson.PersonID, pPurchase.BankID, pPurchase.Amount, pPerson.CountryID, pDepositor, pBankReference, date);

                if (result)
                    emailSender.EmailTransferConfirmationReceived(pPerson.Email, pDepositor, pBankReference, bank.BankName, amount, fullName, depositDate);
            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("PurchaseBL " + ex.Message);
            }

            return result;
        }
    }
}
