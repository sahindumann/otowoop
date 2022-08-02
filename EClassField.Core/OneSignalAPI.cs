using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EClassField.Core
{
    public class OneSignalAPI
    {




        public static void SendMessage(string message, List<string> usersIDs)
        {
    

           
            var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;

            request.KeepAlive = true;
            request.Method = "POST";
            request.ContentType = "application/json";

            request.Headers.Add("authorization", "Basic NWZiY2FjZjMtOTMyYy00MjZiLTljZjEtMzQzODg0ZjMzZmFk");

            var serializer = new JavaScriptSerializer();
            var obj = new
            {
                app_id = "797d35dd-a929-470d-ae76-4d77a57c6e33",
                contents = new { en = message },
                data = new { MEsaj = "" },
                include_player_ids = usersIDs
            };
            var param = serializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(param);

            string responseContent = null;

            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
            }

            System.Diagnostics.Debug.WriteLine(responseContent); ;



        }

        public static void SendMessage(string messsage, int userID)

        {

         
        
            throw new NotImplementedException();
        }
    }
}
