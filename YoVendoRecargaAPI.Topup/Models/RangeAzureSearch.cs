using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Topup.Models
{
    public class RangeAzureSearch
    {
        public string term_id { get; set; }
        public string term_init { get; set; }
        public string term_end { get; set; }
        public string country_code { get; set; }
        public string mcc { get; set; }
        public string mnc { get; set; }
        public string mno_id { get; set; }
    }
}
