using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using YoVendoRecargaAPI.Entities;
using JamaaTech.Smpp.Net.Client;
using JamaaTech.Smpp.Net.Lib.Util;
using JamaaTech.Smpp.Net.Lib;
using JamaaTech.Smpp.Net.Lib.Protocol;
using YoVendoRecargaAPI.SVC.Models;
using System.Threading;


namespace YoVendoRecargaAPI.SVC
{
    public class SmsTelevida
    {
        static ISmppConfiguration smppConfig;
        public bool SendSMS(string pBody, string pTargetPhone)
        {
            bool result = false;
            string key = ConfigurationManager.AppSettings["keytelevida"].ToString();
            string url = ConfigurationManager.AppSettings["urltelevida"].ToString();
            String urlcompleta = url + "&to=" + pTargetPhone + "&message=" + pBody;
            String textResult = "";
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(urlcompleta);
                request.Method = "Post";
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream stream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(stream);
                textResult = streamReader.ReadToEnd();
                var serializer = new JavaScriptSerializer();
                var serializedResult = serializer.Deserialize<ResponseTelevida>(textResult);

                if (serializedResult.resultCode == "0" && (serializedResult.message == "Success" || serializedResult.message == "Success."))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        public bool SendSMSC(string pBody, string pTargetPhone)
        {
            bool result = false;

            try
            {
                smppConfig = GetSmppConfiguration();

                var client = CreateSmppClient(smppConfig);
                client.Start();
                
                // must wait until connected before start sending
                while (client.ConnectionState != SmppConnectionState.Connected)
                    Thread.Sleep(100);

                TextMessage msg = new TextMessage();

                msg.DestinationAddress = pTargetPhone; //Receipient number
                msg.SourceAddress = "4094949"; //Originating number
                msg.Text = pBody;
                msg.RegisterDeliveryNotification = true; //I want delivery notification for this message

                client.SendMessage(msg);

                result = true;
            }
            catch (Exception)
            {

                result = false;
            }

            return result;
        }

        private static ISmppConfiguration GetSmppConfiguration()
        {
            return new SmppConfiguration
            {
                TimeOut = 60000,
                StartAutomatically = true,
                Name = "MyLocalClient",
                SystemID = "GLBPAY01",
                Password = "GLBP9201",
                Host = "200.10.173.9",
                Port = 2775,
                SystemType = "5750",
                DefaultServiceType = "5750",
                SourceAddress = "5750",
                AutoReconnectDelay = 5000,
                KeepAliveInterval = 5000,
                ReconnectInteval = 10000,
                Encoding = JamaaTech.Smpp.Net.Lib.DataCoding.ASCII
            };
        }

        static SmppClient CreateSmppClient(ISmppConfiguration config)
        {
            var client = new SmppClient();
            client.Name = config.Name;
            client.ConnectionStateChanged += new EventHandler<ConnectionStateChangedEventArgs>(client_ConnectionStateChanged);
            client.StateChanged += new EventHandler<StateChangedEventArgs>(client_StateChanged);
            client.MessageSent += new EventHandler<MessageEventArgs>(client_MessageSent);
            client.MessageDelivered += new EventHandler<MessageEventArgs>(client_MessageDelivered);
            client.MessageReceived += new EventHandler<MessageEventArgs>(client_MessageReceived);

            SmppConnectionProperties properties = client.Properties;
            properties.SystemID = config.SystemID;// "mysystemid";
            properties.Password = config.Password;// "mypassword";
            properties.Port = config.Port;// 2034; //IP port to use
            properties.Host = config.Host;// "196.23.3.12"; //SMSC host name or IP Address
            properties.SystemType = config.SystemType;// "mysystemtype";
            properties.DefaultServiceType = config.DefaultServiceType;// "mydefaultservicetype";
            properties.DefaultEncoding = config.Encoding;

            //Resume a lost connection after 30 seconds
            client.AutoReconnectDelay = config.AutoReconnectDelay;

            //Send Enquire Link PDU every 15 seconds
            client.KeepAliveInterval = config.KeepAliveInterval;

            return client;
        }

        private static void client_ConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            var client = (SmppClient)sender;

            //if (client.LastException != null)
            //{

            //}

            switch (e.CurrentState)
            {
                case SmppConnectionState.Closed:
                    //Connection to the remote server is lost
                    //Do something here
                    {
                        e.ReconnectInteval = smppConfig.ReconnectInteval; //Try to reconnect after Interval in seconds
                        break;
                    }
                case SmppConnectionState.Connected:
                    //A successful connection has been established
                    break;
                case SmppConnectionState.Connecting:
                    //A connection attemp is still on progress
                    break;
            }

        }

        private static void client_StateChanged(object sender, StateChangedEventArgs e)
        {
            var client = (SmppClient)sender;
        }

        private static void client_MessageSent(object sender, MessageEventArgs e)
        {
            var client = (SmppClient)sender;
            // CANDO: save sent sms
        }

        private static void client_MessageDelivered(object sender, MessageEventArgs e)
        {
            var client = (SmppClient)sender;


            // CANDO: save delivered sms
        }

        private static void client_MessageReceived(object sender, MessageEventArgs e)
        {
            var client = (SmppClient)sender;
            TextMessage msg = e.ShortMessage as TextMessage;

            // CANDO: save received sms
        }
    }
}
