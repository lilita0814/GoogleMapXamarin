using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Essentials;
using static GoogleMapXamarin.Model.PlaceInfoResponse;

namespace GoogleMapXamarin.Model
{
    public static class RequestAPI
    {
        public static string searchRestaurant(double latitude, double longitude)
        {
            if (AppData.searchKeyword.Count == 0)
            {
                return $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude}%2C{longitude}&keyword=food&rankby=distance&key={AppData.APIkey}";
            }
            else
            {
                string joined_word = string.Join(",", AppData.searchKeyword).ToLower();
                return $"https://maps.googleapis.com/maps/api/place/textsearch/json?location={latitude}%2C{longitude}&type=restaurant&rankby=distance&query={joined_word}&key={AppData.APIkey}";
            }
        }
        public static string requestMapPhoto(string photo_reference)
        {
            return $"https://maps.googleapis.com/maps/api/place/photo?key={AppData.APIkey}&photoreference={photo_reference}&maxwidth=400";
        }
        public static string requestMapPhoto(string photo_reference, int width, int height)
        {
            return $"https://maps.googleapis.com/maps/api/place/photo?key={AppData.APIkey}&photoreference={photo_reference}&maxwidth={width}&maxheight={height}";
        }

        public static string requestMapPageToken(string nextPageToken)
        {
            return $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?pagetoken={nextPageToken}&key={AppData.APIkey}";
        }

        public static string requestDirection(Xamarin.Essentials.Location origin, Xamarin.Essentials.Location destination)
        {
            return $"https://maps.googleapis.com/maps/api/directions/json?origin={origin.Latitude},{origin.Longitude}&destination={destination.Latitude},{destination.Longitude}&key={AppData.APIkey}";
        }
    }
}
