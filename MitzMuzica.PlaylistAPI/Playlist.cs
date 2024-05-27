/**************************************************************************
 *                                                                        *
 *  Description: Implementation of the IPlaylist Interface                *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Petrisor Eduard-Gabriel                        *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/
using System.Data.SqlClient;
using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.PlaylistAPI;

public class Playlist : IPlaylist
{
    public string PlaylistName { get; }

    private IDatabase db;
    
    /// <summary>
    /// Sets the playlist name and establishes a connection to the db.
    /// </summary>
    /// <param name="name">The name of the playlist.</param>
    /// <param name="database">Instance of a database.</param>
    public Playlist(string name, IDatabase database)
    {
        PlaylistName = name;
        db = database;
    }

    public void AddSongs(List<int> songIds)
    {
        db.InsertNewPlaylist(PlaylistName, songIds);
    }
    
    public List<int> GetSongs()
    {
        return db.GetPlaylist(PlaylistName);
    }
    
    public void DeletePlaylist()
    {
        db.DeletePlaylist(PlaylistName);
    }
}
