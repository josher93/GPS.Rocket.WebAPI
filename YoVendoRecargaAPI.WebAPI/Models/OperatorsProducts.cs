using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class OperatorsProducts : IResponse
    {
        public List<OperatorProduct> countryOperators { get; set; }
    }
}