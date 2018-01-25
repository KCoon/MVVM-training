# MVVM-training

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
