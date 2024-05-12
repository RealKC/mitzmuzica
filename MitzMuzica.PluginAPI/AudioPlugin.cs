namespace MitzMuzica.PluginAPI;

public interface IAudioPlugin
{
    public string Name { get; }

    public IAudioFile Open(string path);
}
