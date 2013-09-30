using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.AccountRight.SDK.Services.Contact;
using MYOB.Sample.Annotations;

namespace MYOB.Sample.ViewModels
{
    public class CustomerViewModel : INotifyPropertyChanged
    {
        private Contact _customer;
        private CompanyFile _companyFile;
        private readonly IOAuthKeyService _keyService;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isLoading;

        private static readonly BitmapImage Default = new BitmapImage(new Uri(@"/Images/Man_silhouette-gray.svg.png", UriKind.Relative));

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public CustomerViewModel(IOAuthKeyService keyService)
        {
            _keyService = keyService;
            _picture = Default;
        }

        public CompanyFileCredentials Credentials { get; set; }

        public CompanyFile CompanyFile
        {
            get { return _companyFile; }
            set { 
                if (_companyFile == value) return;
                _companyFile = value;
                OnPropertyChanged("CompanyFile");
            }
        }

        public Contact Customer
        {
            get { return _customer; }
            set
            {
                if (_customer == value) return;
                _customer = value;
                OnPropertyChanged("Customer");
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

        private BitmapImage _picture;
        public BitmapImage Picture
        {
            get { return _picture; }
            set
            {
                if (_picture == value) return;
                _picture = value;
                OnPropertyChanged("Picture");
            }
        }

        public void FetchPicture()
        {
            if (_customer.Maybe(_ => _.PhotoURI) == null) return;
            var service = new CustomerService(new ApiConfiguration(), null, _keyService);
            service.GetPhotoAsync(_companyFile, _customer.UID, Credentials)
                   .ContinueWith(t =>
                       {
                           using (var stream = new MemoryStream(t.Result))
                           {
                               var bi = new BitmapImage();
                               bi.SetSource(stream);
                               Picture = bi;
                           }
                       }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Refresh()
        {
            if (_customer == null) return;
            IsLoading = true;
            var service = new CustomerService(new ApiConfiguration(), null, _keyService);
            service.GetAsync(_companyFile, _customer.UID, Credentials)
                   .ContinueWith(t =>
                       {
                           if (!t.IsCompleted) return;
                           Customer = t.Result;
                           FetchPicture();
                       }, TaskScheduler.FromCurrentSynchronizationContext())
                   .ContinueWith(t =>
                       {
                           IsLoading = false;
                       }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void UploadPicture(MemoryStream chosenPhoto) 
        {
            if (_customer == null) return;
            IsLoading = true;
            var data = chosenPhoto.ToArray();
            var service = new CustomerService(new ApiConfiguration(), null, _keyService);
            service.SavePhotoAsync(_companyFile, _customer.UID, data, Credentials)
                   .ContinueWith(t =>
                       {
                           IsLoading = false;
                       }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void DeletePhoto()
        {
            if (_customer == null) return;
            var service = new CustomerService(new ApiConfiguration(), null, _keyService);
            service.DeletePhotoAsync(_companyFile, _customer.UID, Credentials)
                .ContinueWith(t =>
                    {
                        Picture = Default;
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
