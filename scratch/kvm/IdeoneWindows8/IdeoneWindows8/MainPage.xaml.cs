using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Facebook;
using Windows.Security.Authentication.Web;
using System.Dynamic;
using System.Text;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using System.Net.Http;
using IdeoneWindows8.Library;
using System.Net;
using Windows.Data.Html;
using Chq.OAuth;
using Chq.OAuth.Credentials;
using Chq.OAuth.Helpers;
using IdeoneWindows8.Library;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace IdeoneWindows8
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        private String m_PostResponse;

        DataBindingClass myDataSource = new Library.DataBindingClass();
       // Binding binding = new Binding();
       
        public MainPage()
        {
            this.InitializeComponent();

            DataContext = myDataSource;
            myDataSource.isIndeterminate = false;
            //binding.Source = myDataSource;
            //progressBar.SetBinding(ProgressBar.IsIndeterminateProperty, binding);
            //dataBinder.isIndeterminate = false;
            //progressBar.IsIndeterminate = false;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            myDataSource.isIndeterminate = false;
            //progressBar.IsIndeterminate = false;
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
                    throw new InvalidOperationException("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    // The user canceled the authentication
                }
            }
            catch (Exception ex)
            {
                //
                // Bad Parameter, SSL/TLS Errors and Network Unavailable errors are to be handled here.
                //
                throw ex;
            }
        }

        private async void LoginSucceded(string accessToken, FacebookClient _fb)
        {
            dynamic parameters = new ExpandoObject();
            
            myDataSource.isIndeterminate = true;

            //parameters.access_token = accessToken;
            //parameters.fields = "id";
            
            parameters = _fb.GetTaskAsync("me?fields=first_name");//,middle_name,last_name,username,email,picture");
            
            Frame.Navigate(typeof(HomePage), (object)parameters);
        }

        private async Task<String> PostData(String Url, String Data)
        {
            m_PostResponse = null;
            try
            {
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);
                Request.Method = "POST";
                Request.Headers["Authorization"] = Data;
                HttpWebResponse Response = (HttpWebResponse)await Request.GetResponseAsync();
                StreamReader ResponseDataStream = new StreamReader(Response.GetResponseStream());
                m_PostResponse = await ResponseDataStream.ReadToEndAsync();
            }
            catch (Exception Err)
            {
                //rootPage.NotifyUser("Error posting data to server." + Err.Message, NotifyType.StatusMessage);
            }

            return m_PostResponse;
        }

        private async void Image_Tapped_2(object sender, TappedRoutedEventArgs e)
        {
            //Make a POST Request and get the Request Token

            //set the variables required for the POST request

            TwitterOauth twitter = new TwitterOauth();
            twitter.oauth_callback = "https://www.facebook.com/connect/login_success.html";
            //twitter.oauth_callback = "";
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
            //Make the HTTP POST
            //twitter.ObtainRequestToken();
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

                m_PostResponse = await PostData(TwitterUrl, DataToPost);
                //DebugPrint("Received Data: " + m_PostResponse);

                if (m_PostResponse != null)
                {
                    String oauth_token = null;
                    String oauth_token_secret = null;
                    String[] keyValPairs = m_PostResponse.Split('&');

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

                  //      DebugPrint("Navigating to: " + TwitterUrl);

                        WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                                WebAuthenticationOptions.None,
                                                                StartUri,
                                                                EndUri);
                        if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                        {
                    //        OutputToken(WebAuthenticationResult.ResponseData.ToString());
                            // After authorization, the request token needs to be converted to an access token. 
                            // Please see twitter documentation for additional steps needed after authorization.
                            // See the PasswordVault sample, to store the oauth_token_secret there which can be 
                            // roamed across machines or retrieved when the app is launched the next time.
                        }
                        else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                        {
                      //      OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                        }
                        else
                        {
                        //    OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                        }
                    }
                }
                Frame.Navigate(typeof(HomePage));
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
                string googleClientId = "368616127369.apps.googleusercontent.com";
                string googleCallbackUrl = "http://localhost";
                String GoogleURL = "https://accounts.google.com/o/oauth2/auth?client_id=" + Uri.EscapeDataString(googleClientId) + "&redirect_uri=" + Uri.EscapeDataString(googleCallbackUrl) + "&response_type=code&scope=" + Uri.EscapeDataString("http://picasaweb.google.com/data");

                System.Uri StartUri = new Uri(GoogleURL);
                // When using the desktop flow, the success code is displayed in the html title of this end uri
                System.Uri EndUri = new Uri("https://accounts.google.com/o/oauth2/approval?");

                //DebugPrint("Navigating to: " + GoogleURL);

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None,
                                                        StartUri,
                                                        EndUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    //OutputToken(WebAuthenticationResult.ResponseData.ToString());
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    //OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    //OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
                Frame.Navigate(typeof(HomePage));
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
