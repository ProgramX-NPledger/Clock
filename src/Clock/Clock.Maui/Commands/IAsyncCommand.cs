using System.Windows.Input;

namespace Clock.Maui.Commands;

public interface IAsyncCommand : ICommand
{
    Task ExecuteAsync();
    bool CanExecute();
}