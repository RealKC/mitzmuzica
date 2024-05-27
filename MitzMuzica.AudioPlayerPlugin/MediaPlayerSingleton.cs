using LibVLCSharp.Shared;
namespace MitzMuzica.AudioPlayerPlugin;

public class MediaPlayerSingleton
{
    private static MediaPlayerSingleton _instance;
    public MediaPlayer _player;
    public LibVLC _libVLC;

    /// <summary>
    /// Constructor of the MediaPlayerSingleton class, initializes the libVLC library and the media player
    /// </summary>
    private MediaPlayerSingleton()
    {
        //Initialize libvlc library and media player
        _libVLC = new LibVLC();
        _player = new MediaPlayer(_libVLC);
    }
    
    /// <summary>
    /// Returns the instance of MediaPlayerSingleton class or creates it if it does not exist already
    /// </summary>
    public static MediaPlayerSingleton Instance()
    {
        if (_instance == null)
        {
            _instance = new MediaPlayerSingleton();

        }
        return _instance;
    }
}
