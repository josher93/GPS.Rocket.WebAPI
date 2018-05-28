using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class NewAchievement : IResponse
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public int ValueNextLevel { get; set; }
        public int Prize { get; set; }
    }
}