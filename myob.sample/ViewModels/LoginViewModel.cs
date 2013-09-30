using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Communication;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Services;
using MYOB.Sample.Annotations;

namespace MYOB.Sample.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly IOAuthKeyService _keyService;

        public LoginViewModel(IOAuthKeyService keyService)
        {
            _keyService = keyService;
            _showBrowser = true;
            CompanyFiles = new ObservableCollection<CompanyFileViewModel>();
        }

        private bool _showBrowser;
        private bool _isLoading;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool ShowBrowser
        {
            get { return _showBrowser; }
            set
            {
                if (value.Equals(_showBrowser)) return;
                _showBrowser = value;
                OnPropertyChanged("ShowBrowser");
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

        public void FetchCompanyFies(Action<Exception> onError)
        {
            CompanyFiles.Clear();
            IsLoading = true;
            ShowBrowser = false;

            var service = new CompanyFileService(new ApiConfiguration(), new WebRequestFactory(), _keyService);
            service.GetRangeAsync()
                   .ContinueWith(t =>
                       {
                           if (t.IsFaulted)
                           {
                               onError(t.Exception);
                               ShowBrowser = true;
                               IsLoading = false;
                           }
                           else
                           {
                               CompanyFiles.Clear();
                               foreach (var companyFile in t.Result)
                               {
                                   CompanyFiles.Add(new CompanyFileViewModel {CompanyFile = companyFile});
                               }
                               IsLoading = false;
                           }
                       }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public ObservableCollection<CompanyFileViewModel> CompanyFiles { get; private set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CollapseAllBut(CompanyFileViewModel selectedItem)
        {
            foreach (var companyFileViewModel in CompanyFiles.Where(companyFileViewModel => selectedItem != companyFileViewModel))
            {
                companyFileViewModel.IsExpanded = false;
            }
        }
    }
}