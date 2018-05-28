using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class HistoryResponse : IResponse
    {
        public List<Sale> transactions { get; set; }
    }
}