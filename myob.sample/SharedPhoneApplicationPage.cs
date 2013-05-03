using MYOB.Sample.Contracts;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MYOB.Sample
{
    public abstract class SharedPhoneApplicationPage : PhoneApplicationPage
    {
        protected static OAuthResponse OAuthResponse
        {
            get
            {
                var store = PhoneApplicationService.Current.State;
                object value;
                store.TryGetValue("OAuthToken", out value);
                return value as OAuthResponse;
            }
            set
            {
                var store = PhoneApplicationService.Current.State;
                store["OAuthToken"] = value;
            }
        }
    }
}