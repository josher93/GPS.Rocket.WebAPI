using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class RocketBalanceEN
    {
        public int BalanceID { get; set; }
        public decimal BalanceAmount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Status { get; set; }
        public DateTime ConciliationDate { get; set; }
        public decimal Profit { get; set; }
        public int PersonID { get; set; }
    }
}
