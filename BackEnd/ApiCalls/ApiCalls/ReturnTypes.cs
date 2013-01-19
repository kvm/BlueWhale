using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls
{   
    /// <summary>
    /// Class Containing enum for return codes
    /// </summary>
    public class ReturnTypes
    {
        /// <summary>
        /// Contains Return Codes for Functions.
        /// Positive means operation was successful.
        /// negative means operation was unsuccessful.
        /// </summary>
        public enum ReturnCode
        {
            UserCreationSuccessful = 1 ,
            UserExists = 2 ,
            UserCreationUnsuccessful = -1,
            UserNotExists = -2,
            UserAlreadyExists = -3,
            ServerError = -500,
            UnKnownError = -1001,
            XmlException = -1002

        }
    }
}
