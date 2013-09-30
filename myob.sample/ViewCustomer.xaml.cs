using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using MYOB.AccountRight.SDK;
using MYOB.AccountRight.SDK.Contracts;
using MYOB.AccountRight.SDK.Contracts.Version2.Contact;
using MYOB.Sample.ViewModels;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Microsoft.Phone.Tasks;

namespace MYOB.Sample
{
    public partial class ViewCustomer : SharedPhoneApplicationPage
    {
        private readonly PhotoChooserTask _photoChooserTask;
        private CameraCaptureTask _cameraCaptureTask;

        private WriteableBitmap _writeable;

        public CustomerViewModel ViewModel { get; private set; }
        public ViewCustomer()
        {
            InitializeComponent();
            ViewModel = new CustomerViewModel(this);
            DataContext = ViewModel;
            _photoChooserTask = new PhotoChooserTask();
            _photoChooserTask.Completed += _photoChooserTask_Completed;
            _cameraCaptureTask = new CameraCaptureTask();
            _cameraCaptureTask.Completed += _cameraCaptureTask_Completed;
        }

        void _cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            ExtractPhoto(e);
        }

        void _photoChooserTask_Completed(object sender, PhotoResult e)
        {
            ExtractPhoto(e);
        }

        private void ExtractPhoto(PhotoResult e)
        {
            if (e.TaskResult != TaskResult.OK) return;
            var image = new BitmapImage();
            image.SetSource(e.ChosenPhoto);
            ViewModel.Picture = image;

            var st = new ScaleTransform() {ScaleX = 0.2, ScaleY = 0.2};

            _writeable =
                new WriteableBitmap(new Image {Width = 800, Height = 600, Visibility = Visibility.Collapsed, Source = image}, st);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (State.ContainsKey("Customer"))
            {
                ViewModel.Customer = JsonConvert.DeserializeObject<Contact>((string)State["Customer"]);
                ViewModel.CompanyFile = (CompanyFile)State["CompanyFile"];
                ViewModel.Credentials = new CompanyFileCredentials((string)State["Username"], (string)State["Password"]);

                if (_writeable != null)
                {
                    using (var stream = new MemoryStream())
                    {
                        _writeable.SaveJpeg(stream, 160, 120, 0, 80);
                        ViewModel.UploadPicture(stream);
                    }
                    _writeable = null;
                }
            }
            else
            {
                ViewModel.FetchPicture();
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            State["Customer"] = JsonConvert.SerializeObject(ViewModel.Customer);
            State["CompanyFile"] = ViewModel.CompanyFile;
            State["Username"] = ViewModel.Credentials.Username;
            State["Password"] = ViewModel.Credentials.Password;
            base.OnNavigatingFrom(e);
        }

        private void Refresh_OnClick(object sender, EventArgs e)
        {
            ViewModel.Refresh();
        }

        private void Upload_OnClick(object sender, EventArgs e)
        {
            _photoChooserTask.Show();
        }

        private void Delete_OnClick(object sender, EventArgs e)
        {
            ViewModel.DeletePhoto();
        }

        private void Photo_OnClick(object sender, EventArgs e)
        {
            _cameraCaptureTask.Show();
        }
    }
}