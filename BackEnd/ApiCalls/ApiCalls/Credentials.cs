using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCalls
{
    class Credentials
    {

        public Credentials()
        {
            this.name = "ankur";
            this.service = "facebook";
            this.serviceUserId = 123;
        }
        public string name
        {
            get;
            set;
        }
        public string service
        {
            get;
            set;
        }
        public long serviceUserId{
            get;
            set;
        }


    }
}
