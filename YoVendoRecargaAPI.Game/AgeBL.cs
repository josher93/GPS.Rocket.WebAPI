using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Game.DAL;
using System.Configuration;

namespace YoVendoRecargaAPI.Game
{
    public class AgeBL
    {

       
        AgeDAL ageDAL = new AgeDAL();

        public ChangeAgeEN GetAgeImages(int AgeID, int ConsumerID, ref string error)
        {
            var result = ageDAL.GetAgeImages(AgeID, ConsumerID, ref error);
            return result;
        }

        public List<AgeImagesResponseEN> GetAgeImagesList(int AgeID, ref string error)
        {
            var result = ageDAL.GetAgeImagesList(AgeID, ref error);
            return result;
        }


        public List<AgeEN> GetAges(ref string error)
        {
            List<AgeEN> agesList = new List<AgeEN>();

            try
            {
                agesList = ageDAL.GetAges();
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return agesList;
        }

        public PlayersTrackingEN UpdatePlayerAge(int ConsumerID, int AgeID)
        {
            var result = ageDAL.UpdatePlayerAge(ConsumerID, AgeID);
            return result;
        }
    }
}
