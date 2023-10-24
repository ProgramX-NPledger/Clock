using Clock.Maui.Model;
using SQLite;

namespace Clock.Maui.Data;

public class WorkItemRepository
{
	private readonly string _fileName;

	public WorkItemRepository(string fileName)
	{
		_fileName = fileName;
	}
	
	private SQLiteConnection GetSqLiteConnection()
	{
		SQLiteConnection sqLiteConnection = new SQLiteConnection(_fileName);
		// this creation could be moved elsewhere
		CreateTableResult createTableResult=sqLiteConnection.CreateTable<WorkItem>();
		return sqLiteConnection;
	}
	
	internal IEnumerable<WorkItem> GetLastNWorkItems(int lastN)
	{
		SQLiteConnection sqLiteConnection = GetSqLiteConnection();
		TableQuery<WorkItem> results=sqLiteConnection.Table<WorkItem>().OrderByDescending(q => q.StopTime).Take(lastN);
		return results.ToArray();
	}
	
	internal void AddCurrentWorkItemToDatabase(WorkItem workItem)
	{
		SQLiteConnection sqLiteConnection = GetSqLiteConnection();
		int result = sqLiteConnection.Insert(workItem);
		switch (result)
		{
			case < 1:
				// something went wrong
				throw new InvalidOperationException("Expected 1 update, but 0 were effected");
			case > 1:
				// more updates than were expected
				throw new InvalidOperationException($"Expected 1 update, but {result} were effected");
		}
        
        
	}
	
}