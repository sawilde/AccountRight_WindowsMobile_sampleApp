using System;
using System.Runtime.Serialization;

namespace MYOB.Sample.Contracts
{
    /// <summary>
    /// http://developer.myob.com/docs/read/getting_started/Authentication
    /// </summary>
    [DataContract]
    public class OAuthResponse
    {
        private DateTime _receivedTime;

        public void MarkReceivedTime()
        {
            _receivedTime = DateTime.Now;
        }

        public bool HasExpired
        {
            get
            {
#if DEBUG
                return (_receivedTime + TimeSpan.FromSeconds(20)) < DateTime.Now;
#else
                return (_receivedTime + TimeSpan.FromSeconds(ExpiresIn-120)) < DateTime.Now;
#endif
            }
        }

        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "refresh_token")]
        public string RefreshToken { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        [DataMember(Name = "scope")]
        public string Scope { get; set; }
    }
}