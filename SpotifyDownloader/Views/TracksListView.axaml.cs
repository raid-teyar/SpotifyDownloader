using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SpotifyDownloader.ViewModels;

namespace SpotifyDownloader.Views;

public partial class TracksListView : ReactiveUserControl<TrackListViewModel>
{
    public TracksListView()
    {
        InitializeComponent();
    }
}