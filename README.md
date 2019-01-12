# MVVM-training


## Minimum Example

1. add folders
	* Model
	* View
	* ViewModel
	* MVVM

1. move **MainWindow**
	* move **MainWindow** into View
	* **App.xaml**
		* change StartupUri to View/MainWindow.xaml

1. change namespace of the view
	* **MainWindow.xaml.cs**
		* change namespace to [Project].ViewModel
	* **MainWindow.xaml**
		* change x:Class to [Project].ViewModel.MainWindow

1. adding class **ViewModelBase** to directory *MVVM*
	```c#
	using System.ComponentModel;

	namespace MVVM_Training.MVVM
	{
		class ViewModelBase : INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;
			internal void RaisePropertyChanged(string property)
			{
				if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(property)); }
			}
		}
	}
	```
1. adding class **RelayCommand** to directory *MVVM*
	```c#
	using System;
	using System.Diagnostics;
	using System.Windows.Input;

	namespace MVVM_Training.MVVM
	{
		public class RelayCommand : ICommand
		{
			readonly Action<object> _execute;
			readonly Predicate<object> _canExecute;

			public RelayCommand(Action<object> execute) : this(execute, null) { }
			public RelayCommand(Action<object> execute, Predicate<object> canExecute)
			{
				if (execute == null)
					throw new ArgumentNullException("execute");
				_execute = execute; _canExecute = canExecute;
			}

			[DebuggerStepThrough]
			public bool CanExecute(object parameter)
			{
				return _canExecute == null ? true : _canExecute(parameter);
			}

			public event EventHandler CanExecuteChanged
			{
				add { CommandManager.RequerySuggested += value; }
				remove { CommandManager.RequerySuggested -= value; }
			}

			public void Execute(object parameter) { _execute(parameter); }
		}
	}
	```

1. adding class **ViewModelMain** to directory *ViewModel*
	* inherit from ```ViewModelBase```
	* adding string property
	* adding constructor
	* initialize string property
	```c#
	using MVVM_Training.MVVM;

	namespace MVVM_Training.ViewModel
	{
		class ViewModelMain : ViewModelBase
		{
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
				_TextProperty1 = "Enter text and press enter...";
			}
		}
	}
	```

1. **MainWindow.xaml** - Prepare binding of **View** to **ViewModel**
	* add namespace xmlns:vm="clr-namespace:[Project].ViewModel"
	* add ```DataContext="{DynamicResource ViewModelMain}"```
	* add Window.Resources
	```xaml
	<Window.Resources>
        <vm:ViewModelMain x:Key="ViewModelMain" />
    </Window.Resources>
	```
1. **MainWindow.xaml** - Add Controls to View and bind them to ViewModel
	* add TextBox with Binding to TextProperty1 and UpdateSourceTrigger=PropertyChanged
	```xaml
	<Grid>
        <TextBox Name="TextBox1" Margin="-200,-200,0,0" Text="{Binding TextProperty1, UpdateSourceTrigger=PropertyChanged}" Width="160" Height="22"/>
    </Grid>
	```

1. Add **LogicModel** to directory *Model*
	* add method ```ChangeText```
	```c#
	namespace MVVM_Training.Model
	{
		class LogicModel
		{
			public LogicModel() { }

			public string ChangeText(string text)
			{
				return text.ToUpper();
			}
		}
	}
	```

1. Instantiate **LogicModel** in **ViewModel**
	```c#
	private readonly LogicModel converter = new LogicModel();
	```

1. Add RelayCommand instance to call **ChangeText** method in **LogicModel**
	```c#
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
	```
1. Add input binding to **View**
	```xaml
	<Window.InputBindings>
		<KeyBinding Key="Enter" Command="{Binding ConvertTextCommand}" />
    </Window.InputBindings>
	```

## ListBox

1. Add class **ListItem** to *ViewModel*
	```c#
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

			public ListItem(bool boolean, string text)
			{
				_booleanPropery = boolean;
				_textProperty = text;
			}
		}
	}
	```
1. **ViewModelMain**
	* Add ```using System.Collections.ObjectModel;```
	* Add property of type ObserableCollection<ListItem>
	```c#
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
	```
	* Initialize **Listing** with data in the constructor
	```c#
	public ViewModelMain()
	{
		_TextProperty1 = "Enter text and press Enter...";

		_listing = new ObservableCollection<ListItem>();
		_listing.Add(new ListItem(true, "foo"));
		_listing.Add(new ListItem(false, "bar"));
	}
	```
