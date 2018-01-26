using System;
using MVVM_Training.Model;
using System.Collections.ObjectModel;

namespace MVVM_Training.ViewModel
{
    class ListItem
    {
        private bool _booleanPropery;
        public bool BooleanProperty
        {
            get
            {
                return _booleanPropery;
            }
            set
            {
                _booleanPropery = value;
            }
        }

        private string _textProperty;
        public string TextProperty
        {
            get
            {
                return _textProperty;
            }
            set
            {
                _textProperty = value;
            }
        }

        private ObservableCollection<string> _citysProperty;
        public ObservableCollection<string> CitysProperty
        {
            get
            {
                return _citysProperty;
            }
            set
            {
                _citysProperty = value;
            }
        }
            
        public ListItem(bool boolean, string text)
        {
            _booleanPropery = boolean;
            _textProperty = text;

            _citysProperty = new ObservableCollection<string>(Enum.GetNames(typeof(Citys)));
        }
    }
}
