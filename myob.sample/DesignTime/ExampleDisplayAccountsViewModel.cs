using System;
using MYOB.Sample.Contracts;
using MYOB.Sample.ViewModels;

namespace MYOB.Sample.DesignTime
{
    public class ExampleDisplayAccountsViewModel : DisplayAccountsViewModel
    {
        public ExampleDisplayAccountsViewModel()
        {
            this.IsLoading = false;
            this.CompanyFile = new CompanyFileViewModel()
                {
                    CompanyFile = new CompanyFile()
                        {
                            Name = "Sample Company File", 
                            ProductVersion = "2013.1", 
                            Id = Guid.NewGuid()
                        }
                };
            this.Accounts.Add(new Account()
                {
                    Name = "Main Account", 
                    CurrentAccountBalance = 100.00m
                });
            this.Accounts.Add(new Account()
                {
                    Name = "Second Account", 
                    CurrentAccountBalance = -100.00m
                });
            this.Accounts.Add(new Account()
                {
                    Name = "Savings Account", 
                    CurrentAccountBalance = 0.00m
                });

        }
    }
}