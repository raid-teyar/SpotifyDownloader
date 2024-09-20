using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Notification;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyDownloader.Models;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Videos.Streams;

namespace SpotifyDownloader.services;

/// <summary>
/// Spotify Service that has all the Spotify API calls and methods
/// </summary>
public class SpotifyService : ISpotifyService
{
    private INotificationService _notificationService;

    private static readonly string spotifyBaseUrl = "https://open.spotify.com/";
    private static readonly string spotifyApiUrl = "https://api.spotify.com/";
    private static readonly string UserAgent = "SpotifySongDoawnloaderRuntime/1.0";
    private readonly int _divergence = 1000 * 60;

    // this a public token anyone can get by making a public call, still I know its bad practice to put it here
    private string _spotifyToken =
        "BQCZOHiFnSqDVqVYF9oL-d1lY93TJdLPGcTxuSmkOK9w0WmC5GN3cOZAm3V7JNUvPJMFZFP6JwpmvxqKZGMavujdHKlOClfREKJYOSKTBYY61VUupK4";

    public SpotifyService(INotificationService noticeMessageService)
    {
        _notificationService = noticeMessageService;
    }

    public string GetId(string link)
    {
        // playlist or song link is in the format of https://open.spotify.com/playlist/PlaylistID?si=78rFToSQSFClbrwpvSrXeA
        return link.Split("/")[4].Split("?")[0];
    }


    // gets the Spotify token used to make requests
    public async Task GetAuthToken()
    {
        HttpRequestMessage tokenRequest = new HttpRequestMessage(HttpMethod.Get,
            spotifyBaseUrl + "get_access_token?reason=transport&productType=web_player");
        tokenRequest.Headers.Add("User-Agent", UserAgent);

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.SendAsync(tokenRequest);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                JObject spotifyJsonToken = JObject.Parse(result);
                _spotifyToken = spotifyJsonToken.SelectToken("accessToken").ToString();
                _notificationService.ShowSuccess("Anonymous connection with Spotify established successfully!");
            }
            else
            {
                //_noticeMessageService.ShowFatal("There was an error when trying to get the access token :(");
            }
        }
    }
    
    // gets a single track using its id
    public async Task<SpotifyTrack?> GetTrack(string trackId)
    {
        string options = "?fields=name,duration_ms,artists(name),album(images(url))";
        HttpRequestMessage trackRequest = new HttpRequestMessage(HttpMethod.Get,
            spotifyApiUrl + $"v1/tracks/{trackId}" + options);
        trackRequest.Headers.Add("User-Agent", UserAgent);
        trackRequest.Headers.Add("Authorization", "Bearer " + _spotifyToken);

        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.SendAsync(trackRequest);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadAsStringAsync();
                    var responseJsonObject = JObject.Parse(result);
                    string? trackjsonString = responseJsonObject.ToString();

                    var track = JsonConvert.DeserializeObject<SpotifyTrack>(trackjsonString);
                    return track;

                case HttpStatusCode.Unauthorized:
                    _notificationService.ShowInfo("Token expired, getting a new one...");
                    await GetAuthToken();
                    var track2 = await GetTrack(trackId);
                    return track2;

                default:
                    _notificationService.ShowError("There was an error when trying to get the track :(");
                    return null;
            }
        }
    }
    

    // gets any Spotify playlist tracks using the given playlist id
    public async Task<List<SpotifyTrack>?> GetTracks(string playlistId)
    {
        string options = "?fields=items(track(name,duration_ms,artists(name),album(images(url))))";
        HttpRequestMessage playlistRequest = new HttpRequestMessage(HttpMethod.Get,
            spotifyApiUrl + $"v1/playlists/{playlistId}/tracks" + options);
        playlistRequest.Headers.Add("User-Agent", UserAgent);
        playlistRequest.Headers.Add("Authorization", "Bearer " + _spotifyToken);


        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.SendAsync(playlistRequest);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadAsStringAsync();
                    var responseJsonObject = JObject.Parse(result);
                    string? tracksjsonString = responseJsonObject.SelectToken("items")?.ToString();

                    // resulting json string needs "track" to be removed from the hierarchy before deserializing
                    JArray jsonArray = JArray.Parse(tracksjsonString);
                    foreach (JObject item in jsonArray)
                    {
                        JObject trackObject = (JObject)item["track"];
                        item.RemoveAll();
                        item.Merge(trackObject);
                    }
                    string modifiedJsonString = jsonArray.ToString();
                    
                    var tracksList = JsonConvert.DeserializeObject<List<SpotifyTrack>>(modifiedJsonString);
                    return tracksList;

                case HttpStatusCode.Unauthorized:
                    //_noticeMessageService.ShowInfo("Token expired, getting a new one...");
                    await GetAuthToken();
                    var trackList2 = await GetTracks(playlistId);
                    return trackList2;

                default:
                    //_noticeMessageService.ShowError("There was an error when trying to get the playlist tracks :(");
                    return null;
            }
        }
    }
    
    // using YoutubeExplode to download the track
    public async Task DownloadTrack(SpotifyTrack track, string path)
    {
        var youtubeClient = new YoutubeClient();
        VideoSearchResult chosenVideo = null;
        // join artist names for search
        await foreach(VideoSearchResult result in youtubeClient.Search.GetVideosAsync(track.Name + " - " + track.Artists[0].Name))
        {
            int resultAudioDuration = (int)result.Duration.Value.TotalMilliseconds;
            
            if (track.Duration_ms - _divergence > resultAudioDuration || resultAudioDuration > track.Duration_ms + _divergence)
            {
                continue;
            }
            
            chosenVideo = result;
            
            break;
        }
        
        var streamInfoSet = await youtubeClient.Videos.Streams.GetManifestAsync(chosenVideo.Url);
        var streamInfo = streamInfoSet.GetAudioOnlyStreams();
        var besStreamInfo = streamInfo.GetWithHighestBitrate();
        
        // Ensure the directory exists
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        await youtubeClient.Videos.Streams.DownloadAsync(besStreamInfo, path + "/" + track.Name + " - " + track.Artists.First().Name + ".mp3");
        _notificationService.ShowSuccess("Downloaded " + track.Name + " - " + track.Artists.First().Name + " successfully!");
    }
}