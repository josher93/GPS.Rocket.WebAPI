using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class MassivePushResult : IResponse
    {
        public List<MassivePushItemResult> pushResults { get; set; }
        public DateTime operationStart { get; set; }
        public DateTime operationFinished { get; set; }
    }
}