using GoogleMapXamarin.Model;
using GoogleMapXamarin.Popup;
using Rg.Plugins.Popup.Services;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static GoogleMapXamarin.Model.PlaceInfoResponse;

namespace GoogleMapXamarin.PageModel
{
    public class RestaurantDetailPageModel : INotifyPropertyChanged
    {
        public string name { get; set; }
        public List<ImageReference> photosReference { get; set; }
        public List<BusinessTimeListView> weekday_text { get; set; }
        public double rating { get; set; }
        public string mapUrl { get; set; }
        public int user_ratings_total { get; set; }
        public string formatted_address { get; set; }
        public List<Review> reviews { get; set; }
        public int price_level { get; set; }
        public string get_price_level_dollar
        {
            get
            {
                string result = "";
                for (int x = price_level; x > 0; x--)
                    result += "$";
                return result;
            }
        }
        public string openStatus { get; set; }
        public string openStatusColor { get; set; }
        public string formatted_phone_number { get; set; }
        public double distance { get; set; }
        public Command backButton { get; set; }
        public int weekday_text_height { get { return weekday_text.Count * 20; } }
        public void runBackButton()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
        public Command openOnMapButton { get; set; }
        public void runOpenOnMapButton()
        {
            Browser.OpenAsync(new Uri(mapUrl), BrowserLaunchMode.SystemPreferred);
        }
        public Command copyAddress { get; set; }
        public async Task runCopyAddress(object sender)
        {
            SfButton button = (SfButton) sender;
            button.Text = "Copied";
            button.ImageSource = "checkmark_icon.png";
            await Clipboard.SetTextAsync(formatted_address);
        }
        public Command generateQr { get; set; }
        public async Task runGenerateQr()
        {
            string url = $"https://api.qrserver.com/v1/create-qr-code/?size=800x800&data={mapUrl}";
            PopupNavigation.Instance.PushAsync(new FitImage(url));
        }
        public RestaurantDetailPageModel()
        {
            backButton = new Command(() => runBackButton());
            openOnMapButton = new Command(() => runOpenOnMapButton());
            copyAddress = new Command(async (sender) => await runCopyAddress(sender));
            generateQr = new Command(() => runGenerateQr());
        }



        public void resetAllProperties()
        {

        }
        public virtual void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
