using System.Collections.ObjectModel;
using System.ComponentModel;
using MYOB.Sample.Annotations;
using MYOB.Sample.Contracts;

namespace MYOB.Sample.ViewModels
{
    public class DisplayAccountsViewModel : INotifyPropertyChanged
    {
        public DisplayAccountsViewModel()
        {
            Accounts = new ObservableCollection<Account>();
        }

        private CompanyFileViewModel _companyFile;
        private bool _isLoading;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Account> Accounts { get; private set; }

        public CompanyFileViewModel CompanyFile
        {
            get { return _companyFile; }
            set
            {
                if (Equals(value, _companyFile)) return;
                _companyFile = value;
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
    }
}