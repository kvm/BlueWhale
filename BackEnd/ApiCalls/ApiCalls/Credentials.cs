using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls
{
    public class Credentials
    {
        public enum ServiceName
        {   
            Google ,
            Facebook,
            Twitter
        }

        /// <summary>
        /// Initializes a object for class Credentials
        /// </summary>
        /// <param name="serviceUserName">Username given by Service</param>
        /// <param name="serviceuserID">UserId given by service</param>
        /// <param name="firstName">Firstname of user</param>
        /// <param name="lastName">Lastname of user</param>
        /// <param name="service">service type:google,fb,twitter ?</param>
        public Credentials(string serviceUserName,long serviceuserID,string firstName,string lastName,ServiceName service)
        {
            this.ServiceuserName = serviceUserName;
            this.service = service;
            this.ServiceUserId = serviceuserID;
            this.FirstName = firstName;
            this.LastName = lastName;
        }
        public Credentials()
        {
            this.ServiceuserName = "kvm";
            this.service = ServiceName.Facebook;
            this.ServiceUserId = 123;
            this.FirstName = "ankur";
            this.LastName = "khurana";
        }
        public string ServiceuserName
        {
            get;
            set;
        }
        public long ServiceUserId
        {
            get;
            set;
        }
        public string FirstName{
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }
        public ServiceName service
        {
            get;
            set;
        }

    }
}
