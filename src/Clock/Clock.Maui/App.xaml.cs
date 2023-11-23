using Clock.Maui.Data;
using Clock.Maui.Model;
using Clock.Maui.Services;

namespace Clock.Maui;

public partial class App : Application
{
    public static WorkItemRepository WorkItemRepository { get; private set; }
    
    public App(WorkItemRepository workItemRepository)
    {
        InitializeComponent();
        Task.Run(async () =>
        {
            // async operations cannot be run before the UI is displayed,
            // except in this context
            // https://stackoverflow.com/questions/74500117/net-maui-asynchronous-initialization-issue-async-await

            // bool updatesEnabled = Preferences.Get(GitHubUpdateService.AUTOUPDATE_ENABLED_CONFIG_STRING,
            //     false);
            // updatesEnabled = true;
            // // start service
            // if (updatesEnabled)
            // {
            //     bool preferPreRelease = Preferences.Get(GitHubUpdateService.PREFER_PRERELEASE_CONFIG_STRING, false);
            //     preferPreRelease = true;
            //     using (GitHubUpdateService gitHubUpdateService = new GitHubUpdateService())
            //     {
            //         AvailableUpdateStatus latestUpdate = await gitHubUpdateService.GetUpdateStatus(preferPreRelease);
            //         if (latestUpdate.IsUpdateAvailable())
            //         {
            //             
            //         }
            //         
            //         // service should have current version number
            //         // and be able to get latest version for configured channel from GitHub
            //         
            //     }
            //     
            //
            // }
        });
        MainPage = new NavigationPage(new MainPage());
        WorkItemRepository = workItemRepository;
    }
}