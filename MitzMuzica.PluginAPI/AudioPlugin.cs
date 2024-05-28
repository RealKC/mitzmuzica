/**************************************************************************
 *                                                                        *
 *  Description: Interface that describes a plugin which can load audio   *
 *               files                                                    *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Mițca Dumitru                                  *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

namespace MitzMuzica.PluginAPI;

/// <summary>
/// An IAudioPlugin is a MitzMuzica plugin that can open audio files on a supported platform
/// </summary>
public interface IAudioPlugin
{
    /// <summary>
    /// The name of this plugin
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Opens the file given in the parameter as an audio file associated with this plugin
    /// </summary>
    /// <param name="path">The path to the audio file</param>
    /// <returns>A playable audio file</returns>
    public IAudioFile Open(string path);
}
