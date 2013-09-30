using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.Sample.ViewModels;

namespace MYOB.Sample.DesignTime
{
    public class ExampleLoginViewModel : LoginViewModel 
    {
        public ExampleLoginViewModel() : base(null)
        {
            this.IsLoading = false;
            this.ShowBrowser = false;
            this.CompanyFiles.Add(new CompanyFileViewModel()
                {
                    IsExpanded = true,
                    CompanyFile = new CompanyFile()
                        {
                            Name = "Sample Company File",
                            ProductVersion = "2013.3",
                            Id = Guid.NewGuid()
                        }
                });
            this.CompanyFiles.Add(new CompanyFileViewModel()
                {
                    IsExpanded = true,
                    CompanyFile = new CompanyFile()
                        {
                            Name = "Sample Company File",
                            ProductVersion = "2013.2",
                            Id = Guid.NewGuid()
                        }
                });
        }
    }
}
