using MYOB.Sample.Contracts;
using Microsoft.Phone.Controls;

namespace MYOB.Sample
{
    public abstract class SharedPhoneApplicationPage : PhoneApplicationPage
    {
        protected static OAuthResponse OAuthResponse { get; set; }
    }
}