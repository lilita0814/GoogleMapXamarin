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
        public static string SyncfusionKey = "Ngo9BigBOggjHTQxAR8/V1NHaF5cWWdCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXteeHRRRmlfWUR3X0c=";
        public static List<Restaurant> restaurantResult = new List<Restaurant>();
        public static ObservableCollection<string> searchKeyword = new ObservableCollection<string>();


        public static Xamarin.Essentials.Location CurrentlyLocation { get; set; }
        public static async Task<Xamarin.Essentials.Location> getCurrentlyLocation()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    // 请求位置权限
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (status != PermissionStatus.Granted)
                    {
                        // 如果用户拒绝了位置权限，则无法请求打开 GPS
                        await Application.Current.MainPage.DisplayAlert("Permission Denied", "Location permission is required to access GPS.", "OK");
                        return null;
                    }
                }
                CurrentlyLocation = await Geolocation.GetLocationAsync();
                return CurrentlyLocation;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("", "Please enable gps service.", "ok");
                return null;
            }
        }


    }
}
