using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace IdeoneWindows8
{
    enum SocialSite
    {
        FB = 1,
        TWITTER,
        GOOGLE
    }
    interface OAuthBase
    {
        /// <summary>
        /// Generates the LoginUrl
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="site"></param>
        /// <returns></returns>
        string GetLoginUrl(dynamic obj);
    }
    public class TwitterOauth:OAuthBase
    {
        public string oauth_callback
        {
            get;
            set;
        }

        public string oauth_consumer_key
        {
            get;
            set;
        }

        public string oauth_nonce
        {
            get;
            set;
        }

        public string oauth_signature
        {
            get;
            set;
        }

        public string oauth_signature_method
        {
            get;
            set;
        }

        public string oauth_timestamp
        {
            get;
            set;
        }

        public string oauth_version
        {
            get;
            set;
        }

        public string oauth_token
        {
            get;
            set;
        }

        public string oauth_secret
        {
            get;
            set;
        }

        public string oauth_consumer_secret
        {
            get;
            set;
        }

        public string request_uri
        {
            get;
            set;
        }

        public string authorize_uri { get; set; }

        public async void ObtainRequestToken()
        {
            var req = new HttpClient();

            StringBuilder str = new StringBuilder();
            str.Append("OAuth ");
            str.Append("oauth_nonce=").Append("\"").Append(oauth_nonce).Append("\",");
            str.Append("oauth_callback=").Append("\"").Append(oauth_callback).Append("\",");
            str.Append("oauth_signature_method=").Append("\"").Append(oauth_signature_method).Append("\",");
            str.Append("oauth_timestamp=").Append("\"").Append(oauth_timestamp).Append("\",");
            str.Append("oauth_consumer_key=").Append("\"").Append(oauth_consumer_key).Append("\",");
            str.Append("oauth_signature=").Append("\"").Append(oauth_signature).Append("\",");
            str.Append("oauth_version=").Append("\"").Append(oauth_version).Append("\"");

            req.DefaultRequestHeaders.Add("Authorization",str.ToString());

            HttpResponseMessage H = await req.PostAsync(request_uri, new FormUrlEncodedContent(new Dictionary<string, string>()));

            //return (Task<HttpResponseMessage>)H;
        }
        public string GetLoginUrl(dynamic obj)
        {
            string LoginUrl = "";

            LoginUrl = "https://api.twitter.com/oauth/authenticate?oauth_token=" + obj.oauth_token;

            return LoginUrl;
        }

        public async Task<HttpResponseMessage> GetAccessToken(dynamic obj)
        {
            var req = new HttpClient();

            StringBuilder str = new StringBuilder();
            str.Append("OAuth ");
            str.Append("oauth_consumer_key,").Append(oauth_consumer_key);
            str.Append("oauth_nonce,").Append(oauth_nonce);
            str.Append("oauth_signature,").Append(oauth_signature);
            str.Append("oauth_signature_method,").Append(oauth_signature_method);
            str.Append("oauth_timestamp,").Append(oauth_timestamp);
            str.Append("oauth_token,").Append(oauth_token);
            str.Append("oauth_version,").Append(oauth_version);

            req.DefaultRequestHeaders.Add("Authorization", str.ToString());

            return await req.PostAsync(request_uri, new FormUrlEncodedContent(new Dictionary<string, string>()));
        }
    }
}
