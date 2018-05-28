using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("Age", Schema = "Game")]
    public class AgeEN
    {
        [Key]
        public int AgeID { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("Status")]
        public int Status { get; set; }
        [Column("IconImage")]
        public string IconImage { get; set; }
        [Column("MainImage")]
        public string MainImage { get; set; }
        [Column("RegDate")]
        public DateTime RegDate { get; set; }
        [Column("ModDate")]
        public DateTime ModDate { get; set; }
}
}
