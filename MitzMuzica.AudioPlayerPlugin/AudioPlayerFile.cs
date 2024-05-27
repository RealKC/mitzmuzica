using LibVLCSharp.Shared;
using MitzMuzica.PluginAPI;
namespace MitzMuzica.AudioPlayerPlugin;

public class AudioPlayerFile : IAudioFile
{
    
    private string _path;
    private string _title;
    private string _author;
    private long _length;
    private MediaPlayerSingleton _mediaPlayer;
    private Media _media;
    public string Title { get { return _title; } }
    public string Author { get { return _author; } }
    public long Length { get { return _length; } }
    
    /// <summary>
    /// Constructor of AudioPlayerPlugin class initializes the _mediaPlayer with the instance of MediaPlayerSingleton
    /// Creates the media object, initializes all other fields based on the metadata of the media object
    /// </summary>
    /// <param name="path">The path to the file</param>
    public AudioPlayerFile(string path)
    {
        Core.Initialize();
        
        _path = path;
        
        _mediaPlayer = MediaPlayerSingleton.Instance();
        _media = new Media(_mediaPlayer._libVLC, _path);
        
        _media.Parse().Wait();
        
        var title = _media.Meta(MetadataType.Title);
        _title = String.IsNullOrEmpty(title) ? "unknown" : title;
        var author = _media.Meta(MetadataType.Artist);
        _author = String.IsNullOrEmpty(author) ? "unknown" : author;
        
        _length = _media.Duration;
    }

    /// <summary>
    /// Stops any track currently playing on the media player and starts playing the content of the media object
    /// </summary>
    public void Start()
    {
        //stop any tracks already playing
        _mediaPlayer._player.Stop();
        //start media player
        _mediaPlayer._player.Play(_media);
    }

    /// <summary>
    /// Pauses the current track
    /// </summary>
    public void Stop()
    {
        //stop media playerul
        _mediaPlayer._player.Pause();
    }
    
    /// <summary>
    /// Sets the time of the currently playing media to s
    /// </summary>
    /// <param name="s">Timestamp in milliseconds</param>
    public void SeekTo(long s)
    {
        //sets the time of the currently playing track to s milliseconds
        if (s <= _length && s >= 0)
        {
            _mediaPlayer._player.Time = s;
        }
    }
}
