using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class ChangeAgeResponse : IResponse
    {
        public int AgeID { get; set; }
        public string Name { get; set; }
        public string IconImage { get; set; }
        public string MarkerG { get; set; }
        public string MarkerS { get; set; }
        public string MarkerB { get; set; }
        public string MarkerW { get; set; }
        public string WildcardMain { get; set; }
        public string WildcardWin { get; set; }
        public string WildcardLose { get; set; }
        public string PrizeImage { get; set; }

        //public List<AgeImagesCollectionResponse> ImageCollection { get; set; }
    }
}