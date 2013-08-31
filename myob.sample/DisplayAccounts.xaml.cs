using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Navigation;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.AccountRight.SDK.Contracts.Version2.GeneralLedger;
using MYOB.Sample.UserControls;
using MYOB.Sample.ViewModels;

namespace MYOB.Sample
{
    public partial class DisplayAccounts : SharedPhoneApplicationPage
    {
        public DisplayAccountsViewModel ViewModel { get; private set; }

        public DisplayAccounts()
        {
            InitializeComponent();
            ViewModel = new DisplayAccountsViewModel(this);
            DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (ViewModel.CompanyFile == null && State.Keys.Contains("CompanyFile")) // resuming from tombstoning
            {
                ViewModel.CompanyFile = new CompanyFileViewModel() { CompanyFile = (CompanyFile)State["CompanyFile"] };
                ViewModel.CompanyFile.Authentication.Add(new CompanyFileAuthenticationViewModel());
                ViewModel.CompanyFile.Authentication[0].Username = (string)State["Username"];
                ViewModel.CompanyFile.Authentication[0].Password = (string)State["Password"];
            }
            
            if (ViewModel.Accounts.Count == 0)
            {
                ViewModel.FetchData(ShowError);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            State["CompanyFile"] = ViewModel.CompanyFile.CompanyFile;
            State["Username"] = ViewModel.CompanyFile.Authentication[0].Username;
            State["Password"] = ViewModel.CompanyFile.Authentication[0].Password;

            base.OnNavigatingFrom(e);
        }


        private static void ShowError(Exception ex)
        {
            MessageBox.Show(string.Format("An Error Occured: {0}", ex.Message));
        }

        private void Refresh_OnClick(object sender, EventArgs e)
        {
            if (ViewModel.IsLoading) return;
            ViewModel.FetchData(ShowError);
        }

        private CustomerViewModel _selectedCustomer;
        private void ListCustomers_OnCustomerClicked(object sender, CustomerClickedEventArgs e)
        {
            if (e.Customer == null) return;
            _selectedCustomer = e.Customer;
            NavigationService.Navigate(new Uri("/ViewCustomer.xaml", UriKind.Relative));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (e.Content is ViewCustomer)
            {
                var view = e.Content as ViewCustomer;
                view.ViewModel.CompanyFile = _selectedCustomer.CompanyFile;
                view.ViewModel.Customer = _selectedCustomer.Customer;
                view.ViewModel.Credentials = _selectedCustomer.Credentials;
            }
            base.OnNavigatedFrom(e);
        }

    }
}