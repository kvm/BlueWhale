using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using ApiCalls.Configuration;
using System.Net.Http.Headers;
using System.Data;
namespace ApiCalls
{
    public class Login : Ilogin
    {
        private HttpClient userCheckClient;
        private HttpResponseMessage userCheckResponse;
        public Login()
        {
            userCheckClient = new HttpClient();
            userCheckClient.BaseAddress = CSConfig.BaseAddress;
        }

        public int Check_User_Exist(long userid, string username, string service)
        {

            
            userCheckClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            userCheckResponse = userCheckClient.GetAsync("comments").Result;
            
            if (userCheckResponse.IsSuccessStatusCode)
            {
                // Parse the response body. Blocking!
                DataSet dst = new DataSet();
                //dst.ReadXml(userCheckResponse.Content.ReadAsStreamAsync(), XmlReadMode.ReadSchema); 
                Console.WriteLine(userCheckResponse.Content.ReadAsStringAsync().Result);
                
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)userCheckResponse.StatusCode, userCheckResponse.ReasonPhrase);
            }
            return 0;
        }

        public int Create_User(long serviceUserId,string username,string firstName,string lastName)
        {
            Credentials cred = new Credentials();
            string requestString = "Createuser/"+serviceUserId.ToString()+"/"+username+"/"+firstName+"/"+lastName;
            userCheckResponse = userCheckClient.PostAsJsonAsync(requestString,cred).Result;

            if (userCheckResponse .IsSuccessStatusCode)
            {
                Uri newuri = userCheckResponse.Headers.Location;
                Console.WriteLine(userCheckResponse.Content.ReadAsStringAsync().Result);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)userCheckResponse.StatusCode, userCheckResponse.ReasonPhrase);
            }
            return 0;
        }
    }
}
