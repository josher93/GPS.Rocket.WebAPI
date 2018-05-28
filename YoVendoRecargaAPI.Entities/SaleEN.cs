using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class SaleEN
    {
        public int ID { get; set; }
        public string TransactionID { get; set; }
        public string Msisdn { get; set; }
        public Decimal Amount { get; set; }
        public string FormattedAmount { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeZoneDate { get; set; }
        public string ISOServerDate { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public int OperatorID { get; set; }
        public string OperatorName { get; set; }
        public int PersonID { get; set; }
        public string PersonName { get; set; }
        public string Currency { get; set; }
        public bool Success { get; set; }
        public bool Paid { get; set; }
    }
}
