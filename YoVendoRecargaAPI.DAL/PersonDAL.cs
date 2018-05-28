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
    public class PersonDAL
    {
        private Connection connection = new Connection();

        public PersonEN AuthenticatePerson(String pEmail, String pPassword)
        {
            PersonEN person = new PersonEN();

            try
            {
                connection.Cnn.Open();
                person = connection.Cnn.Query<PersonEN>("SpValidateVendorCredentials", new { email = pEmail, password = pPassword },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error PersonDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return person;
        }

        public PersonEN VerifyPerson(PersonEN pPerson)
        {
            PersonEN person = new PersonEN();

            try
            {
                connection.Cnn.Open();
                person = connection.Cnn.Query<PersonEN>("SpVerifyVendorCredentials", new { email = pPerson.Email, passwordSalt = pPerson.Password },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("VerifyPerson : " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }
            return person;
        }

        public void UpdatePersonReferredStatus(int pStatus, string pEmail)
        {
            var query = connection.Cnn.Query<int>("SpUpdateUserReferredStatus", new { email = pEmail, @status = pStatus },
                 commandType: CommandType.StoredProcedure);
        }

        public int InsertGamerProfile(GamerProfileEN pProfile)
        {
            int inserted = default(int);

            try
            {
                connection.Cnn.Open();
                inserted = connection.Cnn.Insert(pProfile) ?? default(int);
            }
            catch (Exception ex)
            {
                inserted = 0;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("InsertGamerProfile: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return inserted;
        }

        public GamerProfileEN GetGamerProfileByNickname(string pNickname)
        {
            GamerProfileEN profile = new GamerProfileEN();

            try
            {
                connection.Cnn.Open();
                profile = connection.Cnn.Query<GamerProfileEN>("SpGamerProfileByNickname", new { nickname = pNickname.Trim() },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                profile = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetGamerProfileByNickname: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return profile;
        }

        public bool get_Master_ip_Available(int idmaster, string ip)
        {
            bool response = false;
           
            try
            {
                connection.Cnn.Open();
                var available = connection.Cnn.Query<int>("select count([AvailableIP]) from [Person].[AvailableIP] where [PersonMasterID]=@idmaster and [AvailableIP]=@ip and [Estado]=1", new { idmaster = idmaster, ip = ip }).FirstOrDefault();

                if (available > 0)
                {
                    response = true;
                }
                else
                {
                    response = false;
                }
            }
            catch (Exception)
            {
                response = false;

            }
            finally
            {
                connection.Cnn.Close();
            }
            return response;
        }

    }
}
