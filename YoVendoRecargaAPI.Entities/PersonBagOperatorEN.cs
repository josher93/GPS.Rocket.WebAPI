using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class PersonBagOperatorEN
    {
        public int BagID { get; set; }
        public int PersonID { get; set; }
        public string MobileOperatorName { get; set; }
        public int MobileOperatorID { get; set; }
        public string UserBalance { get; set; }
        public Decimal UserBalanceAmount { get; set; }
        public bool Status { get; set; }

    }
}
