﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Clock.Maui.Model;

namespace Clock.Maui.ViewModel;

public class ReportOptionsViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;
	public event EventHandler<ReportEventArgs> ReportRequired;
		
	private bool _beginEachLineWithABullet;
	private bool _includeFieldsStartTime;
	private bool _includeFieldsStopTime;
	private bool _includeFieldsRecordedTime;
	private bool _includeFieldsTitle;
	private bool _reportFormatTsv;
	private bool _reportFormatCsv;
	private bool _quoteFieldsWithSpaces;
	private bool _fromDateSelected;
	private bool _toDateSelected;
	private DateTime _fromDate;
	private DateTime _toDate;

	public bool FromDateSelected
	{
		get => _fromDateSelected;
		set
		{
			if (_fromDateSelected != value)
			{
				_fromDateSelected = value;
				OnPropertyChanged();
			}
		}
	}

	public bool ToDateSelected
	{
		get => _toDateSelected;
		set
		{
			if (_toDateSelected != value)
			{
				_toDateSelected = value;
				OnPropertyChanged();
			}
		}
	}

	public DateTime FromDate
	{
		get => _fromDate;
		set
		{
			if (_fromDate != value)
			{
				_fromDate = value;
				OnPropertyChanged();
			}
		}
	}

	public DateTime ToDate
	{
		get => _toDate;
		set
		{
			if (_toDate != value)
			{
				_toDate = value;
				OnPropertyChanged();
			}
		}
	}
	
	public bool QuoteFieldsWithSpaces
	{
		get => _quoteFieldsWithSpaces;
		set
		{
			if (_quoteFieldsWithSpaces != value)
			{
				_quoteFieldsWithSpaces = value;
				OnPropertyChanged();
			}
		}
	}
	
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

	public ReportOptionsViewModel()
	{
		GenerateButtonCommand = new Command(() => { GenerateText(); });
		
	}

	private void GenerateText()
	{
		// raise an event to have the consuming code-behind generate the data
		
		ReportEventArgs reportEventArgs = new ReportEventArgs()
		{
			Options = new ReportOptions()
			{
				IncludeBullets = BeginEachLineWithABullet,
				QuoteFieldsWithSpaces = QuoteFieldsWithSpaces,
				IncludeFields = GetIncludedFields(),
				FromDate = FromDateSelected ? FromDate : null,
				ToDate = ToDateSelected ? ToDate : null
			},
			ReportFormat = GetReportFormat()
		};
		ReportRequired?.Invoke(this,reportEventArgs);

	}

	private ReportFormat GetReportFormat()
	{
		if (ReportFormatCsv) return ReportFormat.Csv;
		if (ReportFormatTsv) return ReportFormat.Tsv;
		throw new InvalidOperationException("Invalid report format");
	}

	private string[] GetIncludedFields()
	{
		List<string> fields = new List<string>();
		if (IncludeFieldsTitle) fields.Add("Title");
		if (IncludeFieldsRecordedTime) fields.Add("RecordedTime");
		if (IncludeFieldsStopTime) fields.Add("StopTime");
		if (IncludeFieldsStartTime) fields.Add("StartTime");
		return fields.ToArray();
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