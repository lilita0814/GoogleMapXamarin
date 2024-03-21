using GoogleMapXamarin.Popup;
using GoogleMapXamarin.View;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static GoogleMapXamarin.Model.PlaceInfoResponse;

namespace GoogleMapXamarin.Model
{
    public class Restaurant : INotifyPropertyChanged
    {
        public string placeId { get; set; }
        public string name { get; set; }
        public double distance { get; set; }
        public double rating { get; set; }
        public string imageReference { get; set; }
        public string fullSizeImage { get; set; }
        public string business_status { get; set; }
        public string business_status_color { get; set; }
        public string business_time { get; set; }
        public PlaceInfoResult placeInfo { get; set; }
        public string infoApiUrl { get; set; }
        public Command onTapImage { get; set; }
        public async Task runOnTapImage()
        {
            PopupNavigation.Instance.PushAsync(new FitImage(fullSizeImage.ToString()));
        }
        public Command tapOpenRestaurantPage { get; set; }
        public async Task runTapOpenRestaurantPage()
        {
            Application.Current.MainPage.Navigation.PushAsync(new RestaurantDetailPage(placeId));
        }
        public Restaurant()
        {
            onTapImage = new Command(async () => await runOnTapImage());
            tapOpenRestaurantPage = new Command(async () => await runTapOpenRestaurantPage());
        }
        public async Task getPlaceInfo()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(infoApiUrl);
            if (response.IsSuccessStatusCode)
            {

                PlaceInfoRoot root = JsonConvert.DeserializeObject<PlaceInfoRoot>(await response.Content.ReadAsStringAsync());
                placeInfo = root.result;
            }


            //  set data
            if (placeInfo != null && placeInfo.current_opening_hours != null)
            {
                //  set business status
                if (placeInfo.current_opening_hours.open_now)
                {
                    business_status = "Opening";
                    business_status_color = "Green";
                }
                else
                {
                    business_status = "Closed";
                    business_status_color = "Red";
                }

                //  set business time
                if (placeInfo.current_opening_hours.periods.Any(e => e.open.date == DateTime.Now.ToString("yyyy-MM-dd")))
                {
                    List<Period> period = placeInfo.current_opening_hours.periods;
                    string startTime = period.FirstOrDefault(e => e.open.date == DateTime.Now.ToString("yyyy-MM-dd")).open.time;
                    //  get next close time
                    string closeTime = "";
                    int closeDay = period.FirstOrDefault(e => e.open.date == DateTime.Now.ToString("yyyy-MM-dd")).open.day;
                    string endTimeDay = "";
                    for (int x = 0; x < 7; x++)
                    {
                        //  close on today
                        if (period.Any(e => e.close.day == closeDay))
                        {
                            //  founded close data
                            closeTime = placeInfo.current_opening_hours.periods.FirstOrDefault(e => e.close.day == closeDay).close.time;
                            break;
                        }
                        closeDay = closeDay == 6 ? 0 : closeDay + 1;
                        endTimeDay = $" ({Enum.GetName(typeof(DayOfWeek), closeDay)})";
                    }

                    business_time = $"{businessTimeConvertTo12Hour(startTime)} - {businessTimeConvertTo12Hour(closeTime)}{endTimeDay}";
                }
            }
            else
            {
                business_status = "Unknow Business State";
                business_status_color = "Gold";
            }

            resetAllProperties();
        }

        public string businessTimeConvertTo12Hour(string time)
        {
            byte hour = byte.Parse(time.Substring(0, 2));
            byte minute = byte.Parse(time.Substring(2, 2));
            string timeType = "AM";
            if (hour >= 12)
            {
                hour = (byte)(hour > 12 ? hour - 12 : hour);
                timeType = "PM";
            }
            else if (hour == 0)
                hour = 12;
            return $"{hour.ToString("00")}:{minute.ToString("00")} {timeType}";
        }

        public void resetAllProperties()
        {
            NotifyPropertyChanged(nameof(business_status));
            NotifyPropertyChanged(nameof(business_status_color));
            NotifyPropertyChanged(nameof(business_time));
        }
        public virtual void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
