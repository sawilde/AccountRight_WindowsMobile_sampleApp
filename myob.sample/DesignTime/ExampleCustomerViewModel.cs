using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.Sample.ViewModels;

namespace MYOB.Sample.DesignTime
{
    public class ExampleCustomerViewModel : CustomerViewModel
    {
        public ExampleCustomerViewModel()
            : base(null)
        {
            Customer = new Customer()
                {
                    CompanyName = "MI6",
                    FirstName = "James",
                    LastName = "Bond",
                    IsIndividual = false,
                    CurrentBalance = 125.27m
                };
            CompanyFile = new CompanyFile() {Name = "Sample Company"};
        }
    }
}