using GoogleMapXamarin.Popup;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GoogleMapXamarin.Model
{
    public class ImageReference
    {
        public string url { get; set; }
        public string fullSizeUrl { get; set; }
        public Command onTapImage { get; set; }
        public async Task runOnTapImage()
        {
            PopupNavigation.Instance.PushAsync(new FitImage(fullSizeUrl.ToString()));
        }
        public ImageReference()
        {
            onTapImage = new Command(async () => await runOnTapImage());
        }
    }
}
