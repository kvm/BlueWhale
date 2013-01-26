using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls.Configuration
{
    public static class CSConfig
    {
        public static Uri BaseAddressLocalHost
        {
            get;
            set;
        }
        public static Uri BaseAddressLive
        {
            get;
            set;
        }
       static CSConfig()
       { 

            BaseAddressLocalHost = new Uri("http://localhost/codestudio/index.php/users/");
            BaseAddressLive = new Uri("http://outlookinglife.com/phoenixlabs/CodeStudio/index.php/users/");
            
        }

    }
}
