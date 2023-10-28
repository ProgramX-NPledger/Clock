using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clock.Maui.ViewModel;

namespace Clock.Maui;

public partial class ReportViewerPage : ContentPage
{
    public ReportViewerPage()
    {
        InitializeComponent();

        ReportViewerPageViewModel viewModel = (ReportViewerPageViewModel)BindingContext;

        viewModel.RequestClose += async (s, e) =>
        {
            await Navigation.PopToRootAsync();
        };

    }
}