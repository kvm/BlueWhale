using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace IdeoneWindows8.Library
{
    class UserInfo
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string UserID { get; set; }
        public BitmapImage ProfileImage { get; set; }

        public UserInfo(dynamic parameter)
        {
            if (parameter.service != null)
            {
                IDictionary<string, object> idict = (IDictionary<string, object>)parameter;

                UserID = (idict.ContainsKey("id")) ? idict["id"].ToString() : "No UserID";
                EmailID = (idict.ContainsKey("email")) ? idict["email"].ToString() : "abc@example.com";
                UserName = (idict.ContainsKey("username")) ? idict["username"].ToString() : "johnsmith";
                if (idict.ContainsKey("picture"))
                {
                    ProfileImage = new BitmapImage(new Uri(idict["picture"].ToString()));
                }
                else
                {
                    ProfileImage = new BitmapImage(new Uri("ms-appx:/Resources/no_profile_image.png"));
                }
                if (String.Compare(parameter.service, "Facebook") == 0)
                {
                    try
                    {
                        FullName = String.Concat(parameter.first_name, parameter.middle_name, parameter.last_name);
                    }
                    catch
                    {

                    }
                }
                else if (String.Compare(parameter.service, "Twitter") == 0)
                {
                    if (idict.ContainsKey("first_name"))
                    {
                        FullName = parameter.first_name;
                    }
                }
                else if (String.Compare(parameter.service, "Google") == 0)
                {
                    FullName = (idict.ContainsKey("first_name") ? parameter.first_name : string.Empty) + (idict.ContainsKey("last_name") ? parameter.last_name : string.Empty);
                }
                if (String.IsNullOrEmpty(FullName))
                {
                    FullName = "No Name";
                }
            }
        }
    }
}
