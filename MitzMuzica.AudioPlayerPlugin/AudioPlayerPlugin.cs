using MitzMuzica.PluginAPI;

namespace MitzMuzica.AudioPlayerPlugin;

public class AudioPlayerPlugin : IAudioPlugin
{
    
    private string _name = "AudioPlayerPlugin";
    public string Name { get { return _name; } }

    public IAudioFile Open(string path)
    {
        return new AudioPlayerFile(path);
    }
}
