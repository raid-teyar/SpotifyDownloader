namespace SpotifyDownloader.services;

public interface INotificationService
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
    void ShowFatal(string message);
}