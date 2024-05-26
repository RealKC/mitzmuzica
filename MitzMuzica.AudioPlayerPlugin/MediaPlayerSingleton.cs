using LibVLCSharp.Shared;
namespace MitzMuzica.AudioPlayerPlugin;

public class MediaPlayerSingleton
{
    private static MediaPlayerSingleton _instance;
    public MediaPlayer _player;
    public LibVLC _libVLC;

    private MediaPlayerSingleton()
    {
        //Initialize libvlc library and media player
        _libVLC = new LibVLC();
        _player = new MediaPlayer(_libVLC);
    }
    
    public static MediaPlayerSingleton Instance()
    {
        if (_instance == null)
        {
            _instance = new MediaPlayerSingleton();

        }
        return _instance;
    }
}
