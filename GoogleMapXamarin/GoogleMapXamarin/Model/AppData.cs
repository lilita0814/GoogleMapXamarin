using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoogleMapXamarin.Model
{
    public static class AppData
    {
        public static string APIkey = "AIzaSyDssYgPcc1P_jwCd4ngWtfbdW7PexcH3L8";
        public static string SyncfusionKey = "Ngo9BigBOggjHTQxAR8/V1NAaF1cW2hKYVJ/WmFZfVpgfF9HZ1ZUQGY/P1ZhSXxXdkdiUX1fcHRUQmlfU0A=";
        public static List<Restaurant> restaurantResult = new List<Restaurant>();
        public static ObservableCollection<string> searchKeyword = new ObservableCollection<string>();


        public static Xamarin.Essentials.Location CurrentlyLocation { get; set; }
        public static async Task<Xamarin.Essentials.Location> getCurrentlyLocation()
        {
            try
            {
                CurrentlyLocation = await Geolocation.GetLocationAsync();
                return CurrentlyLocation;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("","Please enable GPS","ok");
                return null;
            }
        }


    }
}
