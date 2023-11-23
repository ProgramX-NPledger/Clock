namespace Clock.Maui.Utilities;

// from https://johnthiriet.com/removing-async-void/    
public static class TaskUtilities
{
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
    public static async void FireAndForgetSafeAsync(this Task task, object handler = null) // see https://johnthiriet.com/removing-async-void/ for errorhandler second param
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            //handler?.HandleError(ex);
        }
    }
}