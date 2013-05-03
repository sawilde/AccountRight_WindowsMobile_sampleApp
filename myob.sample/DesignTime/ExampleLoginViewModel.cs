using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MYOB.Sample.Contracts;
using MYOB.Sample.ViewModels;

namespace MYOB.Sample.DesignTime
{
    public class ExampleLoginViewModel : LoginViewModel
    {
        public ExampleLoginViewModel()
        {
            this.IsLoading = false;
            this.ShowBrowser = false;
            this.CompanyFiles.Add(new CompanyFileViewModel()
                {
                    IsExpanded = false, 
                    CompanyFile = new CompanyFile()
                        {
                            Name = "Sample Company File", 
                            ProductVersion = "2013.1", 
                            Id = Guid.NewGuid()
                        }
                });
            this.CompanyFiles.Add(new CompanyFileViewModel()
                {
                    IsExpanded = true, 
                    CompanyFile = new CompanyFile()
                        {
                            Name = "Sample Company File", 
                            ProductVersion = "2013.1", 
                            Id = Guid.NewGuid()
                        }
                });
        }
    }
}
