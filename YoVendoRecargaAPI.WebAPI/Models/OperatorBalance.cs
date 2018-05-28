using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class OperatorBalance
    {
        public String mobileOperator { get; set; }
        public int operatorId { get; set; }
        public String balance { get; set; }
    }
}