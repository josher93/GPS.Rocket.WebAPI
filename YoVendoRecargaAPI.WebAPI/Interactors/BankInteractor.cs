using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class BankInteractor
    {
        BankBL bankBL = new BankBL();

        public BankResponse CreateBankResponse(List<BankEN> pBanks)
        {
            BankResponse response = new BankResponse();
            BankList bankList = new BankList();
            bankList.countryBanks = new List<Bank>();

            foreach(var bank in pBanks)
            {
                Bank _bank = new Bank();

                _bank.BankID = bank.BankID;
                _bank.BankName = bank.BankName;
                _bank.minLength = bank.MinLenght;
                _bank.maxLength = bank.MaxLength;

                bankList.countryBanks.Add(_bank);
            }

            response.banks = bankList;
            response.count = bankList.countryBanks.Count;

            return response;


        }
    }
}