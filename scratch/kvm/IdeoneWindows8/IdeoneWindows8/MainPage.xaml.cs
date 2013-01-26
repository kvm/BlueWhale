#region Namespaces

using Chq.OAuth;
using Chq.OAuth.Credentials;
using Chq.OAuth.Helpers;
using Facebook;
using IdeoneWindows8.Library;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Html;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

#endregion

namespace IdeoneWindows8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private String PostResponse;

        DataBindingClass myDataSource = new Library.DataBindingClass();
       
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = myDataSource;
            myDataSource.isIndeterminate = false;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            myDataSource.isIndeterminate = false;
        }

        private void Image_GotFocus_1(object sender, RoutedEventArgs e)
        {
            ((Image)sender).Opacity = 0.8;
        }

        private void Image_PointerEntered_1(object sender, PointerRoutedEventArgs e)
        {
            ((Image)sender).Opacity = 0.8;
        }

        private void Image_PointerExited_1(object sender, PointerRoutedEventArgs e)
        {
            ((Image)sender).Opacity = 1.0;
        }

        private void Image_PointerEntered_2(object sender, PointerRoutedEventArgs e)
        {
            ((Image)sender).Opacity = 0.8;
        }

        private void Image_PointerExited_2(object sender, PointerRoutedEventArgs e)
        {
            ((Image)sender).Opacity = 1.0;
        }

        private void Image_PointerEntered_3(object sender, PointerRoutedEventArgs e)
        {
            ((Image)sender).Opacity = 0.8;
        }

        private void Image_PointerExited_3(object sender, PointerRoutedEventArgs e)
        {
            ((Image)sender).Opacity = 1.0;
        }

        private async void Image_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            myDataSource.isIndeterminate = true;

            string _facebookAppId = "135066233237101";// You must set your own AppId here
            string _permissions = "user_about_me,email"; // Set your permissions here

            FacebookClient _fb = new FacebookClient();

            var redirectUrl = "https://www.facebook.com/connect/login_success.html";
            try
            {
                //fb.AppId = facebookAppId;
                var loginUrl = _fb.GetLoginUrl(new
                {
                    client_id = _facebookAppId,
                    redirect_uri = redirectUrl,
                    scope = _permissions,
                    display = "popup",
                    response_type = "token"
                });

                var endUri = new Uri(redirectUrl);

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None,
                                                        loginUrl,
                                                        endUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    var callbackUri = new Uri(WebAuthenticationResult.ResponseData.ToString());
                    var facebookOAuthResult = _fb.ParseOAuthCallbackUrl(callbackUri);
                    var accessToken = facebookOAuthResult.AccessToken;
                    if (String.IsNullOrEmpty(accessToken))
                    {
                        // User is not logged in, they may have canceled the login
                    }
                    else
                    {
                        // User is logged in and token was returned
                        LoginSucceded(accessToken,_fb);
                    }
                    
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    myDataSource.isIndeterminate = false;
                    var messageBox = new MessageDialog("Could not connect to the network.Please check your internet connection");
                    messageBox.Commands.Add(new UICommand("OK"));
                    messageBox.DefaultCommandIndex = 0;
                    await messageBox.ShowAsync();
                    //TO DO : Write a stub to push error logs onto the database
                }
                else
                {
                    myDataSource.isIndeterminate = false;
                    // The user canceled the authentication
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void LoginSucceded(string accessToken, FacebookClient _fb)
        {
            dynamic parameters = new ExpandoObject();
            parameters.access_token = accessToken;
            parameters.fields = "id";
            _fb.AccessToken = accessToken;

            dynamic result = await _fb.GetTaskAsync("me");
            dynamic picture = await _fb.GetTaskAsync("me?fields=picture");
            result.picture = picture.picture.data.url;
            result.service = "Facebook";
            UserInfo user = new UserInfo(result);

            Frame.Navigate(typeof(HomePage), (object)user);
        }

        private async Task<String> PostData(String Url, String Data, bool isPOST)
        {
            PostResponse = null;
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
                if (isPOST)
                    Request.Method = "POST";
                else
                    Request.Method = "GET";
                Request.Headers["Authorization"] = Data;
                
                HttpWebResponse Response = (HttpWebResponse)await Request.GetResponseAsync();
                StreamReader ResponseDataStream = new StreamReader(Response.GetResponseStream());
                PostResponse = await ResponseDataStream.ReadToEndAsync();
            }
            catch (Exception Err)
            {
                //rootPage.NotifyUser("Error posting data to server." + Err.Message, NotifyType.StatusMessage);
            }

            return PostResponse;
        }

        private async void Image_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            myDataSource.isIndeterminate = true;

            //Make a POST Request and get the Request Token
            TwitterOauth twitter = new TwitterOauth();
            twitter.oauth_callback = "https://www.facebook.com/connect/login_success.html";
            twitter.oauth_consumer_key = "iNpGqyW7YR6tEGCi8jQw";
            twitter.oauth_consumer_secret = "yIu7O2de1ohn4Wn6TIMoi1PRjMFtuYIlLbzO6lDgJMY";
            twitter.oauth_nonce = Convert.ToBase64String(
                                  Encoding.UTF8.GetBytes(
                                       DateTime.Now.Ticks.ToString()));
            TimeSpan timespan = DateTime.UtcNow
                                  - new DateTime(1970, 1, 1, 0, 0, 0, 0,
                                       DateTimeKind.Utc);
            twitter.oauth_timestamp = Convert.ToInt64(timespan.TotalSeconds).ToString();
            twitter.request_uri = "https://api.twitter.com/oauth/request_token";
            twitter.oauth_version = "1.0";
            twitter.oauth_signature_method = "HMAC-SHA1";

            //Compute Key  and BaseString for implementing the SCHA1 Algorithm
            var compositeKey = string.Concat(WebUtility.UrlEncode(twitter.oauth_consumer_secret),
                         "&");
            string baseString,baseFormat;
            baseFormat = "oauth_callback={0}&oauth_consumer_key={1}&oauth_nonce={2}&oauth_signature={3}" +
                "&oauth_signature_method={4}&oauth_timestamp={5}&oauth_version={6}";
            baseString = String.Format(baseFormat, twitter.oauth_callback, twitter.oauth_consumer_key,
                                        twitter.oauth_nonce, twitter.oauth_signature, twitter.oauth_signature_method,
                                        twitter.oauth_timestamp, twitter.oauth_version);
            baseString = String.Concat("POST&", WebUtility.UrlEncode(twitter.request_uri),"&", WebUtility.UrlEncode(baseString));
            twitter.oauth_signature = Utils.HmacSha1(baseString, compositeKey);
            twitter.authorize_uri = "https://api.twitter.com/oauth/authorize";


            try
            {
                //
                // Acquiring a request token
                //
                TimeSpan SinceEpoch = (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
                Random Rand = new Random();
                String TwitterUrl = "https://api.twitter.com/oauth/request_token";
                Int32 Nonce = Rand.Next(1000000000);
                //
                // Compute base signature string and sign it.
                //    This is a common operation that is required for all requests even after the token is obtained.
                //    Parameters need to be sorted in alphabetical order
                //    Keys and values should be URL Encoded.
                //
                String SigBaseStringParams = "oauth_callback=" + Uri.EscapeDataString(twitter.oauth_callback);
                SigBaseStringParams += "&" + "oauth_consumer_key=" + twitter.oauth_consumer_key;
                SigBaseStringParams += "&" + "oauth_nonce=" + Nonce.ToString();
                SigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
                SigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(SinceEpoch.TotalSeconds);
                SigBaseStringParams += "&" + "oauth_version=1.0";
                String SigBaseString = "POST&";
                SigBaseString += Uri.EscapeDataString(TwitterUrl) + "&" + Uri.EscapeDataString(SigBaseStringParams);

                IBuffer KeyMaterial = CryptographicBuffer.ConvertStringToBinary(twitter.oauth_consumer_secret + "&", BinaryStringEncoding.Utf8);
                MacAlgorithmProvider HmacSha1Provider = MacAlgorithmProvider.OpenAlgorithm("HMAC_SHA1");
                CryptographicKey MacKey = HmacSha1Provider.CreateKey(KeyMaterial);
                IBuffer DataToBeSigned = CryptographicBuffer.ConvertStringToBinary(SigBaseString, BinaryStringEncoding.Utf8);
                IBuffer SignatureBuffer = CryptographicEngine.Sign(MacKey, DataToBeSigned);
                String Signature = CryptographicBuffer.EncodeToBase64String(SignatureBuffer);
                String DataToPost = "OAuth oauth_callback=\"" + Uri.EscapeDataString(twitter.oauth_callback) + "\", oauth_consumer_key=\"" + twitter.oauth_consumer_key + "\", oauth_nonce=\"" + Nonce.ToString() + "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + Math.Round(SinceEpoch.TotalSeconds) + "\", oauth_version=\"1.0\", oauth_signature=\"" + Uri.EscapeDataString(Signature) + "\"";

                PostResponse = await PostData(TwitterUrl, DataToPost, true);

                if (PostResponse != null)
                {
                    String oauth_token = null;
                    String oauth_token_secret = null;
                    String[] keyValPairs = PostResponse.Split('&');

                    for (int i = 0; i < keyValPairs.Length; i++)
                    {
                        String[] splits = keyValPairs[i].Split('=');
                        switch (splits[0])
                        {
                            case "oauth_token":
                                oauth_token = splits[1];
                                break;
                            case "oauth_token_secret":
                                oauth_token_secret = splits[1];
                                break;
                        }
                    }

                    if (oauth_token != null)
                    {

                        TwitterUrl = "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth_token;
                        System.Uri StartUri = new Uri(TwitterUrl);
                        System.Uri EndUri = new Uri(twitter.oauth_callback);

                        WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                                WebAuthenticationOptions.None,
                                                                StartUri,
                                                                EndUri);
                        if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                        {
                            string oauth_verifier = "";
                            string oauth_token2 = "";
                            string webResponseString = WebAuthenticationResult.ResponseData.ToString();
                            
                            int index_oauth = webResponseString.IndexOf("oauth_token");

                            int i = index_oauth;
                            while (webResponseString[i] != '=')
                            {
                                i++;
                            }
                            i++;

                            while (webResponseString[i] != '&')
                            {
                                oauth_token2 += webResponseString[i];
                                i++;
                            }

                            while (webResponseString[i] != '=')
                            {
                                i++;
                            }
                            i++;
                            
                            while (i != webResponseString.Length)
                            {
                                oauth_verifier += webResponseString[i];
                                i++;
                            }

                            //Get the access token by making a POST request

                            string access_token_post_url = "https://api.twitter.com/oauth/access_token?oauth_verifier=" + oauth_verifier;

                            Signature = Utils.HmacSha1(baseString, compositeKey + oauth_token_secret);

                            string postData = "OAuth oauth_consumer_key=\"" + twitter.oauth_consumer_key + "\", oauth_nonce=\"" + Nonce.ToString() + "\", oauth_signature=\"" + Uri.EscapeDataString(Signature) + "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" +  Math.Round(SinceEpoch.TotalSeconds) + "\", oauth_token=\"" + oauth_token2 + "\", oauth_version=\"1.0\"";

                            string postResponsedata = await PostData(access_token_post_url, postData,true);

                            string accessToken = "", accessSecretKey = "", userID = "", screenName = "";

                            List<string> tokenList = new List<string>(postResponsedata.Split('&'));

                            foreach (string tokenNameValue in tokenList)
                            {
                                List<string> singleTokenList = new List<string>(tokenNameValue.Split('='));
                                if (String.Compare(singleTokenList[0], "oauth_token") == 0)
                                {
                                    accessToken = singleTokenList[1];
                                }
                                else if(String.Compare(singleTokenList[0],"oauth_token_secret") == 0)
                                {
                                    accessSecretKey = singleTokenList[1];
                                }
                                else if (String.Compare(singleTokenList[0], "user_id") == 0)
                                {
                                    userID = singleTokenList[1];
                                }
                                else if (String.Compare(singleTokenList[0], "screen_name") == 0)
                                {
                                    screenName = singleTokenList[1];
                                }

                            }

                            //Getting the user details
                            string userDetailsUrl = "https://api.twitter.com/1.1/account/verify_credentials.json";

                            SigBaseStringParams = "oauth_consumer_key=" + twitter.oauth_consumer_key;
                            SigBaseStringParams += "&" + "oauth_nonce=" + Nonce.ToString();
                            SigBaseStringParams += "&" + "oauth_signature_method=HMAC-SHA1";
                            SigBaseStringParams += "&" + "oauth_timestamp=" + Math.Round(SinceEpoch.TotalSeconds);
                            SigBaseStringParams += "&" + "oauth_token=" + accessToken;
                            SigBaseStringParams += "&" + "oauth_version=1.0";
                            SigBaseString = "GET&";
                            SigBaseString += Uri.EscapeDataString(userDetailsUrl) + "&" + Uri.EscapeDataString(SigBaseStringParams);


                            Signature = Utils.HmacSha1(SigBaseStringParams, compositeKey + accessSecretKey);

                            postData = "OAuth oauth_consumer_key=\"" + twitter.oauth_consumer_key + "\", oauth_nonce=\"" + Nonce.ToString() + "\", oauth_signature=\"" + Uri.EscapeDataString(Signature) + "\", oauth_signature_method=\"HMAC-SHA1\", oauth_timestamp=\"" + Math.Round(SinceEpoch.TotalSeconds) + "\", oauth_token=\"" + accessToken + "\", oauth_version=\"1.0\"";

                            var context = new OAuthContext(twitter.oauth_consumer_key, twitter.oauth_consumer_secret, "https://api.twitter.com/oauth/request_token", "https://api.twitter.com/oauth/authorize", "https://api.twitter.com/oauth/access_token", twitter.oauth_callback);

                            var client = new Client(context);

                            client.AccessToken = new TokenContainer();

                            client.AccessToken.Token = accessToken;
                            client.AccessToken.Secret = accessSecretKey;

                            String postResponse = await client.MakeRequest("GET")
                                .WithFormEncodedData(new {}) //this will be sent as a key/value in the request body an is included in the OAuth signature
                                .ForResource(client.AccessToken.Token,new Uri("https://api.twitter.com/1.1/account/verify_credentials.json"))
                                .Sign(client.AccessToken.Secret)
                                .ExecuteRequest();


                            Newtonsoft.Json.Linq.JObject twitterUserInfoJObject = Newtonsoft.Json.Linq.JObject.Parse(postResponse);

                            dynamic parameter = new ExpandoObject();
                            parameter.id = twitterUserInfoJObject["id"];
                            parameter.username = twitterUserInfoJObject["screen_name"];
                            parameter.first_name = twitterUserInfoJObject["name"];
                            parameter.picture = twitterUserInfoJObject["profile_image_url_https"];
                            parameter.service = "Twitter";
                            UserInfo user = new UserInfo(parameter);

                            Frame.Navigate(typeof(HomePage), (object) user);
                        }
                        else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                        {
                              myDataSource.isIndeterminate = false;
                              var messageBox = new MessageDialog("Could not connect to the network.Please check your internet connections");
                              messageBox.Commands.Add(new UICommand("OK"));
                              messageBox.DefaultCommandIndex = 0;
                              await messageBox.ShowAsync();
                      //      OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                        }
                        else
                        {
                              myDataSource.isIndeterminate = false;
                        //    OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                        }
                    }
                }
                
            }
            catch (Exception Error)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //
                //DebugPrint(Error.ToString());
            }

        }

        private async void Image_Tapped_3(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                myDataSource.isIndeterminate = true;
                string googleAccessToken = String.Empty;
                string googleClientId = "368616127369.apps.googleusercontent.com";
                string googleClientSecret = "aGXFskgLTG52-DG8uxYN60cJ";
                string googleCallbackUrl = "http://localhost";
                string GoogleURL = String.Concat("https://accounts.google.com/o/oauth2/auth?",
                        "scope=https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.email+https%3A%2F%2Fwww.googleapis.com%2Fauth%2Fuserinfo.profile&",
                        "state=%2Fprofile&",
                        "redirect_uri=" , Uri.EscapeDataString(googleCallbackUrl) , "&",
                        "response_type=code&",
                        "client_id=" , Uri.EscapeDataString(googleClientId));
                
                System.Uri StartUri = new Uri(GoogleURL);
                System.Uri EndUri = new Uri("http://localhost");

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None,
                                                        StartUri,
                                                        EndUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string responseCode = WebAuthenticationResult.ResponseData.ToString();

                    string googleCode = responseCode.Substring(responseCode.IndexOf("code") + 5);

                    string queryStringFormat = @"code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code";
                    string postcontents = string.Format(queryStringFormat
                                                       , Uri.EscapeDataString(googleCode)
                                                       , Uri.EscapeDataString(googleClientId)
                                                       , Uri.EscapeDataString(googleClientSecret)
                                                       , Uri.EscapeDataString(googleCallbackUrl));
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://accounts.google.com/o/oauth2/token");
                    request.Method = "POST";
                    byte[] postcontentsArray = Encoding.UTF8.GetBytes(postcontents);
                    request.ContentType = "application/x-www-form-urlencoded";
                    using (Stream requestStream = await request.GetRequestStreamAsync())
                    {
                        requestStream.Write(postcontentsArray, 0, postcontentsArray.Length);
                        WebResponse response = await request.GetResponseAsync();
                        using (Stream responseStream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseFromServer = reader.ReadToEnd();
                            if (responseFromServer.Length != 0)
                            {
                               List<string> commaSplitTokens = new List<string>(responseFromServer.Split(new char[] { ',' }));
                               List<string> colenSplitTokens = new List<string>(commaSplitTokens[0].Split(new char[] { ':' }));
                               googleAccessToken = colenSplitTokens[1].Substring(2, colenSplitTokens[1].Length - 3);
                            }
                        }
                    }

                    request = (HttpWebRequest)WebRequest.Create(@"https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + googleAccessToken);
                    request.Method = "GET";
                    WebResponse webResponse = await request.GetResponseAsync();
                    Stream responseStream1 = webResponse.GetResponseStream();
                    StreamReader reader1 = new StreamReader(responseStream1);
                    Newtonsoft.Json.Linq.JObject googleUserInfoJObject = Newtonsoft.Json.Linq.JObject.Parse(reader1.ReadToEnd());

                    dynamic parameter = new ExpandoObject();
                    parameter.id = googleUserInfoJObject["id"].ToString();
                    parameter.username = googleUserInfoJObject["email"].ToString().Substring(0, googleUserInfoJObject["email"].ToString().IndexOf('@'));
                    parameter.email = googleUserInfoJObject["email"].ToString();
                    parameter.picture = googleUserInfoJObject["image"]["url"].ToString();
                    parameter.first_name = googleUserInfoJObject["given_name"].ToString();
                    parameter.last_name = googleUserInfoJObject["family_name"].ToString();
                    parameter.service = "Google";

                    UserInfo user = new UserInfo(parameter);
                    Frame.Navigate(typeof(HomePage),(object) user);
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    myDataSource.isIndeterminate = false;
                    var messageBox = new MessageDialog("Could not connect to the network.Please check your internet connections");
                    messageBox.Commands.Add(new UICommand("OK"));
                    messageBox.DefaultCommandIndex = 0;
                    await messageBox.ShowAsync();
                }
                else
                {
                    //OutputToken("Error returned by AuthenticateeaAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //
                //DebugPrint(Error.ToString());
            }
        }
    }
}
