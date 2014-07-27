using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using MYOB.AccountRight.SDK.Contracts.Version2.Sale;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MYOB.Sample.UserControls
{
    public class InvoiceClickedEventArgs : EventArgs
    {
        public InvoiceClickedEventArgs(Invoice invoice)
        {
            Invoice = invoice;
        }

        public Invoice Invoice { get; set; }
    }

    public partial class ListInvoices : UserControl
    {
        public ListInvoices()
        {
            InitializeComponent();
        }

        public event EventHandler<InvoiceClickedEventArgs> InvoiceClicked;

        private void Invoice_Tap(object sender, GestureEventArgs e)
        {
            var box = sender as ListBox;
            if (box == null) return;
            InvoiceClicked(sender, new InvoiceClickedEventArgs(box.SelectedItem as Invoice));
        }
    }
}
