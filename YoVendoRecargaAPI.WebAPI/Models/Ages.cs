using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;


namespace YoVendoRecargaAPI.WebAPI.Models
{
    public class Ages: IResponse
    {
        public List<AgesListModel> agesListModel { get; set; }
    }
}