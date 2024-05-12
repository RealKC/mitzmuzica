using System.Reflection;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.PluginLoader;

public class PluginLoader
{
    public PluginLoader(string pluginDirectoryPath)
    {
        DirectoryPath = pluginDirectoryPath;
    }

    public List<IAudioPlugin> AudioPlugins { get; private set; } = new();

    public string DirectoryPath { get; }

    public void LoadPlugins()
    {
        var plugins = from file in Directory.EnumerateFiles(DirectoryPath, "*.dll", SearchOption.AllDirectories)
            from type in LoadPlugin(Path.GetFullPath(file)).GetTypes()
            where typeof(IAudioPlugin).IsAssignableFrom(type)
            select Activator.CreateInstance(type) as IAudioPlugin;

        AudioPlugins = plugins.ToList();
    }

    private static Assembly LoadPlugin(string absolutePath)
    {
        var loadContext = new PluginLoadContext(absolutePath);
        return loadContext.LoadFromAssemblyPath(absolutePath);
    }
}
