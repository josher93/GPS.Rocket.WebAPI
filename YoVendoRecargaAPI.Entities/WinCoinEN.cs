using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("WinCoins", Schema = "Consumer")]
    public class WinCoinEN
    {
        public int WinCoinsID { get; set; }
        [Column("LocationID")]
        public string LocationID { get; set; }
        [Column("Longitude")]
        public decimal Longitude { get; set; }
        [Column("Latitude")]
        public decimal Latitude { get; set; }
        [Column("ConsumerID")]
        public int ConsumerID { get; set; }
        [Column("RegDate")]
        public DateTime RegDate { get; set; }
        public int ChestID { get; set; }
        public int ExchangeCoins { get; set; }

        public int ChestType { get; set; }
        public PlayersTrackingEN Tracking { get; set; }
        public AchievementEN Achievement { get; set; }
    }
}
