using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class ProductEN
    {
        public int ProductID { get; set; }
        public int OperatorID { get; set; }
        public int CategoryID { get; set; }
        public string Code { get; set; }
        public string OperatorName { get; set; }
        public string Brand { get; set; }    
        public Decimal Amount { get; set; }
        public String Description { get; set; }
        public int Relevance { get; set; }
        public int OperatorRelevance { get; set; }
        public int ProviderProductCode { get; set; }
        public Decimal PersonDiscount { get; set; }
        public Decimal InventoryDiscount { get; set; }
        public Decimal ProviderDiscount { get; set; }
        public int PackageCode { get; set; }
        public bool Available { get; set; }
        public bool Active { get; set; }
    }
}
