namespace MitzMuzica.PluginAPI;

public interface IAudioFile
{
    public string Title { get; }
    public string Author { get; }
    public long Length { get; }

    public void Start();
    public void Stop();
    public void SeekTo(long s);
}
