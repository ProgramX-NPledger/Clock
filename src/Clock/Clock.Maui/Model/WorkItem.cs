using SQLite;

namespace Clock.Maui.Model;

[Table("WorkItem")]
public class WorkItem
{
	[PrimaryKey, AutoIncrement, Column("Id")]
	public int Id { get; set; }

	[NotNull] public DateTime StartTime { get; set; }
	
	[NotNull] public DateTime StopTime { get; set; }
	
	[NotNull] public TimeSpan RecordedTime { get; set; }
	
	public string Title { get; set; }
	
}