using Avalonia.Notification;
using Microsoft.Extensions.DependencyInjection;
using SpotifyDownloader.services;
using SpotifyDownloader.ViewModels;

namespace SpotifyDownloader;

public static class ServiceCollectionExtensions {
    public static void AddCommonServices(this IServiceCollection collection)
    {
        collection.AddSingleton<ISpotifyService, SpotifyService>();
        
        // crucial for the instance to be a singleton
        collection.AddSingleton<INotificationMessageManager, NotificationMessageManager>();
        collection.AddSingleton<INotificationService, NotificationService>();
        
        collection.AddTransient<MainWindowViewModel>();
    }
}