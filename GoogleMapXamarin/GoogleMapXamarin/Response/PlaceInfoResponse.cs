using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleMapXamarin.Model
{
    public class PlaceInfoResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Close
        {
            public string date { get; set; }
            public int day { get; set; }
            public string time { get; set; }
        }

        public class CurrentOpeningHours
        {
            public bool open_now { get; set; }
            public List<Period> periods { get; set; }
            public List<string> weekday_text { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Open
        {
            public string date { get; set; }
            public int day { get; set; }
            public string time { get; set; }
        }

        public class OpeningHours
        {
            public bool open_now { get; set; }
            public List<Period> periods { get; set; }
            public List<string> weekday_text { get; set; }
        }

        public class Period
        {
            public Close close { get; set; }
            public Open open { get; set; }
        }

        public class Photo
        {
            public int height { get; set; }
            public List<string> html_attributions { get; set; }
            public string photo_reference { get; set; }
            public int width { get; set; }
        }

        public class PlusCode
        {
            public string compound_code { get; set; }
            public string global_code { get; set; }
        }

        public class PlaceInfoResult
        {
            public List<AddressComponent> address_components { get; set; }
            public string adr_address { get; set; }
            public string business_status { get; set; }
            public CurrentOpeningHours current_opening_hours { get; set; }
            public bool dine_in { get; set; }
            public string formatted_address { get; set; }
            public string formatted_phone_number { get; set; }
            public Geometry geometry { get; set; }
            public string icon { get; set; }
            public string icon_background_color { get; set; }
            public string icon_mask_base_uri { get; set; }
            public string international_phone_number { get; set; }
            public string name { get; set; }
            public OpeningHours opening_hours { get; set; }
            public List<Photo> photos { get; set; }
            public string place_id { get; set; }
            public PlusCode plus_code { get; set; }
            public int price_level { get; set; }
            public double rating { get; set; }
            public string reference { get; set; }
            public bool reservable { get; set; }
            public List<Review> reviews { get; set; }
            public bool serves_brunch { get; set; }
            public bool serves_dinner { get; set; }
            public bool serves_lunch { get; set; }
            public bool takeout { get; set; }
            public List<string> types { get; set; }
            public string url { get; set; }
            public int user_ratings_total { get; set; }
            public int utc_offset { get; set; }
            public string vicinity { get; set; }
        }

        public class Review
        {
            public string author_name { get; set; }
            public string author_url { get; set; }
            public string language { get; set; }
            public string original_language { get; set; }
            public string profile_photo_url { get; set; }
            public int rating { get; set; }
            public string getFullRatingStar
            {
                get
                {
                    string result = "";
                    for (int x = 0; x < rating; x++)
                        result += "★";
                    return result;
                }
            }
            public string getEmptyRatingStar
            {
                get
                {
                    string result = "";
                    for (int x = 0; x < (5 - rating); x++)
                        result += "☆";
                    return result;
                }
            }
            public string relative_time_description { get; set; }
            public string text { get; set; }
            public int time { get; set; }
            public bool translated { get; set; }
        }

        public class PlaceInfoRoot
        {
            public List<object> html_attributions { get; set; }
            public PlaceInfoResult result { get; set; }
            public string status { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }


    }
}
