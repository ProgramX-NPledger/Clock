using System.Collections.ObjectModel;

namespace Clock.Maui.Model;

public class WorkItemGroupByDate : ObservableCollection<WorkItem>
{
	public string Name { get; set; }
	public DateTime Date { get; set; }

	public WorkItemGroupByDate(string name, DateTime date, ObservableCollection<WorkItem> workItems)
		: base(workItems)
	{
		Name = name;
		Date = date;
	}
}