using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls
{
    /// <summary>
    /// Interface for log In class to Contact ,Used By  Client Side
    /// </summary>
    interface Ilogin
    {
        /// <summary>
        /// Check if the given user exists or not
        /// </summary>
        /// <returns>
        /// integer depending upon the result -1 for server error
        /// 0 for 
        /// </returns>
        int Check_User_Exist(Credentials credentials);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="username"></param>
        /// <param name="firstname"></param>
        /// <param name="service"></param>
        /// <param name="FbPicUrl"></param>
        /// <returns></returns>
//        int Create_User(int userid,string username,string firstname,string service,Uri FbPicUrl);



    }
}
