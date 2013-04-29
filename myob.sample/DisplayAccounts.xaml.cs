using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MYOB.Sample.Annotations;
using MYOB.Sample.Communication;
using MYOB.Sample.Contracts;
using MYOB.Sample.ViewModels;

namespace MYOB.Sample
{
    public partial class DisplayAccounts : SharedPhoneApplicationPage
    {
        public DisplayAccountsViewModel ViewModel { get; private set; }

        public DisplayAccounts()
        {
            InitializeComponent();
            ViewModel = new DisplayAccountsViewModel();
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BeginOauthRequest();
            ViewModel.IsLoading = true;
            base.OnNavigatedTo(e);
        }

        private void BeginOauthRequest()
        {
            var oauthRequest = new OAuthRequestHandler();
            oauthRequest.RenewOAuthTokens(OAuthResponse, HandleOauthResponse, ShowError);
        }

        private void HandleOauthResponse(OAuthResponse oauth)
        {
            OAuthResponse = oauth;
            Dispatcher.BeginInvoke(() =>
            {
                ViewModel.IsLoading = true;
            });

            var request = new ApiRequestHandler(OAuthResponse);
            request.GetAccounts(ViewModel.CompanyFile.CompanyFile.Id,
                                ViewModel.CompanyFile.Authentication[0].Username,
                                ViewModel.CompanyFile.Authentication[0].Password, DisplayCompanyAccounts, ShowError);
        }

        private void DisplayCompanyAccounts(PagedCollection<Account> accounts)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    ViewModel.Accounts.Clear();
                    foreach (var account in accounts.Items.Where(a => !a.IsHeader))
                    {
                        ViewModel.Accounts.Add(account);
                    }
                    ViewModel.IsLoading = false;
                });
        }

        private void ShowError()
        {
            Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show("An Error Occured: Try Again");
                ViewModel.IsLoading = false;
            });
        }

        private void Refresh_OnClick(object sender, EventArgs e)
        {
            if (ViewModel.IsLoading) return;
            ViewModel.IsLoading = true;

            BeginOauthRequest();
        }
    }
}