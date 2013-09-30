using System;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.AccountRight.SDK.Contracts.Version2.GeneralLedger;
using MYOB.Sample.ViewModels;

namespace MYOB.Sample.DesignTime
{
    public class ExampleDisplayAccountsViewModel : DisplayAccountsViewModel
    {
        public ExampleDisplayAccountsViewModel(): base(null)
        {
            this.IsLoading = false;
            this.CompanyFile = new CompanyFileViewModel()
                {
                    CompanyFile = new CompanyFile()
                        {
                            Name = "Sample Company File", 
                            ProductVersion = "2013.2", 
                            Id = Guid.NewGuid()
                        }
                };
            this.Accounts.Add(new Account()
                {
                    Name = "Main Account",
                    CurrentBalance = 100.00m
                });
            this.Accounts.Add(new Account()
                {
                    Name = "Second Account",
                    CurrentBalance = -100.00m
                });
            this.Accounts.Add(new Account()
                {
                    Name = "Savings Account",
                    CurrentBalance = 0.00m
                });
            this.Customers.Add(new CustomerViewModel(null) { Customer = new Customer { CompanyName = "MYOB", CurrentBalance = 0, IsIndividual = false } });
            this.Customers.Add(new CustomerViewModel(null) { Customer = new Customer() { FirstName = "Shaun", LastName = "Wilde", CurrentBalance = 100.0m, IsIndividual = true } });

        }
    }
}