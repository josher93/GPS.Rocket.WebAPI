using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("GATS_TransacctionRecharges", Schema = "Person")]
    public class GatsTransactionEN
    {
        [Key]
        public Int32 Id { get; set; }
        [Column("IdCountry")]
        public Int32 CountryID { get; set; }
        [Column("Phone_Number")]
        public long PhoneNumber { get; set; }
        [Column("Amount")]
        public Decimal Amount { get; set; }
        [Column("Request")]
        public String Request { get; set; }
        [Column("Response")]
        public String Response { get; set; }
        [Column("regdate")]
        public DateTime RegDate { get; set; }
        [Column("Response_Code")]
        public String ResponseCode { get; set; }
        [Column("TransacctionId")]
        public String TransactionID { get; set; }
        [Column("TransacctionIdProvider")]
        public String ProviderTransactionID{ get; set; }
        [Column("Paid")]
        public bool Paid { get; set; }
    }
}
