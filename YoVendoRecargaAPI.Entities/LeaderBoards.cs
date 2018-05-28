using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.Entities
{
    public class LeaderBoards
    {
        public string Nickname { get; set; }
        public int TotalCoins { get; set; }
        
    }

    public class VLeaderBoardsToday
    {
        public string Nickname { get; set; }
        public int TotalCoins { get; set; }
    }

    public class VLeaderBoardsWeek
    {
        public string Nickname { get; set; }
        public int TotalCoins { get; set; }
    }

    public class VLeaderBoardsMonth
    {
        public string Nickname { get; set; }
        public int TotalCoins { get; set; }
    }

    public class VLeaderBoardsOverAll
    {
        public string Nickname { get; set; }
        public int TotalCoins { get; set; }
    }

    public class AllLeaderBoards
    {
        public List<LeaderBoards> ListLeaderBoardsToday { get; set; }
        public List<LeaderBoards> ListLeaderBoardsWeek { get; set; }
        public List<LeaderBoards> ListLeaderBoardsMonth { get; set; }
        public List<LeaderBoards> ListLeaderBoardsOverAll { get; set; }
    }

    public class LastWinnerLeaderBoards
    {
        public LeaderBoards LastWinnerOnYesterday { get; set; }
        public LeaderBoards LastWinnerOnWeek { get; set; }
        public LeaderBoards LastWinnerOnMonth { get; set; }
    }
}
