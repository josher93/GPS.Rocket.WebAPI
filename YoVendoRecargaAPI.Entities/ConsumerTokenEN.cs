using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("ConsumerToken", Schema = "Consumer")]
    public class ConsumerTokenEN
    {
        [Key]
        public int ConsumerTokenID { get; set; }
        [Column("Token")]
        public string Token { get; set; }
        [Column("Status")]
        public bool Status { get; set; }
        [Column("ConsumerID")]
        public int ConsumerID { get; set; }
        [Column("CreationDate")]
        public DateTime CreationDate { get; set; }
        [Column("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }
    }
}
