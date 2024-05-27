/**************************************************************************
 *                                                                        *
 *  Description: Tests for the plugin loader                              *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Mițca Dumitru                                  *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using Xunit.Abstractions;

namespace MitzMuzica.PluginLoaderTest;

public class PluginLoaderTests
{
    private readonly PluginLoader.PluginLoader _pluginLoader = new("Plugins");
    private readonly ITestOutputHelper output;

    public PluginLoaderTests(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void CorrectPath()
    {
        Assert.Equal("Plugins", _pluginLoader.DirectoryPath);
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

    [Fact]
    public void UnimplementedMethods()
    {
        _pluginLoader.LoadPlugins();
        var plugin = _pluginLoader.AudioPlugins.First();
        var audioFile = plugin.Open("test");
        Assert.Throws<NotImplementedException>(() => audioFile.Start());
        Assert.Throws<NotImplementedException>(() => audioFile.Stop());
        Assert.Throws<NotImplementedException>(() => audioFile.SeekTo(0));
    }
}
