using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class LeaderBoardsResponse : IResponse
    {
        public List<LeaderBoards> Leaderboards { get; set; }
        public LeaderBoards LastWinner { get; set; }
        public string filter { get; set; }
    }
}