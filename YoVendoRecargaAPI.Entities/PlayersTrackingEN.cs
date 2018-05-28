using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace YoVendoRecargaAPI.Entities
{
    [Table("PlayersTracking", Schema = "Consumer")]
    public class PlayersTrackingEN
    {
        [Key]
        public int PlayersTrackingID { get; set; }
        [Column("TotalWinCoins")]
        public int TotalWinCoins { get; set; }
        [Column("TotalWinPrizes")]
        public int TotalWinPrizes { get; set; }
        [NotMapped]
        public int TotalSouvenirs { get; set; }
        public int CurrentCoinsProgress { get; set; }
        public int ConsumerID { get; set; }
        [NotMapped]
        public DateTime RegDate { get; set; }
        public int AgeID { get; set; }
        public DateTime ModDate { get; set; }
        [NotMapped]
        public int ExchangeCoins { get; set; }
        [NotMapped]
        public string Nickname { get; set; }
     
    }
}
