using GoogleMapXamarin.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoogleMapXamarin.PageModel
{
    public class SearchKeyWordPageModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> SelectedTags { get { return AppData.searchKeyword; } }


        public SearchKeyWordPageModel()
        {
            
        }

        public void resetAllProperties()
        {
            NotifyPropertyChanged(nameof(SelectedTags));
        }
        public virtual void NotifyPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
