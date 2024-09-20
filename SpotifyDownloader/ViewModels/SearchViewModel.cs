using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Notification;
using ReactiveUI;
using SpotifyDownloader.Helpers;
using SpotifyDownloader.Models;
using SpotifyDownloader.services;

namespace SpotifyDownloader.ViewModels;

public class SearchViewModel : ViewModelBase, IRoutableViewModel
{
    #region Private Properties

    private ISpotifyService _spotifyService;

    #endregion
    
    #region Public Properties

    public IScreen HostScreen { get; }
    public string UrlPathSegment { get; } = "search";
    public IObservable<bool> IsLoading => SearchCommand.IsExecuting;
    
    private string? _searchInput = "";
    public string? SearchInput
    {
        get => _searchInput;
        set 
        {
            value?.IsValid(DataTypes.SearchInput);
            this.RaiseAndSetIfChanged(ref _searchInput, value);
        }
    }

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> SearchCommand { get; }
    
    private async Task Search()
    {
        string id = _spotifyService.GetId(SearchInput);

        if (SearchInput.Contains("playlist"))
        {
            var result = await _spotifyService.GetTracks(id);
            HostScreen.Router.Navigate.Execute(new TrackListViewModel(HostScreen, result, _spotifyService));
        }
        else
        {
            var result = await _spotifyService.GetTrack(id);
            HostScreen.Router.Navigate.Execute(new TrackListViewModel(HostScreen, new List<SpotifyTrack> { result }, _spotifyService));
        }
    }

    #endregion
    
    #region Methods
    
    private IObservable<bool> CanSearch()
    {
        // can search only when the search input is not empty
        return this.WhenAnyValue(
            x => x.SearchInput,
            searchInput => 
                !string.IsNullOrEmpty(searchInput));
    }
    
    #endregion

    #region Constructor

    public SearchViewModel(IScreen screen, ISpotifyService spotifyService)
    {
        HostScreen = screen;
        _spotifyService = spotifyService;

        SearchCommand = ReactiveCommand.CreateFromTask(Search, CanSearch());
    }
    #endregion

}