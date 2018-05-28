using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class AchievementDAL 
    {
        private Connection con { get; set; }
        private SqlConnection connection { get; set; }

        public AchievementDAL()
        {
            this.con = new Connection();
        }

        //#region GetAchievements
        //public List<AchievementEN> GetAchievements(ref string error)
        //{

        //    List<AchievementEN> result = new List<AchievementEN>();

        //    try
        //    {

        //        con.Cnn.Open();

        //        result = con.Cnn.Query<AchievementEN>("SpGetAchievements", commandType: CommandType.StoredProcedure).AsList();

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error AchievementDAL: " + ex.Message);
        //        EventViewerLoggerDAL.LogError(ex.Message);
        //    }
        //    finally
        //    {
        //        con.Cnn.Close();
        //    }

        //    return result;

        //}
        //#endregion

        #region GetAchievementsByConsumer
        public List<AchievementEN> GetAchievementsByConsumer(int consumerID, ref string error)
        {
            List<AchievementEN> result = new List<AchievementEN>();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<AchievementEN>("SpGetAchievementsConsumer", new { @ConsumerID = consumerID }, commandType: CommandType.StoredProcedure).AsList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error AchievementDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;

        }
        #endregion
    }
}
