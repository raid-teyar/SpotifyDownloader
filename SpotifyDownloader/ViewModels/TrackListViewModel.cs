using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using HarfBuzzSharp;
using ReactiveUI;
using SpotifyDownloader.Controls;
using SpotifyDownloader.Models;
using SpotifyDownloader.services;

namespace SpotifyDownloader.ViewModels;

public class TrackListViewModel : ViewModelBase, IRoutableViewModel
{

    #region public properties

    public string? UrlPathSegment { get; } = "list";
    public IScreen HostScreen { get; }

    public List<SpotifyTrack> Songs { get; set; }
    public List<SongCard> SongCards { get; set; }
    
    public IObservable<bool> IsLoading => DownloadCommand.IsExecuting;

    #endregion
    
    #region private properties

    private readonly ISpotifyService _spotifyService;
    
    #endregion

    #region methods

    private void FillList()
    {
        foreach (var song in Songs)
        {
            SongCard card = new()
            {
                Title = song.Name,
                Artist = song.Artists[0].Name,
                Image = song.Album.Images[0].Url
            };
            
            SongCards.Add(card);
        }
    }

    #endregion


    #region commands

    public ReactiveCommand<Unit, Unit> DownloadCommand { get; }
    
    private async Task Download()
    {
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        var dialog = new SaveFileDialog();
        dialog.Title = "Select a folder to save the songs";
        dialog.Directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyMusic);
        dialog.InitialFileName = "Songs";
        var result = await dialog.ShowAsync(mainWindow);
        
        
        if (result != null)
        {
            foreach (var song in Songs)
            {
                await _spotifyService.DownloadTrack(song, result);
            }
        }
    }

    #endregion
    
    #region constructor

    public TrackListViewModel(IScreen screen, List<SpotifyTrack> songs, ISpotifyService spotifyService)
    {
        HostScreen = screen;
        Songs = songs;
        SongCards = new List<SongCard>();
        _spotifyService = spotifyService;
        
        FillList();
        
        DownloadCommand = ReactiveCommand.CreateFromTask(Download);
    }

    #endregion
    
}