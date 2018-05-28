using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill;

namespace YoVendoRecargaAPI.BL
{
    public class EmailSender
    {
        public MandrillApi apimd = new MandrillApi("XUnoP_FmZIHp4S_Ij7mVEg", false);

        public String EmailAccountActivation(String email, String url, String name)
        {
            string result = "error";
            try
            {
                UserInfo user = new UserInfo();
                Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("Correo_verificacion");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga.com"
                    ,
                    auto_text = false
                    ,
                    subject = "Activar Cuenta"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(email, name, "to") }
                    ,
                    bcc_address = "cbolanos@globalpay.us"
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                };
                msg.AddGlobalVariable("NAME", name);
                msg.AddGlobalVariable("LINK", url);

                apimd.SendMessage(msg, template.name, null);
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }

            return result;
        }


        public String EmailTransferConfirmationReceived(String pUserEmail, String pNameDepositor, String pReferenceNumber, String pBank, String pAmount, String pUserNames, String pDate)
        {

            string result = "error";
            try
            {
                UserInfo user = new UserInfo();
                Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("Confirmación_transferencia_recibida");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga.com"
                    ,
                    auto_text = false
                    ,
                    subject = "Transferencia YoVendoRecarga"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(pUserEmail, pUserNames, "to") }
                    ,
                    bcc_address = "depositos@globalpay.us"
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                };
                msg.AddGlobalVariable("NUMBER", pReferenceNumber);
                msg.AddGlobalVariable("BANK", pBank);
                msg.AddGlobalVariable("AMOUNT", "$ " + pAmount);
                msg.AddGlobalVariable("DATE", pDate);
                msg.AddGlobalVariable("NAME", pNameDepositor);
                msg.AddGlobalVariable("USER", pUserNames);
                apimd.SendMessage(msg, template.name, null);
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }

            return result;
        }

        public String EmailBalanceAppliedSuccessfully(String email, String name)
        {
            string result = "error";
            try
            {
                UserInfo user = new UserInfo();
                Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("Saldo_aplicado_exitosamente ");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga.com"
                    ,
                    auto_text = false
                    ,
                    subject = "Saldo Aplicado"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(email, name, "to") }
                    ,
                    bcc_address = "depositos@globalpay.us"
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                };
                msg.AddGlobalVariable("NAME", name);

                apimd.SendMessage(msg, template.name, null);
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }

            return result;
        }

        public String EmailUserProfile100(String email, String name, String code)
        {
            string result = "error";
            try
            {

                UserInfo user = new UserInfo();
                Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("Saldo_aplicado_exitosamente ");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga.com"
                    ,
                    auto_text = false
                    ,
                    subject = "Saldo Aplicado"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(email, name, "to") }
                    ,
                    bcc_address = "cbolanos@globalpay.us"
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                };
                msg.AddGlobalVariable("NAME", name);
                msg.AddGlobalVariable("CODE", code);
                msg.AddGlobalVariable("EMAIL", email);
                apimd.SendMessage(msg, template.name, null);
                result = "exito";
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }

            return result;
        }

        public String EmailRestorePassword(String email, String url, String name)
        {
            string result = "error";
            try
            {
                UserInfo user = new UserInfo();
                Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("Restablecer_contraseña");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga.com"
                    ,
                    auto_text = false
                    ,
                    subject = "Reestablecer Contraseña"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(email, name, "to") }
                    ,
                    bcc_address = "cbolanos@globalpay.us"
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                };
                msg.AddGlobalVariable("NAME", name);
                msg.AddGlobalVariable("LINK", url);

                apimd.SendMessage(msg, template.name, null);
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }

            return result;
        }


        public String EmailNewLogin(String name, String email, String deviceInfo, DateTime dateLogin, String deviceIP)
        {
            string result = "error";

            try
            {
                UserInfo user = new UserInfo();
                Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("multiples_dispositivos");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga.com"
                    ,
                    auto_text = false
                    ,
                    subject = "Nuevo Inicio de Sesión"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(email, name, "to") }
                    ,
                    bcc_address = "cbolanos@globalpay.us"
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                };

                msg.AddGlobalVariable("NAME", name);
                msg.AddGlobalVariable("EMAIL", email);
                msg.AddGlobalVariable("MODELO", deviceInfo);
                msg.AddGlobalVariable("DATE", dateLogin.ToString("dd/MM/yy"));
                msg.AddGlobalVariable("TIME", dateLogin.ToString("hh:mm:ss tt"));
                msg.AddGlobalVariable("ADDRESS", deviceIP);

                apimd.SendMessage(msg, template.name, null);
                result = "success";
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }

            return result;
        }

        public String EmailRequestBalance(String masterEmail, String sellerName, String masterName)
        {
            string result = "error";
            try
            {
                UserInfo user = new UserInfo();
                Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("Solicitud_vendedor_a_master");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga.com"
                    ,
                    auto_text = false
                    ,
                    subject = "Solicitud de saldo"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(masterEmail, masterName, "to") }
                    ,
                    bcc_address = "cbolanos@globalpay.us"
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                };
                msg.AddGlobalVariable("NAME", sellerName);
                msg.AddGlobalVariable("USER", masterName);
                msg.AddGlobalVariable("LINK", "https://yovendorecarga.com/SV/Account/LogIn");
                apimd.SendMessage(msg, template.name, null);
                result = "exito";
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }

            return result;
        }

        public String EmailReferred(String email, String name, String phone, String issue)
        {
            string result = "error";

            try
            {
                UserInfo user = new UserInfo();
                //Mandrill.Models.TemplateInfo template = apimd.TemplateInfo("FRM_CONTACTO");
                Mandrill.EmailMessage msg = new Mandrill.EmailMessage
                {
                    from_email = "info@yovendorecarga.com"
                    ,
                    from_name = "YoVendoRecarga"
                    ,
                    auto_text = false
                    ,
                    subject = "Únete a nosotros"
                    ,
                    to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress(email, email, "to") }
                        //to = new List<Mandrill.EmailAddress> { new Mandrill.EmailAddress("cbolanos@globalpay.us", "cbolanos@globalpay.us", "to"), new Mandrill.EmailAddress("geovannymartinez_20@hotmail.com", "Carlos Geovanny", "to") }
                    ,
                    merge_language = "mailchimp"
                    ,
                    merge = true
                    ,
                    bcc_address = "cbolanos@globalpay.us"
                    ,
                    text = name + " " + "quiere que te descargues la aplicación de YoVendoRecarga.com! Para descargarla y comenzar a vender recargas crea una cuenta usando el siguiente enlace: http://www.yovendorecarga.com/SV/Account/Registrate"

                };

                apimd.SendMessage(msg);
                result = "exito";
            }
            catch (Exception)
            {
                result = "error";
                throw;
            }


            return result;
        }
    }
}
