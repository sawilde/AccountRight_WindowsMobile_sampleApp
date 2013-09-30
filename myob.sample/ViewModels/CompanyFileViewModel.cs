using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.Sample.Annotations;

namespace MYOB.Sample.ViewModels
{
    public class CompanyFileAuthenticationViewModel : INotifyPropertyChanged, ICompanyFileCredentials
    {
        private string _username;
        private string _password;
        private bool _canAuthenticate;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Username
        {
            get { return _username; }
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public bool CanAuthenticate
        {
            get { return _canAuthenticate; }
            set
            {
                if (value == _canAuthenticate) return;
                _canAuthenticate = value;
                OnPropertyChanged("CanAuthenticate");
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CompanyFileViewModel : INotifyPropertyChanged
    {
        public CompanyFileViewModel()
        {
            Authentication = new List<CompanyFileAuthenticationViewModel>()
                {
                    new CompanyFileAuthenticationViewModel() {Username = "Administrator"}
                };
        }

        private CompanyFile _companyFile;
        private bool _isExpanded;
        public event PropertyChangedEventHandler PropertyChanged;

        public CompanyFile CompanyFile
        {
            get { return _companyFile; }
            set
            {
                if (value == _companyFile) return;
                _companyFile = value;
                Authentication.First().CanAuthenticate = _companyFile != null && new Version(_companyFile.ProductVersion) >= new Version("2013.3");
                OnPropertyChanged("CompanyFile");
            }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value.Equals(_isExpanded)) return;
                _isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public IList<CompanyFileAuthenticationViewModel> Authentication { get; private set; }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
