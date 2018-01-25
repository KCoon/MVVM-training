using System.Windows.Input;
using MVVM_Training.MVVM;
using MVVM_Training.Model;

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

        public ViewModelMain()
        {
            _TextProperty1 = "Enter text and press Enter...";
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
    }
}
