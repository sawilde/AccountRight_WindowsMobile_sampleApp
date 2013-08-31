using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MYOB.Sample.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MYOB.Sample.UserControls
{
    public class CustomerClickedEventArgs : EventArgs
    {
        public CustomerClickedEventArgs(CustomerViewModel customer)
        {
            Customer = customer;
        }

        public CustomerViewModel Customer { get; set; }
    }

    public partial class ListCustomers : UserControl
    {
        public ListCustomers()
        {
            InitializeComponent();
        }

        public event EventHandler<CustomerClickedEventArgs> CustomerClicked;

        private void Customer_Tap(object sender, GestureEventArgs e)
        {
            var box = sender as ListBox;
            if (box == null) return;
            CustomerClicked(sender, new CustomerClickedEventArgs(box.SelectedItem as CustomerViewModel));
        }
    }
}
