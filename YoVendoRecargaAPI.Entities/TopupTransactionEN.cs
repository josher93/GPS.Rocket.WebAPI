using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("topupstransactions", Schema = "sales")]
    public class TopupTransactionEN
    {
        [Key]
        public long TransactionID { get; set; }

        [Column("PersonID")]
        public String PersonID { get; set; }

        [Column("GATS_TransacctionRechargesID")]
        public long GATSTransactionID { get; set; }

        [Column("AmountRqstd")]
        public String AmountRequested { get; set; }

        [Column("PersonDiscount")]
        public decimal PersonDiscount { get; set; }

        [Column("InventoryDiscount")]
        public decimal InventoryDiscount { get; set; }

        [Column("Status")]
        public Byte Status { get; set; }

        [Column("regdate")]
        public DateTime RegDate { get; set; }

        [Column("CountryID")]
        public Int32 CountryID { get; set; }

        [Column("Operator")]
        public String Operator { get; set; }
        [NotMapped]
        public String ServiceTransactionID { get; set; }
        [NotMapped]
        public String Code { get; set; }
        [NotMapped]
        public String Message { get; set; }
        [NotMapped]
        public String Msisdn { get; set; }
        [NotMapped]
        public String Mno { get; set; }
        [NotMapped]
        public String RequestURL { get; set; }
        [NotMapped]
        public String Response { get; set; }
    }
}
