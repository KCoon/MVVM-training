# MVVM-training

1. add folders
	* Model
	* View
	* ViewModel

1. move MainWindow into View

1. MainWindow.xaml.cs
	change namespace to [Project].ViewModel

1. adding class ViewModelBase to directory ViewModel
	* ```using System.ComponentModel```
	* inherit from ```INotifyPropertyChanged```
	* adding:
	```c#
	internal void RaisePropertyChanged(string prop)
	{
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
    }
    public event PropertyChangedEventHandler PropertyChanged;
	```

1. adding class ViewModelMain to directory ViewModel
	* inherit from ```ViewModelBase```
	* adding string property
	```c#
	private string _TextProperty1;
	public string TextProperty1
	{
		get
		{
			return _TextProperty1;
		}
		set
		{
			if(_TextProperty1 != value)
			{
				_TextProperty1 = value;
				RaisePropertyChanged("TextProperty1");
			}
		}
	}
	```
	* adding constructor and setting string

1. MainWindow.xaml
	* change x:Class to [Project].ViewModel.MainWindow
	* add namespace xmlns:vm="clr-namespace:[Project].ViewModel"
	* add ```DataContext="{DynamicResource ViewModelMain}"```
	* add Window.Resources
	```xaml
	<Window.Resources>
        <vm:ViewModelMain x:Key="ViewModelMain" />
    </Window.Resources>
	```
	* add TextBlock with Binding to TextProperty1
	* add TextBox with Binding to TextProperty1 and UpdateSourceTrigger=PropertyChanged

1. App.xaml
	* change StartupUri to View/MainWindow.xaml
