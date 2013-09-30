using System;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.Sample.Settings;
using MYOB.Sample.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MYOB.Sample
{
    public abstract class SharedPhoneApplicationPage : PhoneApplicationPage, IOAuthKeyService
    {
        static readonly SettingsProvider SettingsProvider = new SettingsProvider();
        protected static OAuthTokens OAuthResponse
        {
            get
            {
                lock (SettingsProvider)
                {
                    return SettingsProvider.GetSettings<OAuthTokens>();
                }
            }
            set
            {
                lock (SettingsProvider)
                {
                    SettingsProvider.ResetToDefaults<OAuthTokens>();
                    if (value != null)
                        SettingsProvider.SaveSettings(value);
                }
            }
        }

        OAuthTokens IOAuthKeyService.OAuthResponse
        {
            get { return OAuthResponse; }
            set { OAuthResponse = value; }
        }
    }
}