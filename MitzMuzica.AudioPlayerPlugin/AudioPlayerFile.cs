/**************************************************************************
 *                                                                        *
 *  Description: Library that takes care of the play of media             *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Panciuc Ilie Cosmin                            *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using LibVLCSharp.Shared;
using MitzMuzica.PluginAPI;
namespace MitzMuzica.AudioPlayerPlugin;

public class AudioPlayerFile : IAudioFile
{
    
    private string _path;
    private string _title;
    private string _author;
    private long _length;
    
    /// <summary>
    /// _currentTime in seconds
    /// </summary>
    private long _currentTime = 0;
    
    private MediaPlayerSingleton _mediaPlayer;
    private Media _media;
    
    /// <summary>
    /// Returns the title of the file if it exists otherwise return "unknown"
    /// </summary>
    public string Title { get { return _title; } }
    
    /// <summary>
    /// Returns the author of the file if it exists otherwise return "unknown"
    /// </summary>
    public string Author { get { return _author; } }
    
    /// <summary>
    /// Returns the length of the file returns -1 if the length cant be determined from metadata
    /// </summary>
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
        _media = new Media(_mediaPlayer.LibVLC, _path);
        
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
        //start a thread playing the song
        Task.Run(() =>
        {
            _mediaPlayer.Player.Play(_media);
            //set the player time to _currentTime
            SeekTo(_currentTime);
        });
    }

    /// <summary>
    /// Pauses the current track
    /// </summary>
    public void Stop()
    {
        //save the time at witch the media is paused
        _currentTime =  _mediaPlayer.Player.Time / 1000;
        //pause media player
        _mediaPlayer.Player.Pause();

    }
    
    /// <summary>
    /// Sets the time of the currently playing media to s
    /// </summary>
    /// <param name="s">Timestamp in seconds</param>
    public void SeekTo(long s)
    {
        
        //Check if the number of seconds is valid
        if (s >= 0)
        {   
            //calculate number of milliseconds
            long m = s * 1000;
            //sets the time of the currently playing track to m milliseconds
            _mediaPlayer.Player.Time = m;
        }
    }
}
