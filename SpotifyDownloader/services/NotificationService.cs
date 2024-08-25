using System;
using Avalonia.Notification;

namespace SpotifyDownloader.services;

public class NotificationService : INotificationService
{
    private INotificationMessageManager _noticeMessageService;

    public NotificationService(INotificationMessageManager notificationMessageManager)
    {
        _noticeMessageService = notificationMessageManager;
    }
    
    public void ShowInfo(string message)
    {
        _noticeMessageService.CreateMessage()
            .HasMessage(message)
            .Accent("#1751C3")
            .Animates(true)
            .Background("#333")
            .HasBadge("Info")
            .Dismiss().WithButton("ok", button => { })
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
    }
    
    public void ShowSuccess(string message)
    {
        // dark green not light
        _noticeMessageService.CreateMessage()
            .HasMessage(message)
            .Accent("#006600")
            .Animates(true)
            .Background("#333")
            .HasBadge("Success")
            .Dismiss().WithButton("ok", button => { })
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
        
    }

    public void ShowError(string message)
    {
        _noticeMessageService.CreateMessage()
            .HasMessage(message)
            .Accent("#cc0000")
            .Animates(true)
            .Background("#333")
            .HasBadge("Error")
            .Dismiss().WithButton("ok", button => { })
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
        
    }

    public void ShowFatal(string message)
    {
        _noticeMessageService.CreateMessage()
            .HasMessage(message)
            .Accent("#cc0000")
            .Animates(true)
            .Background("#333")
            .HasBadge("Fatal")
            .Dismiss().WithButton("ok", button => { })
            .Dismiss().WithDelay(TimeSpan.FromSeconds(5))
            .Queue();
    }
}