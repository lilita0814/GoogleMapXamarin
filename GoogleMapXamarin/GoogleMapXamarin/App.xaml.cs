using GoogleMapXamarin.Model;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoogleMapXamarin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppData.SyncfusionKey);

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
