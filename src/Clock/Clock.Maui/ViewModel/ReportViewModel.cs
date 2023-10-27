using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Clock.Maui.ViewModel;

public class ReportViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	private bool _beginEachLineWithABullet;
	private bool _includeFieldsStartTime;
	private bool _includeFieldsStopTime;
	private bool _includeFieldsRecordedTime;
	private bool _includeFieldsTitle;
	private bool _reportFormatTsv;
	private bool _reportFormatCsv;
	private string _generatedText;

	public bool ReportFormatTsv
	{
		get => _reportFormatTsv;
		set
		{
			if (_reportFormatTsv != value)
			{
				_reportFormatTsv = value;
				OnPropertyChanged();
			}
		}
	}

	public bool ReportFormatCsv
	{
		get => _reportFormatCsv;
		set
		{
			if (_reportFormatCsv != value)
			{
				_reportFormatCsv = value;
				OnPropertyChanged();
			}
		}
	}
	
	public string GeneratedText
	{
		get => _generatedText;
		set
		{
			if (_generatedText != value)
			{
				_generatedText = value;
				OnPropertyChanged();
			}
		}
	}
	
	public bool IncludeFieldsStartTime
	{
		get => _includeFieldsStartTime;
		set
		{
			if (_includeFieldsStartTime != value)
			{
				_includeFieldsStartTime = value;
				OnPropertyChanged();
			}
		}
	}

	public bool IncludeFieldsStopTime
	{
		get => _includeFieldsStopTime;
		set
		{
			if (_includeFieldsStopTime != value)
			{
				_includeFieldsStopTime=value;
				OnPropertyChanged();
			}
		}
	}

	public bool IncludeFieldsRecordedTime
	{
		get => _includeFieldsRecordedTime;
		set
		{
			if (_includeFieldsRecordedTime != value)
			{
				_includeFieldsRecordedTime = value;
				OnPropertyChanged();
			}
		}
	}

	public bool IncludeFieldsTitle
	{
		get => _includeFieldsTitle;
		set
		{
			if (_includeFieldsTitle != value)
			{
				_includeFieldsTitle = value;
				OnPropertyChanged();
			}
		}
	}
	
	public bool BeginEachLineWithABullet
	{
		get => _beginEachLineWithABullet;
		set
		{
			if (_beginEachLineWithABullet!=value)
			{
				_beginEachLineWithABullet = value;
				OnPropertyChanged();
			}
		}
	}
	
	public ICommand GenerateButtonCommand { get; private set; }

	public ReportViewModel()
	{
		GenerateButtonCommand = new Command(() => { GenerateText(); });
		
	}

	private void GenerateText()
	{
		GeneratedText = $"Hello {(ReportFormatCsv ? "CSV" : (ReportFormatTsv ? "TSV" : "????"))}";
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
}