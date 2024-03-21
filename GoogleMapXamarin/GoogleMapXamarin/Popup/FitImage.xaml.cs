using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoogleMapXamarin.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FitImage
    {
        public FitImage(string imageUrl)
        {
            InitializeComponent();
            showImage.Source = imageUrl;
        }
    }
}