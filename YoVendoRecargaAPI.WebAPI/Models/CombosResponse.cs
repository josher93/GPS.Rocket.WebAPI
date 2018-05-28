using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class CombosResponse
    {
        public int ComboID { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string PrizeDescription { get; set; }
        public List<CombosSouvenirResponse> Souvenir { get; set; }
    }

    public class CombosSResponse: IResponse
    {
        public List<CombosResponse> response { get; set; }
    }
}