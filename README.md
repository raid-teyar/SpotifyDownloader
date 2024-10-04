[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/O5O013MET3)
# Cross-Platform SpotifyDownloader

**Cross-Platform SpotifyDownloader** is a versatile desktop application designed to download Spotify playlist songs without requiring any registration or login information. This reworked version is built using Avalonia, .NET 8.0, and follows the MVVM design pattern with ReactiveUI for reactive programming. The application runs seamlessly on Windows, Linux, and macOS.
*None cross-plateform version i made in the past using WPF: https://github.com/raid-teyar/SpotifyPlaylistDownloader*

## How it Works

Downloading songs directly from Spotify requires a premium account, which is why this application takes a different approach:

1. **Spotify API for Metadata**: The app fetches a public token from the Spotify API and retrieves metadata for playlists and songs using the provided playlist or song link.

2. **YouTube Integration**: The application searches for the songs on YouTube by matching the song's name and length.

3. **High-Quality Downloads**: Songs are downloaded using the best available stream (highest bitrate) in `.webm` format.

4. **Format Conversion**: The `.webm` files are converted to `.mp3` format, with the album art embedded into the file.

## Features

- **Cross-Platform**: Runs on Windows, Linux, and macOS.
- **Reactive Programming**: Built with ReactiveUI, ensuring a responsive and fluid user experience.
- **MVVM Design Pattern**: Clean and maintainable codebase, adhering to modern software architecture practices.
- **Flexible Downloads**: Download entire playlists, albums or individual songs using their URLs.

## Screenshots

![image](https://user-images.githubusercontent.com/63502859/181786212-7db8eeac-dfea-4905-b31b-5b41104a1c71.png)

![image](https://user-images.githubusercontent.com/63502859/181786928-a0574b54-557a-4a65-95ba-5f9aa01a296b.png)

![image](https://user-images.githubusercontent.com/63502859/181787357-ab1a1ffe-ac49-4db5-9223-ca5584120260.png)

## Requirements

To build and run this application, you will need:

- .NET 8.0 or later
- A suitable IDE like JetBrains Rider or VS Code (for Linux/macOS)

## Future Work
- Embedding the album art into the downloaded songs.


## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.
