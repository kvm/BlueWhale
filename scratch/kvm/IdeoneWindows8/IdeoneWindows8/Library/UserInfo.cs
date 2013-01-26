using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdeoneWindows8.Library
{
    class UserInfo
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string UserID { get; set; }
        public string PhotoUrl { get; set; }

        public UserInfo(dynamic parameter)
        {
            if (parameter.service != null)
            {
                if (String.Compare(parameter.service, "Facebook") == 0)
                {
                    UserID = parameter.id;
                    FullName = String.Concat(parameter.first_name, parameter.middle_name, parameter.last_name);
                    EmailID = parameter.email;
                    UserName = parameter.username;
                    PhotoUrl = parameter.picture;
                }
                else if (String.Compare(parameter.service, "Twitter") == 0)
                {
                    UserID = parameter.id;
                    FullName = parameter.first_name;
                    UserName = parameter.username;
                    PhotoUrl = parameter.picture;
                    EmailID = null;
                }
                else if (String.Compare(parameter.service, "Google") == 0)
                {
                    UserID = parameter.id;
                    FullName = parameter.first_name + parameter.last_name;
                    UserName = parameter.username;
                    PhotoUrl = parameter.picture;
                    EmailID = parameter.email;
                }
            }
        }
    }
}
