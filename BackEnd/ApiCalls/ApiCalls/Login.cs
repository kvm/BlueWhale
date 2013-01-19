using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Xml;
using ApiCalls.Configuration;
using System.Net.Http.Headers;
using System.Data;
namespace ApiCalls
{
    public class Login : Ilogin
    {
        private HttpClient userCheckClient;
        private int debug;
        private int writeLineMode;
        private HttpResponseMessage userCheckResponse;
        public Login(int debugmode,int WriteLineMode)
        {
            this.debug = debugmode;
            this.writeLineMode = WriteLineMode;
            this.userCheckClient = new HttpClient();
            this.userCheckClient.BaseAddress = CSConfig.BaseAddressLive;
            this.debug = debugmode;
        }

        public int Check_User_Exist(Credentials credential)
        {            
            this.userCheckClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            string requestString = "CheckUser/" + credential.ServiceUserId.ToString() + "/" + ((int)credential.service).ToString();
            this.userCheckResponse = userCheckClient.GetAsync(requestString).Result;            
            if (this.userCheckResponse.IsSuccessStatusCode)
            {
                if (this.writeLineMode == 1)
                Console.WriteLine(userCheckResponse.Content.ReadAsStringAsync().Result);
                XmlDocument document = new XmlDocument();
                //XmlReader xmlReader = new XmlReader(userCheckResponse.Content.ReadAsStreamAsync().Result);
                //xmlReader.
                //TODO: Add a try catch exception Block
                try
                {
                    document.Load(userCheckResponse.Content.ReadAsStreamAsync().Result);
                }
                catch (XmlException ex)
                {
                    return (int)ReturnTypes.ReturnCode.XmlException;
                }
                if (this.writeLineMode == 1)
                Console.WriteLine(document.DocumentElement.SelectSingleNode("/xml/IsPresent").InnerText);

                if (Int32.Parse(document.DocumentElement.SelectSingleNode("/xml/IsPresent").InnerText) == 1)
                    return (int)ReturnTypes.ReturnCode.UserExists;
                else return (int)ReturnTypes.ReturnCode.UserNotExists;
                
            }
            else
            {
                if (this.writeLineMode == 1)
                Console.WriteLine("{0} ({1})", (int)userCheckResponse.StatusCode, userCheckResponse.ReasonPhrase);
                return (int)ReturnTypes.ReturnCode.ServerError;
            }            
        }

        public int Create_User(Credentials credentials)
        {
            
            string requestString = "Createuser/" + credentials.ServiceUserId.ToString() + "/" +credentials. ServiceuserName + "/" + credentials.FirstName + "/" + credentials.LastName + "/" + ((int)credentials.service).ToString() + "/" + this.debug.ToString();
            userCheckResponse = userCheckClient.PostAsJsonAsync(requestString,"").Result;

            if (userCheckResponse .IsSuccessStatusCode)
            {
                if (this.writeLineMode == 1)
                Console.WriteLine(userCheckResponse.Content.ReadAsStringAsync().Result);
                XmlDocument document = new XmlDocument();
                document.Load(userCheckResponse.Content.ReadAsStreamAsync().Result);
                if (Int32.Parse(document.DocumentElement.SelectSingleNode("/xml/IsPresent").InnerText) == 1)
                    return (int)ReturnTypes.ReturnCode.UserAlreadyExists;
                if (Int32.Parse(document.DocumentElement.SelectSingleNode("/xml/Created").InnerText) == 0)
                    return (int)ReturnTypes.ReturnCode.UserCreationSuccessful;
                else return (int)ReturnTypes.ReturnCode.UnKnownError;
            }
            else
            {
                if (this.writeLineMode == 1)
                Console.WriteLine("{0} ({1})", (int)userCheckResponse.StatusCode, userCheckResponse.ReasonPhrase);
                return (int)ReturnTypes.ReturnCode.ServerError;
            }            
        }
    }
}
