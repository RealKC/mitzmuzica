namespace MitzMuzica.PluginAPI;

/// <summary>
/// An audio file as returned by a plugin
/// </summary>
public interface IAudioFile
{
    /// <summary>
    /// Best-effort song title from file metadata
    /// </summary>
    public string Title { get; }
    
    /// <summary>
    /// Best-effort song author/artist from file metadata
    /// </summary>
    public string Author { get; }
    
    /// <summary>
    /// Best-effort song length in seconds from file metadata
    /// </summary>
    public long Length { get; }

    /// <summary>
    /// Starts the playback of this audio file
    /// </summary>
    public void Start();
    
    /// <summary>
    /// Stops this file from playing 
    /// </summary>
    public void Stop();
    
    /// <summary>
    /// Seeks to a given offset in the audio file
    /// </summary>
    /// <param name="s">the offset, in seconds</param>
    public void SeekTo(long s);
}
