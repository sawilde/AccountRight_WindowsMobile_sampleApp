using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MYOB.Sample.Annotations;
using MYOB.Sample.Contracts;

namespace MYOB.Sample.ViewModels
{
    public class CompanyFileViewModel : INotifyPropertyChanged
    {
        public class CompanyFileAuthenticationViewModel : INotifyPropertyChanged
        {
            private string _username;
            private string _password;
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

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public CompanyFileViewModel()
        {
            Authentication = new List<CompanyFileAuthenticationViewModel>(){new CompanyFileAuthenticationViewModel(){Username = "Administrator"}};
        }

        private CompanyFile _companyFile;
        private bool _isExpanded;
        public event PropertyChangedEventHandler PropertyChanged;

        public CompanyFile CompanyFile
        {
            get { return _companyFile; }
            set
            {
                if (Equals(value, _companyFile)) return;
                _companyFile = value;
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
