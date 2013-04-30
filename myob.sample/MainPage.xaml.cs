using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MYOB.Sample.Annotations;
using MYOB.Sample.Communication;
using MYOB.Sample.Contracts;
using MYOB.Sample.ViewModels;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace MYOB.Sample
{
    public partial class MainPage : SharedPhoneApplicationPage
    {
        private readonly Uri _authorizeUri = new Uri(
            string.Format("https://secure.myob.com/oauth2/account/authorize?client_id={0}&redirect_uri={1}&scope=CompanyFile&response_type=code",
                Configuration.ClientId, Uri.EscapeDataString(Configuration.RequestUri)));

        private readonly Uri _logoffUri = new Uri("https://secure.myob.com/oauth2/account/logoff");
        private readonly Uri _loginUri = new Uri("https://secure.myob.com/oauth2/account/login");

        private readonly LoginViewModel _viewModel;

        public CompanyFileViewModel CompanyFile { get; set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            OAuthLogin.Navigated += OAuthLogin_Navigated;
            _viewModel = new LoginViewModel();
            this.DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (OAuthResponse != null)
            {
                _viewModel.ShowBrowser = false;
                if (_viewModel.CompanyFiles.Count == 0)
                {
                    Refresh_OnClick(this, null);
                }
            }
            else if (_viewModel.ShowBrowser)
            {
                _viewModel.CompanyFiles.Clear();
                OAuthLogin.Navigate(_authorizeUri);                
            }
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Content is DisplayAccounts)
            {
                var da = e.Content as DisplayAccounts;
                da.ViewModel.CompanyFile = CompanyFile;
            }
            base.OnNavigatedFrom(e);
        }

        void OAuthLogin_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            Debug.WriteLine(e.Uri);
            if (!string.Equals(e.Uri.ToString(), _authorizeUri.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                if (string.Equals(e.Uri.ToString(), _loginUri.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    OAuthLogin.Navigate(_authorizeUri);
                }
                return;
            }

            var content = OAuthLogin.SaveToString();
            var regex = new Regex(@"\<title\>(.+?)=(.+?)\</title\>");
            var match = regex.Match(content);
            if (!match.Success || match.Groups.Count != 3) return;

            switch (match.Groups[1].Value.ToLowerInvariant())
            { 
                case "code": // we have a code
                    var code = match.Groups[2].Value;
                    var oauthRequest = new OAuthRequestHandler();
                    oauthRequest.GetOAuthTokens(code, HandleOauthResponse, ShowError);
                    break;

                case "error": // user probably said "no thanks"
                    OAuthLogin.Navigate(_logoffUri);
                    break;
            }
        }

        private void HandleOauthResponse(OAuthResponse oauth)
        {
            OAuthResponse = oauth;
            Dispatcher.BeginInvoke(() =>
                {
                    _viewModel.CompanyFiles.Clear();
                    _viewModel.IsLoading = true;
                    _viewModel.ShowBrowser = false;
                });

            var apiRequest = new ApiRequestHandler(OAuthResponse);
            apiRequest.GetCompanyFiles(DisplayCompanyFiles, ShowError);
        }

        private void DisplayCompanyFiles(CompanyFile[] companyFiles)
        {
            Dispatcher.BeginInvoke(() =>
                {
                    _viewModel.CompanyFiles.Clear();
                    foreach (var companyFile in companyFiles)
                    {
                        _viewModel.CompanyFiles.Add(new CompanyFileViewModel{CompanyFile = companyFile});
                    }
                    _viewModel.IsLoading = false;
                });
        }

        private void ShowError()
        {
            Dispatcher.BeginInvoke(() =>
                {
                    MessageBox.Show("An Error Occured: Try Again");
                    _viewModel.IsLoading = false;
                    _viewModel.ShowBrowser = true;
                    OAuthLogin.Navigate(_logoffUri);
                });
        }

        private void Logoff_OnClick(object sender, EventArgs e)
        {
            if (_viewModel.IsLoading) return;
            OAuthResponse = null;
            _viewModel.ShowBrowser = true;
            OAuthLogin.Navigate(_logoffUri);
        }

        private void CompanyFiles_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.CollapseAllBut((CompanyFileViewModel) CompanyFiles.SelectedItem);
        }

        private void CompanyFileLogin_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = (CompanyFileViewModel)CompanyFiles.SelectedItem;
            CompanyFile = selectedItem;
            NavigationService.Navigate(new Uri("/DisplayAccounts.xaml", UriKind.Relative));
        }

        private void Refresh_OnClick(object sender, EventArgs e)
        {
            if (_viewModel.ShowBrowser) return;
            if (_viewModel.IsLoading) return;
            _viewModel.IsLoading = true;

            var oauthRequest = new OAuthRequestHandler();
            oauthRequest.RenewOAuthTokens(OAuthResponse, HandleOauthResponse, ShowError);
        }
    }
}