using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class OperatorEN
    {
        public int OperatorID { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Mnc { get; set; }
        public int CountryID { get; set; }
        public string ISO2Code { get; set; }
        public string OperatorLogo { get; set; }
        public string LogoUrl { get; set; }
        public int LogoVersion { get; set; }
        public string HEXColor { get; set; }
        public string RGBColor { get; set; }
        public int Relevance { get; set; }
        public List<ProductEN> Products { get; set; }
        public bool ConsumerAllowed { get; set; }

    }

}
