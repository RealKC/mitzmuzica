using System.Reflection;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.PluginLoader;

/// <summary>
/// The main entry point for loading plugins
/// </summary>
public class PluginLoader
{
    /// <summary>
    /// Creates a PluginLoader
    /// </summary>
    /// <param name="pluginDirectoryPath">The path where plugins are stored</param>
    public PluginLoader(string pluginDirectoryPath)
    {
        DirectoryPath = pluginDirectoryPath;
    }

    /// <summary>
    /// A list of all audio plugins
    /// </summary>
    public List<IAudioPlugin> AudioPlugins { get; private set; } = new();

    /// <summary>
    /// The path where plugins are stored
    /// </summary>
    public string DirectoryPath { get; }

    /// <summary>
    /// Method to load all plugins from <see cref="DirectoryPath"/>
    /// </summary>
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
