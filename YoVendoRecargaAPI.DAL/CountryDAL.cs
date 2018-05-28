using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class CountryDAL
    {
        public List<CountryEN> GetCountries()
        {
            List<CountryEN> CountriesList = new List<CountryEN>();
            Connection connection = new Connection();
            try
            {
                
                connection.Cnn.Open();
                CountriesList = connection.Cnn.Query<CountryEN>(@"select cty.iso3code as code, cty.NAME as name,cty.Id as countrycode, 
                                                    cty.PhoneCode as PhoneCode from VGetCountries as cty").AsList();
            }
            catch (Exception ex)
            {
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return CountriesList;
        }
    }
}
