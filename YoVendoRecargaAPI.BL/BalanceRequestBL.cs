using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class BalanceRequestBL
    {
        EmailSender emailSender = new EmailSender();
        BalanceRequestDAL balanceRequestDAL = new BalanceRequestDAL();
        public PersonEN RequestBalance(string VendorEmail)
        {
            var result = balanceRequestDAL.RequestBalance(VendorEmail);

            if (result != null)
            {
                emailSender.EmailRequestBalance(result.EmailMaster, result.MyName, result.PersonMaster);
            }

            return result;
        }
    }
}
