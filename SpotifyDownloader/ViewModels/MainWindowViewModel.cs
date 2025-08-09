using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Notification;
using ReactiveUI;
using SpotifyDownloader.services;
using SpotifyDownloader.ViewModels;

namespace SpotifyDownloader.ViewModels;

public class MainWindowViewModel : ViewModelBase, IScreen
{
    public INotificationMessageManager Manager { get; }
    public ISpotifyService SpotifyService { get; }
    public RoutingState Router { get; } = new();
    public ReactiveCommand<Unit, IRoutableViewModel> Init { get; }
    public ReactiveCommand<Unit, IRoutableViewModel?> GoNext { get; }
    public ReactiveCommand<Unit, IRoutableViewModel?> GoBack { get; }
    
    private readonly Stack<IRoutableViewModel> _forwardStack = new();

    public MainWindowViewModel(INotificationMessageManager notificationMessageManager, ISpotifyService spotifyService)
    {
        Manager = notificationMessageManager;
        SpotifyService = spotifyService;
        
        var canGoBack = this.WhenAnyValue(
            x => x.Router.NavigationStack.Count,
            count => count > 1
        );

        var canGoNext = this.WhenAnyValue(
            x => x._forwardStack.Count,
            count => count > 0
        );

        GoBack = ReactiveCommand.CreateFromObservable(
            () =>
            {
                if (Router.NavigationStack.Count > 1)
                {
                    var currentViewModel = Router.GetCurrentViewModel();
                    if (currentViewModel != null)
                    {
                        _forwardStack.Push(currentViewModel);
                    }
                }
                return Router.NavigateBack.Execute();
            }, canGoBack);

        GoNext = ReactiveCommand.CreateFromObservable( () =>
        {
            if (_forwardStack.Count > 0)
            {
                var nextViewModel = _forwardStack.Pop();
                return Router.Navigate.Execute(nextViewModel);
            }
            
            return Observable.Return<IRoutableViewModel?>(null);
        });

        Init = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new SearchViewModel(this, spotifyService)));
        

        Init.Execute();
    }
}