using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class CountryBL
    {
        CountryDAL countryDAL = new CountryDAL();
        public List<CountryEN> GetCountries()
        {
            var result = countryDAL.GetCountries();
            return result;
        }
    }
}
