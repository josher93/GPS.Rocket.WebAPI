using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class CountryOperators: IResponse
    {
        
        public int operatorID { get; set; }
        public string name { get; set; }
        public string brand { get; set; }
        public string mnc { get; set; }
        public string operatorLogo { get; set; }
        public string logoUrl { get; set; }
        public int logoVersion { get; set; }
        public string HexColor { get; set; }
        public string RGBColor { get; set; }
        public int Relevance { get; set; }
        public RGBColors rgbColor { get; set; }
    }
}