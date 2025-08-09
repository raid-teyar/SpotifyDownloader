using Avalonia.Controls;
using Avalonia.ReactiveUI;
using SpotifyDownloader.ViewModels;

namespace SpotifyDownloader.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}