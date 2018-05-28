using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class TransactionsResponse : IResponse
    {
        public HistoryResponse History { get; set; }
        public long count { get; set; }
        public String token { get; set; }
    }
}