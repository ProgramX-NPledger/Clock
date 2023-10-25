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
	
	private async Task<SQLiteAsyncConnection> GetSqLiteConnection()
	{
		SQLiteAsyncConnection sqLiteConnection = new SQLiteAsyncConnection(_fileName);
		// this creation could be moved elsewhere
		CreateTableResult createTableResult=await sqLiteConnection.CreateTableAsync<WorkItem>();
		return sqLiteConnection;
	}
	
	internal async Task<IEnumerable<WorkItem>> GetLastNWorkItems(int lastN)
	{
		SQLiteAsyncConnection sqLiteConnection = await GetSqLiteConnection();
		List<WorkItem> results=await sqLiteConnection.Table<WorkItem>().OrderByDescending(q => q.StopTime).Take(lastN).ToListAsync();
		return results.ToArray();
	}
	
	internal async void AddCurrentWorkItemToDatabase(WorkItem workItem)
	{
		SQLiteAsyncConnection sqLiteConnection = await GetSqLiteConnection();
		int result = await sqLiteConnection.InsertAsync(workItem);
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