using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("PersonProfile", Schema = "Game")]
    public class GamerProfileEN
    {
        [Key]
        public int ID { get; set; }

        [Column("NickName")]
        public String Nickname { get; set; }

        [Column("PersonID")]
        public int PersonID { get; set; }

        [Column("regDate")]
        public DateTime RegDate { get; set; }
    }
}
