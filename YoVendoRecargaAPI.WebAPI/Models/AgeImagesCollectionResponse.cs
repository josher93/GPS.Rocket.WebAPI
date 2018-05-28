using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class AgeImagesCollectionResponse : IResponse
    {
        public string ImgUrl { get; set; }
        public string Type { get; set; }
        public int Sequence { get; set; }


    }
}