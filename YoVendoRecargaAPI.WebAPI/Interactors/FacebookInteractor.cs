using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class FacebookInteractor
    {
        public SimpleTextResponse ValidateRequest(UserFacebookReqBody pRequest)
        {
            SimpleTextResponse validation = new SimpleTextResponse();

            try
            {
                if (pRequest.email == null)
                {
                    validation.result = false;
                    validation.Message = "Email must not be empty";
                }
                else if (pRequest.facebookURL == null)
                {
                    validation.result = false;
                    validation.Message = "FacebookURL must not be empty";
                }
                else if (String.IsNullOrEmpty(pRequest.firstname))
                {
                    validation.result = false;
                    validation.Message = "Firstname is required";
                }
                else if (String.IsNullOrEmpty(pRequest.lastname))
                {
                    validation.result = false;
                    validation.Message = "Lastname is required";
                }
                else if (pRequest.middlename == null)
                {
                    validation.result = false;
                    validation.Message = "Middlename field must not be an empty string";
                }
                else if (pRequest.PersonID == null)
                {
                    validation.result = false;
                    validation.Message = "PersonID is required";
                }
                else if (pRequest.PersonID <= 0)
                {
                    validation.result = false;
                    validation.Message = "PersonID must be greater than zero";
                }
                else if (pRequest.phoneNumber == null)
                {
                    validation.result = false;
                    validation.Message = "Phonenumber field must not be an empty string";
                }
                else if (pRequest.profileID == null)
                {
                    validation.result = false;
                    validation.Message = "ProfileID is required";
                }
                else if (pRequest.userID == null)
                {
                    validation.result = false;
                    validation.Message = "ProfileID is required";
                }
                else
                {
                    validation.result = true;
                    validation.Message = "Accpeted";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return validation;
        }
    }
}