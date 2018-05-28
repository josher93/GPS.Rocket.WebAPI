using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace YoVendoRecargaAPI.Game.DAL
{
    public class AgeDAL
    {
        private Connection cnn = new Connection();

        public List<AgeEN> GetAges()
        {
            List<AgeEN> agesList = new List<AgeEN>();

            try
            {
                int AllAges = int.Parse(ConfigurationManager.AppSettings["ShowAllAges"].ToString());
                cnn.Cnn.Open();

                agesList = cnn.Cnn.Query<AgeEN>("SpGetAges", new { AllAges = AllAges }, commandType: CommandType.StoredProcedure).AsList();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error AgeDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return agesList;
        }

        #region GetAgeByID
        public ChangeAgeEN GetAgeImages(int AgeID, int ConsumerID, ref string error)
        {
            ChangeAgeEN result = new ChangeAgeEN();

            try
            {

                cnn.Cnn.Open();

                result = cnn.Cnn.Query<ChangeAgeEN>("SpGetAgeByID", new { AgeID = AgeID, ConsumerID = ConsumerID }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error AgeDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return result;

        }
        #endregion

        #region GetAgeImages
        public List<AgeImagesResponseEN> GetAgeImagesList(int AgeID, ref string error)
        {
            List<AgeImagesResponseEN> result = new List<AgeImagesResponseEN>();

            try
            {

                cnn.Cnn.Open();

                result = cnn.Cnn.Query<AgeImagesResponseEN>("SpGetAgeImages", new { @AgeID = AgeID }, commandType: CommandType.StoredProcedure).AsList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error AgeDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return result;

        }
        #endregion

        #region UpdatePlayersAge
        public PlayersTrackingEN UpdatePlayerAge(int pConsumerID, int pAgeID)
        {
            PlayersTrackingEN playerTracking = new PlayersTrackingEN();
            try
            {
                playerTracking = cnn.Cnn.Query<PlayersTrackingEN>("SpUpdatePlayerAge",
                        new
                        {
                            ConsumerID = pConsumerID,
                            AgeID = pAgeID
                        }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                playerTracking = null;
                Console.WriteLine("Error AgeDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }
            return playerTracking;
        }

        #endregion

    }
}
