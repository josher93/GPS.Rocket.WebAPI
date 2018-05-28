using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class PurchaseEN
    {
        public int PurchaseID { get; set; }
        public int PersonID { get; set; }
        public int BankID { get; set; }
        public int BankReferenceID { get; set; }
        public String InternalAuthoriation { get; set; }
        public int AuthorizationStatus { get; set; }
        public Decimal AirTime { get; set; }
        public Decimal Tax { get; set; }
        public Decimal TaxFree { get; set; }
        public Decimal Percentage { get; set; }
        public Decimal Amount { get; set; }
        public DateTime ModDate { get; set; }
        public DateTime RegDate { get; set; }
    }
}
