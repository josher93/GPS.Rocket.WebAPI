using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ProductsResponse : IResponse
    {
        public List<TopupProducts> products { get; set; }
    }
}