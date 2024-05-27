/**************************************************************************
 *                                                                        *
 *  Description:  Implementation of IAudioPlugin for VLC audio player     *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Panciuc Ilie Cosmin                            *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using MitzMuzica.PluginAPI;

namespace MitzMuzica.AudioPlayerPlugin;

public class VlcPlayerPlugin : IAudioPlugin
{
    
    public string Name { get { return "VlcPlayerPlugin"; } }

    public IAudioFile Open(string path)
    {
        return new AudioPlayerFile(path);
    }
}
