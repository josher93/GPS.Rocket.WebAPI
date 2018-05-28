using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    [Table("ExternalClient", Schema = "API")]
    public class ExternalClientEN
    {
        [Key]
        public int ExternalClientID { get; set; }

        [Column("CountryID")]
        public int CountryID { get; set; }

        [Column("Name")]
        public String Name { get; set; }

        [Column("Alias")]
        public String Alias { get; set; }

        [Column("Description")]
        public String Description { get; set; }

        [Column("GUID")]
        public String GUID { get; set; }

        [Column("AssignedPassword")]
        public String AssignedPassword { get; set; }

        [Column("Active")]
        public Boolean Active { get; set; }

        [Column("RegDate")]
        public DateTime RegDate { get; set; }
    }
}
