using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("ConsumerAuth", Schema = "Consumer")]
    public class ConsumerAuthKeyEN
    {
        [Key]
        public int ConsumerAuthID { get; set; }

        [Column("ConsumerID")]
        public int ConsumerID { get; set; }

        [Column("ConsumerAuthKey")]
        public string ConsumerAuthKey { get; set; }

        [Column("RegDate")]
        public DateTime RegDate { get; set; }
    }
}
