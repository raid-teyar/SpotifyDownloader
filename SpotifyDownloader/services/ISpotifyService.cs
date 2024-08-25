using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyDownloader.Models;

namespace SpotifyDownloader.services;

public interface ISpotifyService
{
    Task GetAuthToken();
    Task<List<SpotifyTrack>?> GetTracks(string playlistId);
    Task<SpotifyTrack?> GetTrack(string trackId);
    string GetId(string link);

    Task DownloadTrack(SpotifyTrack track, string path);
}