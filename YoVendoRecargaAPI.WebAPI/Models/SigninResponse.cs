using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class SigninResponse : IResponse
    {
        public String token { get; set; }
        public String AvailableAmount { get; set; }
        public int SesionID { get; set; }
        public bool VendorM { get; set; }
        public string CountryID { get; set; }
        public string ISO3Code { get; set; }
        public string PhoneCode { get; set; }
        public int VendorCode { get; set; }
        public bool ProfileCompleted { get; set; }
        public List<OperatorBalance> OperatorsBalance { get; set; }
    }
}