using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using MYOB.Sample.Annotations;

namespace MYOB.Sample.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public LoginViewModel()
        {
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