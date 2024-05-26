using LibVLCSharp.Shared;
using MitzMuzica.PluginAPI;
namespace MitzMuzica.AudioPlayerPlugin;

public class AudioPlayerPlugin : IAudioFile
{
    //Path-ul fisierului audio
    private string _path;
    private string _title;
    private string _author;
    private long _length;
    //Singleton media player
    private MediaPlayerSingleton _mediaPlayer;
    private Media _media;
    public string Title { get { return _title; } }
    public string Author { get { return _author; } }
    public long Length { get { return _length; } }

    public AudioPlayerPlugin(string path, string title, string author, long length)
    {
        //Initializeaza libraria LibVLC
        Core.Initialize();
        
        _path = path;
        _title = title;
        _author = author;
        _length = length;
        
        _mediaPlayer = MediaPlayerSingleton.Instance();
        _media = new Media(_mediaPlayer._libVLC, _path);

    }

    public void Start()
    {
        //stop any tracks already playing
        _mediaPlayer._player.Stop();
        //start media player
        _mediaPlayer._player.Play(_media);
    }

    public void Stop()
    {
        //stop media playerul
        _mediaPlayer._player.Stop();
    }

    public void SeekTo(long s)
    {
        //_mediaPlayer.SeekTo(System.TimeSpan(s));
    }
}
