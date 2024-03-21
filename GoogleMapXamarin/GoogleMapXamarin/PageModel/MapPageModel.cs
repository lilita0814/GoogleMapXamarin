using GoogleMapXamarin.Model;
using GoogleMapXamarin.Popup;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using static GoogleMapXamarin.Model.PlaceInfoResponse;
using Photo = GoogleMapXamarin.Model.Photo;

namespace GoogleMapXamarin.PageModel
{
    public class MapPageModel : INotifyPropertyChanged
    {
        public ObservableCollection<Restaurant> _results { get; set; } = new ObservableCollection<Restaurant>();
        public ObservableCollection<Restaurant> results { get { return _results; } set { _results = value; resetAllProperties(); } }
        public Command searchNearbyRestaurantButton { get; set; }
        public async Task runSearchNearbyRestaurant()
        {
            //  in requesting api
            if (isRequestingApi)
                return;
            isRequestingApi = true;
            AppData.CurrentlyLocation = null;
            AppData.CurrentlyLocation = await AppData.getCurrentlyLocation();
            //  no location
            if (AppData.CurrentlyLocation == null)
                return;
            //  clear restaurant data
            results = new ObservableCollection<Restaurant>();
            AppData.restaurantResult = new List<Restaurant>();
            resultCount = 0;

            //  get result from google api
            Uri uri = new Uri(RequestAPI.searchRestaurant(AppData.CurrentlyLocation.Latitude, AppData.CurrentlyLocation.Longitude));
            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(uri);
            string responseMessage = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                NearbyRestaurantRoot root = JsonConvert.DeserializeObject<NearbyRestaurantRoot>(responseMessage);
                if (root.results.Count == 0)
                {
                    //  no data found
                    Application.Current.MainPage.DisplayAlert("", "no data found", "ok");
                }
                else
                {
                    //  after founded api
                    addRestaurantListToResult(root.results);
                    nextPageToken = root.next_page_token;
                    tabViewIndex = 1;
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Failed to search restaurants", responseMessage, "ok");
            }
            isRequestingApi = false;
        }
        public int _tabViewIndex { get; set; }
        public int tabViewIndex { get { return _tabViewIndex; } set { _tabViewIndex = value; resetAllProperties(); } }
        public Command backToMapButton { get; set; }
        public async Task runBackToMap()
        {
            tabViewIndex = 0;
        }
        public Command goToResultButton { get; set; }
        public async Task runGoToResult()
        {
            tabViewIndex = 1;
        }
        public int _resultCount { get; set; }
        public int resultCount { get { return _resultCount; } set { _resultCount = value; resetAllProperties(); } }
        public string nextPageToken { get; set; }
        public Command addNextPageResult { get; set; }
        public async Task runAddNextPageResult()
        {
            if (results.Count < 20 || isRequestingApi || nextPageToken == null)
                return;

            //  has next page
            isRequestingApi = true;
            HttpClient client = new HttpClient();
            Uri uri = new Uri(RequestAPI.requestMapPageToken(nextPageToken));
            HttpResponseMessage responseMessage = await client.GetAsync(uri);
            if (responseMessage.IsSuccessStatusCode)
            {
                //  success
                NearbyRestaurantRoot root = JsonConvert.DeserializeObject<NearbyRestaurantRoot>(await responseMessage.Content.ReadAsStringAsync());
                addRestaurantListToResult(root.results);
                nextPageToken = root.next_page_token;
            }
            isRequestingApi = false;
        }
        private bool _isRequestingApi { get; set; }
        public bool isRequestingApi
        {
            get { return _isRequestingApi; }
            set
            {
                _isRequestingApi = value;
                searchButtonIconVisible = value;
            }
        }
        public bool _searchButtonIconVisible { get; set; }
        public bool searchButtonIconVisible { get { return _searchButtonIconVisible; } set { _searchButtonIconVisible = value; resetAllProperties(); } }
        public Command openSearchKeyword { get; set; }
        public async Task runOpenSearchKeyword()
        {
            PopupNavigation.Instance.PushAsync(new SearchKeyWordPopup());
        }

        public MapPageModel()
        {
            searchNearbyRestaurantButton = new Command(async () => await runSearchNearbyRestaurant());
            backToMapButton = new Command(async () => await runBackToMap());
            goToResultButton = new Command(async () => await runGoToResult());
            addNextPageResult = new Command(async () => await runAddNextPageResult());
            openSearchKeyword = new Command(async () => await runOpenSearchKeyword());

        }


        public void resetAllProperties()
        {
            NotifyPropertyChanged(nameof(results));
            NotifyPropertyChanged(nameof(tabViewIndex));
            NotifyPropertyChanged(nameof(resultCount));
            NotifyPropertyChanged(nameof(searchButtonIconVisible));
        }
        public virtual void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void addRestaurantListToResult(List<NearbyRestaurantResult> responseRestaurants)
        {
            responseRestaurants.ForEach(async e =>
            {
                Restaurant data = new Restaurant()
                {
                    //  distance
                    distance = Math.Round(GeolocatorUtils.CalculateDistance(new Position(AppData.CurrentlyLocation.Latitude, AppData.CurrentlyLocation.Longitude), new Position(e.geometry.location.lat, e.geometry.location.lng), GeolocatorUtils.DistanceUnits.Kilometers), 2),
                    //  name
                    name = e.name,
                    //  rating
                    rating = e.rating
                };

                //  image
                if (e.photos != null && e.photos.Count > 0)
                {
                    Photo photo = e.photos[0];
                    data.imageReference = RequestAPI.requestMapPhoto(photo.photo_reference);
                    data.fullSizeImage = RequestAPI.requestMapPhoto(photo.photo_reference, photo.width, photo.height);
                }

                //  place id
                data.placeId = e.place_id;
                //  info
                data.infoApiUrl = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={e.place_id}&key={AppData.APIkey}";
                await data.getPlaceInfo();

                //  add to restaurant
                results.Add(data);
                AppData.restaurantResult.Add(data);
                resultCount = results.Count;
            });
        }
    }
}
