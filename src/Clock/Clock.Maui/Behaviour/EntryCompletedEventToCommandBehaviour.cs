using Clock.Maui.ViewModel;

namespace Clock.Maui.Behaviour;

/// <summary>
/// This behaviour allows conversion of the Entry.Completed event to a callable Command in the View Model,
/// in this case, the same functionality as pressing the Start/Stop button
/// </summary>
public class EntryCompletedEventToCommandBehaviour : Behavior<Entry>
{
	protected override void OnAttachedTo(Entry bindable)
	{
		bindable.Completed += OnCompleted;
		base.OnAttachedTo(bindable);
	}

	protected override void OnDetachingFrom(Entry bindable)
	{
		bindable.Completed -= OnCompleted;
		base.OnDetachingFrom(bindable);
	}

	private void OnCompleted(object sender, EventArgs e)
	{
		MainViewModel viewModel = (MainViewModel)((Entry)sender).BindingContext;
		viewModel.StartStopButtonCommand.Execute(null);
		
	}
}