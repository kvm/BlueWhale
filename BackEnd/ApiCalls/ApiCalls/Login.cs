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
        private HttpResponseMessage userCheckResponse;
        public Login(int debugmode)
        {
            this.userCheckClient = new HttpClient();
            this.userCheckClient.BaseAddress = CSConfig.BaseAddress;
            this.debug = debugmode;
        }

        public int Check_User_Exist(Credentials credential)
        {            
            this.userCheckClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            string requestString = "CheckUser/" + credential.ServiceUserId.ToString() + "/" + ((int)credential.service).ToString();
            this.userCheckResponse = userCheckClient.GetAsync(requestString).Result;            
            if (this.userCheckResponse.IsSuccessStatusCode)
            {
                if(this.debug == 1)
                Console.WriteLine(userCheckResponse.Content.ReadAsStringAsync().Result);
                XmlDocument document = new XmlDocument();
                //XmlReader xmlReader = new XmlReader(userCheckResponse.Content.ReadAsStreamAsync().Result);
                //xmlReader.

                document.Load(userCheckResponse.Content.ReadAsStreamAsync().Result);
                if(this.debug == 1)
                Console.WriteLine(document.DocumentElement.SelectSingleNode("/xml/IsPresent").InnerText);

                if (Int32.Parse(document.DocumentElement.SelectSingleNode("/xml/IsPresent").InnerText) == 1)
                    return (int)ReturnTypes.ReturnCode.UserExists;
                else return (int)ReturnTypes.ReturnCode.UserNotExists;
                
            }
            else
            {
                if (this.debug == 1)
                Console.WriteLine("{0} ({1})", (int)userCheckResponse.StatusCode, userCheckResponse.ReasonPhrase);
                return (int)ReturnTypes.ReturnCode.ServerError;
            }            
        }

        public int Create_User(Credentials credentials)
        {
            
            string requestString = "Createuser/" + credentials.ServiceUserId.ToString() + "/" +credentials. ServiceuserName + "/" + credentials.FirstName + "/" + credentials.LastName + "/" + ((int)credentials.service).ToString();
            userCheckResponse = userCheckClient.PostAsJsonAsync(requestString,"").Result;

            if (userCheckResponse .IsSuccessStatusCode)
            {
                if (this.debug == 1)
                Console.WriteLine(userCheckResponse.Content.ReadAsStringAsync().Result);
                return (int)ReturnTypes.ReturnCode.UserCreationSuccessful;
                //TODO:Add return type for already existing user
            }
            else
            {
                if (this.debug == 1)
                Console.WriteLine("{0} ({1})", (int)userCheckResponse.StatusCode, userCheckResponse.ReasonPhrase);
                return (int)ReturnTypes.ReturnCode.ServerError;
            }            
        }
    }
}
