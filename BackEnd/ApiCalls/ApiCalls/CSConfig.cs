using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls.Configuration
{
    public static class CSConfig
    {
        public static Uri BaseAddress
        {
            get;
            set;
        }
       static CSConfig()
       { 
            BaseAddress = new Uri("http://localhost/codestudio/index.php/blog/");
        }

    }
}
