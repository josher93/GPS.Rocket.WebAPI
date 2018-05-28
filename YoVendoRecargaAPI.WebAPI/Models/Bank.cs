using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class Bank: IResponse
    {
        public int BankID { get; set; }
        public string BankName { get; set; }
        public int minLength { get; set; }
        public int maxLength { get; set; }
    }
}