# MVVM-training

1. add folders
	Model
	View
	ViewModel

2. move MainWindow into View

3. MainWindow.xaml.cs
	change namespace to [Project].ViewModel

4. adding class ViewModelBase to directory ViewModel
	- using System.ComponentModel
	- inherit from INotifyPropertyChanged
	- adding:
	internal void RaisePropertyChanged(string prop)
	{
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
    }
    public event PropertyChangedEventHandler PropertyChanged;

5. adding class ViewModelMain to directory ViewModel
	- inherit from ViewModelBase
	- adding string with propertychanged
	- adding constructor and setting string


6. MainWindow.xaml
	- change x:Class to [Project].ViewModel.MainWindow
	- add namespace xmlns:vm="clr-namespace:[Project].ViewModel"
	- add DataContext="{DynamicResource ViewModelMain}"
	- add Window.Resources <vm: ViewModelMain x:Key="ViewModelMain"
	- add TextBlock with Binding to TextProperty1
	- add TextBox with Binding to TextProperty1 and UpdateSourceTrigger=PropertyChanged

7. App.xaml
	- change StartupUri to View/MainWindow.xaml
