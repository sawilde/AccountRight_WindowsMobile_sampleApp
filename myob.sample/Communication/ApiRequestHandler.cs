using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MYOB.Sample.Contracts;

namespace MYOB.Sample.Communication
{
    /// <summary>
    /// Make calls to the MYOB API by setting the appropriate headers
    /// </summary>
    public class ApiRequestHandler : BaseRequestHandler
    {
        private readonly OAuthResponse _oauth;
        private readonly string _accountRightBaseUri = "https://api.myob.com/accountright";

        public ApiRequestHandler(OAuthResponse oauth)
        {
            _oauth = oauth;
        }

        /// <summary>
        /// Get a list of Company Files
        /// </summary>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetCompanyFiles(Action<CompanyFile[]> onSuccess, Action onError)
        {
            var request = (HttpWebRequest)WebRequest.Create(_accountRightBaseUri);
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Bearer {0}", _oauth.AccessToken);
            request.Headers["x-myobapi-key"] = Configuration.ClientId;
            request.Headers["Accept-Encoding"] = "gzip";
            request.BeginGetResponse(HandleResponseCallback<RequestContext<string, CompanyFile[]>, string, CompanyFile[]>,
                new RequestContext<string, CompanyFile[]>
                {
                    Request = request,
                    OnSuccess = onSuccess,
                    OnError = onError
                });
        }

        /// <summary>
        /// Get a list of accounts
        /// </summary>
        /// <param name="companyFileId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onError"></param>
        public void GetAccounts(Guid companyFileId, string username, string password, Action<PagedCollection<Account>> onSuccess, Action onError)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}/account",_accountRightBaseUri, companyFileId));
            request.Headers[HttpRequestHeader.Authorization] = string.Format("Bearer {0}", _oauth.AccessToken);
            request.Headers["x-myobapi-key"] = Configuration.ClientId;
            request.Headers["x-myobapi-cftoken"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password)));
            request.Headers["Accept-Encoding"] = "gzip";
            request.BeginGetResponse(HandleResponseCallback<RequestContext<string, PagedCollection<Account>>, string, PagedCollection<Account>>,
                new RequestContext<string, PagedCollection<Account>>
                {
                    Request = request,
                    OnSuccess = onSuccess,
                    OnError = onError
                });
        }
    }
}
