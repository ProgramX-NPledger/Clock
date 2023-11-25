using Clock.Maui.ViewModel;

namespace Clock.Maui.Behaviour;

/// <summary>
/// This behaviour allows conversion of the Entry.Completed event to a callable Command in the View Model,
/// in this case, the same functionality as pressing the Start/Stop button
/// More: https://learn.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/behaviors/creating
/// </summary>
public class ContentPageLoadedEventToCommandBehaviour : Behavior<ContentPage>
{
	protected override void OnAttachedTo(ContentPage bindable)
	{
		bindable.Loaded += BindableOnLoaded;
		base.OnAttachedTo(bindable);
	}


	protected override void OnDetachingFrom(ContentPage bindable)
	{
		bindable.Loaded -= BindableOnLoaded;
		base.OnDetachingFrom(bindable);
	}

	private void BindableOnLoaded(object sender, System.EventArgs e)
	{
		MainViewModel viewModel = (MainViewModel)((ContentPage)sender).BindingContext;
		viewModel.LoadLatestPersistedWorkItemsCommand.Execute(null);
		
	}
	
}