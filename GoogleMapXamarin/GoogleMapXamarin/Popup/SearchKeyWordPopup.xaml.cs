using GoogleMapXamarin.Model;
using GoogleMapXamarin.PageModel;
using Syncfusion.DataSource.Extensions;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GoogleMapXamarin.Popup
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchKeyWordPopup
    {
        public SearchKeyWordPageModel pageModel = new SearchKeyWordPageModel();
        private ObservableCollection<string> keywords = new ObservableCollection<string>()
        {
            "Vegetarian", "Halal", "Fast food", "Japanese", "Chinese", "Sea Food"
        };

        public SearchKeyWordPopup()
        {
            BindingContext = pageModel;
            InitializeComponent();

            resetTagArea();
        }


        public void resetTagArea()
        {
            TagArea.Children.Clear();
            StackLayout newLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Spacing = 5
            };

            //  item
            keywords.ForEach(word =>
            {
                SfButton button = new SfButton()
                {
                    Text = word,
                    Padding = new Thickness(10),
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.Black,
                    CornerRadius = 10,
                    BackgroundColor = Color.LightGreen
                };
                button.Clicked += clickTag;

                //  already selected
                if (AppData.searchKeyword.Contains(word))
                {
                    button.BackgroundColor = Color.LightGray;
                    button.TextColor = Color.Gray;
                }

                //  add to layout row
                newLayout.Children.Add(button);

                //  if layout row reach 3 child
                if (newLayout.Children.Count == 3)
                {
                    TagArea.Children.Add(newLayout);
                    newLayout = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Spacing = 5
                    };
                }
            });
            //
            TagArea.Children.Add(newLayout);
        }


        private void clickTag(object sender, EventArgs ev)
        {
            SfButton button = (SfButton)sender;
            string text = button.Text;

            //  change color
            if (button.BackgroundColor == Color.LightGreen)
            {
                button.BackgroundColor = Color.LightGray;
                button.TextColor = Color.Gray;
                AppData.searchKeyword.Add(text);
            }
            else
            {
                button.BackgroundColor = Color.LightGreen;
                button.TextColor = Color.Black;
                AppData.searchKeyword.Remove(text);
            }

            pageModel.resetAllProperties();

        }

        private void remove_keyword(object sender, EventArgs ev)
        {
            SfButton button = (SfButton)sender;
            string text = button.CommandParameter.ToString();


            SfButton tagAreaButton = null;
            foreach (var item in TagArea.Children)
            {
                StackLayout layout = item as StackLayout;
                tagAreaButton = layout.Children.FirstOrDefault(e => ((SfButton)e).Text == text) as SfButton;
                if (tagAreaButton != null)
                    break;
            };

            // remove button tag
            tagAreaButton.BackgroundColor = Color.LightGreen;
            tagAreaButton.TextColor = Color.Black;
            AppData.searchKeyword.Remove(text);

            pageModel.resetAllProperties();
        }
    }
}