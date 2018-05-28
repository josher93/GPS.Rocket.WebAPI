using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class OperatorProductsResponse : IResponse
    {
        public OperatorsProducts operators { get; set; }
        public int count { get; set; }
    }
}