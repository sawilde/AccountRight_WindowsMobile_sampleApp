using System;
using System.IO;
using System.Net;
using MYOB.Sample.Contracts;

namespace MYOB.Sample.Communication
{
    public class OAuthRequestHandler : BaseRequestHandler
    {
        private readonly Uri _oauthRequestUri = new Uri("https://secure.myob.com/oauth2/v1/authorize");
        private readonly HttpWebRequest _request;

        public OAuthRequestHandler()
        {
            _request = (HttpWebRequest)WebRequest.Create(_oauthRequestUri);
        }

        /// <summary>
        /// Start the process of getting the OAuth tokens using the code received from the authorization site
        /// </summary>
        /// <param name="code">The code received from the authorization site</param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetOAuthTokens(string code, Action<OAuthResponse> onSuccess, Action onError)
        {
            var data = string.Format("client_id={0}&client_secret={1}&redirect_uri={2}&scope=CompanyFile&code={3}&grant_type=authorization_code",
                                     Configuration.ClientId, Configuration.ClientSecret, Configuration.RequestUri, code);

            BeginRequest(onSuccess, onError, data);
        }

        /// <summary>
        /// Renew the OAuth tokens if the current tokens have expired
        /// </summary>
        /// <param name="oAuthResponse">The current OAuth response object</param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void RenewOAuthTokens(OAuthResponse oAuthResponse, Action<OAuthResponse> onSuccess, Action onError)
        {
            if (oAuthResponse.HasExpired)
            {
                var data = string.Format("client_id={0}&client_secret={1}&refresh_token={2}&grant_type=refresh_token",
                                         Configuration.ClientId, Configuration.ClientSecret, oAuthResponse.RefreshToken);

                BeginRequest(onSuccess, onError, data);
            }
            else
            {
                onSuccess(oAuthResponse);
            }
        }

        private void BeginRequest(Action<OAuthResponse> onSuccess, Action onError, string data)
        {
            _request.Method = "POST";
            _request.ContentType = "application/x-www-form-urlencoded";
            _request.BeginGetRequestStream(HandleRequestCallback,
                                           new RequestContext<string, OAuthResponse>
                                               {
                                                   Body = data,
                                                   Request = _request,
                                                   OnSuccess = onSuccess,
                                                   OnError = onError
                                               });
        }

        protected static void HandleRequestCallback(IAsyncResult asynchronousResult)
        {
            var requestData = (RequestContext<string, OAuthResponse>)asynchronousResult.AsyncState;

            var request = requestData.Request;
            var requestStream = request.EndGetRequestStream(asynchronousResult);
            using (var sw = new StreamWriter(requestStream))
            {
                sw.Write(requestData.Body);
            }
            requestStream.Close();
            request.BeginGetResponse(HandleResponseCallback<RequestContext<string, OAuthResponse>, string, OAuthResponse>, requestData);
        }
    }
}