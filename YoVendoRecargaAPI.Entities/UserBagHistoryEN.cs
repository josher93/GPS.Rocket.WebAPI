using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("UserBagHistory", Schema = "sales")]
    public class UserBagHistoryEN
    {
        [Key]
        public long ID { get; set; }

        [Column("BagID")]
        public int BagID { get; set; }

        [Column("ProductID")]
        public int ProductID { get; set; }

        [Column("TopupTransactionID")]
        public int TopupTransactionID { get; set; }

        [Column("InitialBagValue")]
        public decimal InitialBagValue { get; set; }

        [Column("FinalBagValue")]
        public decimal FinalBagValue { get; set; }

        [Column("InsertedAt")]
        public DateTime InsertedAt { get; set; }

        [Column("UpdatedAt")]
        public DateTime UpdatedAt { get; set; }

    }
}
