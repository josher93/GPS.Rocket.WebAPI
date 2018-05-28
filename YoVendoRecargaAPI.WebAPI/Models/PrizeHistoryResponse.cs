using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class PrizeHistoryResponse : IResponse
    {
        public List<PrizeHistoryEN> data { get; set; }
    }
}