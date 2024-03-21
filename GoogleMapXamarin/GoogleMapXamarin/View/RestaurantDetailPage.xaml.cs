using GoogleMapXamarin.Model;
using GoogleMapXamarin.PageModel;
using GoogleMapXamarin.Response;
using Newtonsoft.Json;
using Plugin.Geolocator.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using static GoogleMapXamarin.Model.PlaceInfoResponse;

namespace GoogleMapXamarin.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantDetailPage : ContentPage
    {
        public RestaurantDetailPageModel pageModel = new RestaurantDetailPageModel();
        public RestaurantDetailPage(string placeId)
        {
            Restaurant restaurant = AppData.restaurantResult.FirstOrDefault(e => e.placeId == placeId);
            PlaceInfoResponse.Location restaurant_location = restaurant.placeInfo.geometry.location;
            pageModel.mapUrl = restaurant.placeInfo.url;
            pageModel.name = restaurant.name;
            pageModel.formatted_address = restaurant.placeInfo.formatted_address;
            pageModel.rating = restaurant.rating;
            pageModel.openStatus = restaurant.business_status;
            pageModel.openStatusColor = restaurant.business_status_color;
            pageModel.reviews = restaurant.placeInfo.reviews;
            pageModel.distance = restaurant.distance;

            //  price level
            pageModel.price_level = restaurant.placeInfo.price_level > 0 ? restaurant.placeInfo.price_level : 1;



            //  open info
            if (restaurant.placeInfo.current_opening_hours != null)
            {
                CurrentOpeningHours openingHours = restaurant.placeInfo.current_opening_hours;

                pageModel.weekday_text = openingHours.weekday_text.Select(e => new BusinessTimeListView() { text = e, visible = true, textColor = "Gray" }).ToList();
                pageModel.weekday_text.LastOrDefault().visible = false;
                pageModel.weekday_text[((int)DateTime.Now.DayOfWeek) - 1].textColor = "Green";
            }


            //  image
            if (restaurant.placeInfo.photos != null)
            {
                List<ImageReference> imageReferences = restaurant.placeInfo.photos.Select(e => new ImageReference() { url = $"https://maps.googleapis.com/maps/api/place/photo?photoreference={e.photo_reference}&maxwidth=400&key={AppData.APIkey}", fullSizeUrl = $"https://maps.googleapis.com/maps/api/place/photo?photoreference={e.photo_reference}&maxwidth={e.width}&maxheight={e.height}&key={AppData.APIkey}" }).ToList();
                pageModel.photosReference = imageReferences;
            }


            BindingContext = pageModel;
            InitializeComponent();

            //  map route request api
            double zoomLevel = 200;

            formMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(restaurant_location.lat, restaurant_location.lng), Xamarin.Forms.Maps.Distance.FromMeters(zoomLevel)));

            //
            Pin pin = new Pin() { 
                Label = restaurant.placeInfo.name,
                Type = PinType.Place,
                Address = restaurant.placeInfo.formatted_address,
                Position = new Xamarin.Forms.Maps.Position(restaurant_location.lat, restaurant_location.lng) };
            formMap.Pins.Add(pin);

        }

        protected override bool OnBackButtonPressed()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAsync();
                return true;
            }
            return base.OnBackButtonPressed();
        }

    }
}