1. Add ListBox to **View** and bind it
	```xaml
	<ListBox ItemsSource="{Binding Listing}" Height="100" Width="160" Margin="-200,-60,0,0">
		<ListBox.ItemTemplate>
			<DataTemplate>
				<StackPanel Orientation="Horizontal">
					<CheckBox IsChecked="{Binding BooleanProperty}" />
					<TextBlock Text="{Binding TextProperty}" Margin="10,0,0,0" Width="30" />
				</StackPanel>
			</DataTemplate>
		</ListBox.ItemTemplate>
	</ListBox>
	```

1. Add Command to **ViewModel**
	```c#
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
	```
1. Add Button to **View** and bind command
	```xaml
	<Button Command="{Binding AddListItemCommand}" Content="Add" Margin="80,-135,0,0" Width="75" Height="25"/>
	```

## ComboBox of enum in ListBox

1. Add file named **Enums.cs** to directory *Model*
	* Add enum
	```c#
	enum Citys { Bergen, Celle, Soltau, Karlsruhe, Donaueschingen, Ratingen }
	```

1. **ListItem**
	* Add ```using System;```
	* Add ```using System.Collections.ObjectModel;```
	* Add ```using MVVM_Training.Model;```
	* Add **CitysProperty**
	```c#
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
	```
	* Initialize Collection with the enum in the constructor
	```c#
	_citysProperty = new ObservableCollection<string>(Enum.GetNames(typeof(Citys)));
	```
1. Add ComboBox to **View**
	```xaml
	<ListBox ItemsSource="{Binding Listing}" Height="100" Width="160" Margin="-200,-60,0,0">
		<ListBox.ItemTemplate>
			<DataTemplate>
				<StackPanel Orientation="Horizontal">
					<CheckBox IsChecked="{Binding BooleanProperty}" />
					<TextBlock Text="{Binding TextProperty}" Margin="10,0,0,0" Width="30" />
					<ComboBox ItemsSource="{Binding CitysProperty}" SelectedIndex="0" Margin="10,0,0,0" Width="80" />
				</StackPanel>
			</DataTemplate>
		</ListBox.ItemTemplate>
	</ListBox>
	```

## Binding RadioButtons to enum

1. **Converter**
	* Add folder *Converter* to project
	* Add class **EnumToBooleanConverter**
	```c#
	using System;
	using System.Windows.Data;

	namespace MVVM_Collections.Converter
	{
		class EnumToBooleanConverter : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				return value.Equals(parameter);
			}

			public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
			{
				return value.Equals(true) ? parameter : Binding.DoNothing;
			}
		}
	}
	```

1. Add enum **Colours** to *Model/Enums.cs*
	```c#
	enum Colours { red, pink, purple}
	```
1. **ViewModel**
	* Add Property **FavouriteColour**
	```c#
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
	```
	* Initialize **FavouriteColour** in constructor
	```c#
	_favouriteColour = Colours.pink;
	```

1. **View**
	* Add namespace of converters
	```xaml
	xmlns:con="clr-namespace:MVVM_Collections.Converter"
	```
	* Add namespace of **Model**
	```xaml
	xmlns:m="clr-namespace:MVVM_Training.Model"
	```
	* Add converter to static resources
	```xaml
	<con:EnumToBooleanConverter x:Key="EnumToBooleanConverter" />
	```
	* Add RadioButtons and bind them
	```xaml
	<StackPanel Width="160" Height="50" Margin="-200,150,0,0">
		<RadioButton Content="Red" IsChecked="{Binding FavouriteColour, Converter={StaticResource EnumToBooleanConverter}, ConverterParameter={x:Static m:Colours.red}}" />
		<RadioButton Content="Pink" IsChecked="{Binding FavouriteColour, Converter={StaticResource EnumToBooleanConverter}, ConverterParameter={x:Static m:Colours.pink}}" />
		<RadioButton Content="Purple" IsChecked="{Binding FavouriteColour, Converter={StaticResource EnumToBooleanConverter}, ConverterParameter={x:Static m:Colours.purple}}" />
	</StackPanel>
	```
