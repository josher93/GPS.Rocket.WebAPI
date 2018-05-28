using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class BalanceRequestResponse : IResponse
    {
        public string MasterName { get; set; }
        public string MasterEmail { get; set; }
        public bool Status { get; set; }
    }
}