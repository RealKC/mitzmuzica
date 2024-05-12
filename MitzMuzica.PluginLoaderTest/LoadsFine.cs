using Xunit.Abstractions;

namespace MitzMuzica.PluginLoaderTest;

public class LoadsFine
{
    private readonly PluginLoader.PluginLoader _pluginLoader = new("Plugins");
    private readonly ITestOutputHelper output;

    public LoadsFine(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void BasicBehaviour()
    {
        _pluginLoader.LoadPlugins();

        Assert.Single(_pluginLoader.AudioPlugins);

        var plugin = _pluginLoader.AudioPlugins.First();

        Assert.Equal("TestPlugin", plugin.Name);

        var audioFile = plugin.Open("test");
        Assert.Equal("test", audioFile.Title);
        Assert.Equal("kc", audioFile.Author);
        Assert.Equal(420, audioFile.Length);
    }
}
