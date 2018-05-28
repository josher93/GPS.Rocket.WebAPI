using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("StoreReports", Schema = "Consumer")]
    public class StoreReportDetailsEN
    {
        [Key]
        public int ID { get; set; }
        [Column("StoreName")]
        public string StoreName { get; set; }
        [Column("AddressStore")]
        public string AddressStore { get; set; }
        [Column("Longitude")]
        public decimal Longitude { get; set; }
        [Column("Latitude")]
        public decimal Latitude { get; set; }
        [Column("FirebaseID")]
        public string FirebaseID { get; set; }
        [Column("ConsumerID")]
        public int ConsumerID { get; set; }
        [Column("RegDate")]
        public DateTime RegDate { get; set; }
        [Column("ModDate")]
        public DateTime ModDate { get; set; }
    }
}
