using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class AchievementBL
    {
        AchievementDAL achievementDAL = new AchievementDAL();

        //public List<AchievementEN> GetAchievements(ref string error)
        //{
        //    var result = achievementDAL.GetAchievements(ref error);
        //    return result;
        //}

        public List<AchievementEN> GetAchievementsByConsumer(int ConsumerID, ref string error)
        {
            var result = achievementDAL.GetAchievementsByConsumer(ConsumerID, ref error);
            return result;
        }
    }
}
