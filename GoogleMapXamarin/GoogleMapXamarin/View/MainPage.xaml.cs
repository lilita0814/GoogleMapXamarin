using GoogleMapXamarin.Model;
using GoogleMapXamarin.PageModel;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GoogleMapXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private MapPageModel pageModel = new MapPageModel();
        public MainPage()
        {
            InitializeComponent();
            BindingContext = pageModel;
            Device.BeginInvokeOnMainThread(async () => await refreshMap());
        }

        public async Task refreshMap()
        {
            try
            {
                //  refresh map view
                var location = await AppData.getCurrentlyLocation();
                if (location != null)
                {
                    double zoomLevel = 200;
                    
                    formMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMeters(zoomLevel)));
                    
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "ok");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAsync();
                return true;
            }
            if (tabView.SelectedIndex > 0)
            {
                tabView.SelectedIndex = 0;
                return true;
            }
            return base.OnBackButtonPressed();
        }


    }
}
