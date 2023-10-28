using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Clock.Maui.ViewModel;

public class ReportViewerPageViewModel: INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
	public event EventHandler<EventArgs> RequestClose;
	
	private string _reportText;

	public string ReportText
	{
		get => _reportText;
		set
		{
			if (_reportText != value)
			{
				_reportText = value;
				OnPropertyChanged();
			}
		}
	}
	
	protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) return false;
		field = value;
		OnPropertyChanged(propertyName);
		return true;
	}

	public ICommand OkCommand { get; private set; }
	
	public ReportViewerPageViewModel()
	{
		OkCommand = new Command(() => OnRequestClose());
	}

	protected virtual void OnRequestClose()
	{
		RequestClose?.Invoke(this, EventArgs.Empty);
	}
}