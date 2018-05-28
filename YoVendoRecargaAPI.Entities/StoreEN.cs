using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("Store", Schema = "API")]
    public class StoreEN
    {
        [Key]
        public Int64 StoreID { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Status")]
        public int Status { get; set; }

        [Column("RegDate")]
        public DateTime RegDate { get; set; }

        [Column("ModDate")]
        public DateTime ModDate { get; set; }

        [Column("ImgUrl")]
        public string ImgUrl { get; set; }

        [Column("Value")]
        public Decimal Value { get; set; }
    }
}
