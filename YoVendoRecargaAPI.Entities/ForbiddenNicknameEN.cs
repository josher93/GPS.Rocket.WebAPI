using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("ForbiddenNicknames", Schema = "Consumer")]
    public class ForbiddenNicknameEN
    {
        [Key]
        public int ID { get; set; }
        [Column("Forbidden")]
        public string Forbidden { get; set; }
        [Column("CountryID")]
        public int CountryID { get; set; }
        [Column("RegDate")]
        public DateTime RegDate { get; set; }
    }
}
