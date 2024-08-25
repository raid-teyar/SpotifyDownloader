using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace SpotifyDownloader.Controls;

public partial class SongCard : UserControl
{
    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<SongCard, string>(nameof(Title));
    public static readonly StyledProperty<string> ArtistProperty = AvaloniaProperty.Register<SongCard, string>(nameof(Artist));
    public static readonly StyledProperty<string> ImageProperty = AvaloniaProperty.Register<SongCard, string>(nameof(Image));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Artist
    {
        get => GetValue(ArtistProperty);
        set => SetValue(ArtistProperty, value);
    }

    public string Image
    {
        get => GetValue(ImageProperty);
        set => SetValue(ImageProperty, value);
    }

    public SongCard()
    {
        DataContext = this;
    }
  
}