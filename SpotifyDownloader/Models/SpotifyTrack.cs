using System.Collections.Generic;

namespace SpotifyDownloader.Models;

public class SpotifyTrack
{
    public string Name { get; set; }
    public int Duration_ms { get; set; }
    public Album Album { get; set; }
    public List<Artist> Artists { get; set; }
}