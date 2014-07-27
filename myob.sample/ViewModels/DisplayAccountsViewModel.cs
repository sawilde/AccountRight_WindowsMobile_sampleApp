using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts.Version2;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.AccountRight.SDK.Contracts.Version2.GeneralLedger;
using MYOB.AccountRight.SDK.Contracts.Version2.Sale;
using MYOB.AccountRight.SDK.Services.Contact;
using MYOB.AccountRight.SDK.Services.GeneralLedger;
using MYOB.AccountRight.SDK.Services.Sale;
using MYOB.Sample.Annotations;
using Windows.Storage;

namespace MYOB.Sample.ViewModels
{
    public class DisplayAccountsViewModel : INotifyPropertyChanged
    {
        private readonly IOAuthKeyService _keyService;

        public DisplayAccountsViewModel(IOAuthKeyService keyService)
        {
            _keyService = keyService;
            Accounts = new ObservableCollection<Account>();
            Customers = new ObservableCollection<CustomerViewModel>();
            Invoices = new ObservableCollection<Invoice>();
            RawCustomers = new List<Contact>();
        }

        private CompanyFileViewModel _companyFileModel;
        private bool _isLoading;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Account> Accounts { get; private set; }
        public ObservableCollection<CustomerViewModel> Customers { get; private set; }
        public ObservableCollection<Invoice> Invoices { get; private set; }
        public List<Contact> RawCustomers { get; private set; }

        public CompanyFileViewModel CompanyFile
        {
            get { return _companyFileModel; }
            set
            {
                if (Equals(value, _companyFileModel)) return;
                _companyFileModel = value;
                OnPropertyChanged("CompanyFile");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (Equals(value, _isLoading)) return;
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void FetchData(Action<Exception> onError)
        {
            IsLoading = true;
            var service = new AccountService(new ApiConfiguration(), null, _keyService);
            var cService = new CustomerService(new ApiConfiguration(), null, _keyService);
            var iService = new ServiceInvoiceService(new ApiConfiguration(), null, _keyService);

            Task.WhenAll(new[]
                {
                    service.GetRangeAsync(CompanyFile.CompanyFile, null, CompanyFile.Authentication[0])
                        .ContinueWith(t => DisplayAccounts(t.Result.Items), TaskScheduler.FromCurrentSynchronizationContext()),
                    cService.GetRangeAsync(CompanyFile.CompanyFile, null, CompanyFile.Authentication[0])
                        .ContinueWith(t => DisplayCustomers(t.Result.Items), TaskScheduler.FromCurrentSynchronizationContext()),
                    iService.GetRangeAsync(CompanyFile.CompanyFile, null, CompanyFile.Authentication[0])
                        .ContinueWith(t => DisplayInvoices(t.Result.Items), TaskScheduler.FromCurrentSynchronizationContext())
                })
                .ContinueWith(t =>
                    {
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void DisplayCustomers(Customer[] collection)
        {
            Customers.Clear();
            RawCustomers.Clear();
            RawCustomers.AddRange(collection);
            foreach (var contact in collection.Maybe(_ => _, new Contact[0]))
            {
                var customer = new CustomerViewModel(_keyService)
                    {
                        Customer = contact,
                        CompanyFile = _companyFileModel.CompanyFile,
                        Credentials = new CompanyFileCredentials(CompanyFile.Authentication[0].Username, CompanyFile.Authentication[0].Password)
                    };
                Customers.Add(customer);
                customer.FetchPicture();
            }
        }

        public void DisplayInvoices(ServiceInvoice[] collection)
        {
            Invoices.Clear();
            foreach (var invoice in collection.Maybe(_ => _, new Invoice[0]))
            {
                Invoices.Add(invoice);
            }
        }

        public void DisplayAccounts(Account[] collection)
        {
            Accounts.Clear();
            foreach (var account in collection.Maybe(_ => _, new Account[0]).Where(a => !a.IsHeader))
            {
                Accounts.Add(account);
            }
        }

        public async void ShowPdf(Invoice invoice)
        {
            if (new Version(CompanyFile.CompanyFile.ProductVersion) < new Version("2013.4"))
            {
                MessageBox.Show("Company file does not support PDF creation!");
                return;
            }
            IsLoading = true;

            var pdfName = invoice.Number + ".pdf";
            var iService = new ServiceInvoiceService(new ApiConfiguration(), null, _keyService);

            try
            {
                var pdf =
                    await
                    iService.GetInvoiceFormAsPdfAsync(CompanyFile.CompanyFile, invoice.UID,
                                                      CompanyFile.Authentication[0], null);

                var localFolder = ApplicationData.Current.LocalFolder;
                var storageFile = await localFolder.CreateFileAsync(pdfName, CreationCollisionOption.ReplaceExisting);
                using (var outputStream = await storageFile.OpenStreamForWriteAsync())
                {
                    await pdf.CopyToAsync(outputStream);
                }
                Windows.System.Launcher.LaunchFileAsync(storageFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}