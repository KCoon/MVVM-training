using System.Windows.Input;
using MVVM_Training.MVVM;
using MVVM_Training.Model;
using System.Collections.ObjectModel;

namespace MVVM_Training.ViewModel
{
    class ViewModelMain : ViewModelBase
    {
        private readonly LogicModel converter = new LogicModel();

        private string _TextProperty1;
        public string TextProperty1
        {
            get
            {
                return _TextProperty1;
            }
            set
            {
                if (_TextProperty1 != value)
                {
                    _TextProperty1 = value;
                    RaisePropertyChanged("TextProperty1");
                }
            }
        }

        private ObservableCollection<ListItem> _listing;
        public ObservableCollection<ListItem> Listing
        {
            get
            {
                return _listing;
            }
            set
            {
                _listing = value;
            }
        }

        private Colours _favouriteColour;
        public Colours FavouriteColour
        {
            get
            {
                return _favouriteColour;
            }
            set
            {
                _favouriteColour = value;
            }
        }

        public ViewModelMain()
        {
            _TextProperty1 = "Enter text and press Enter...";

            _listing = new ObservableCollection<ListItem>();
            _listing.Add(new ListItem(true, "foo"));
            _listing.Add(new ListItem(false, "bar"));

            _favouriteColour = Colours.pink;
        }

        private RelayCommand _convertTextCommand;
        public ICommand ConvertTextCommand
        {
            get
            {
                if (_convertTextCommand == null)
                {
                    _convertTextCommand = new RelayCommand(param => this.ConvertText());
                }
                return _convertTextCommand;
            }
        }

        private void ConvertText()
        {
            TextProperty1 = converter.ChangeText(_TextProperty1);
        }

        private RelayCommand _addListItemCommand;
        public ICommand AddListItemCommand
        {
            get
            {
                if (_addListItemCommand == null)
                {
                    _addListItemCommand = new RelayCommand(param => this.AddListItem());
                }
                return _addListItemCommand;
            }
        }

        private void AddListItem()
        {
            _listing.Add(new ListItem(true, "foobar"));
        }
    }
}
