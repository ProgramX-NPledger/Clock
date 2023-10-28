using Clock.Maui.Data;

namespace Clock.Maui;

public partial class App : Application
{
    public static WorkItemRepository WorkItemRepository { get; private set; }
    
    public App(WorkItemRepository workItemRepository)
    {
        InitializeComponent();
        MainPage = new NavigationPage(new MainPage());
        WorkItemRepository = workItemRepository;
    }
}