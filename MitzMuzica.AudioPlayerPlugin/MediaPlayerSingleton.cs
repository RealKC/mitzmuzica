/**************************************************************************
 *                                                                        *
 *  Description: Initializes the vlc library and the media player         *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Panciuc Ilie Cosmin                            *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using LibVLCSharp.Shared;
namespace MitzMuzica.AudioPlayerPlugin;

public class MediaPlayerSingleton
{
    private static MediaPlayerSingleton _instance;
    public MediaPlayer Player;
    public LibVLC LibVLC;

    /// <summary>
    /// Constructor of the MediaPlayerSingleton class, initializes the libVLC library and the media player
    /// </summary>
    private MediaPlayerSingleton()
    {
        //Initialize libvlc library and media player
        LibVLC = new LibVLC();
        Player = new MediaPlayer(LibVLC);
    }
    
    /// <summary>
    /// Returns the instance of MediaPlayerSingleton class or creates it if it does not exist already
    /// </summary>
    public static MediaPlayerSingleton Instance()
    {
        if (_instance == null)
        {
            _instance = new MediaPlayerSingleton();

        }
        return _instance;
    }
}
