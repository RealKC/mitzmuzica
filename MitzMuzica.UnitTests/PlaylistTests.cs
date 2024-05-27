/**************************************************************************
 *                                                                        *
 *  Description: Unit Tests for the Playlist class                        *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Petrisor Eduard-Gabriel                        *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using System.Data.SqlClient;
using System.Reflection;
using MitzMuzica.DatabaseAPI;
using MitzMuzica.PlaylistAPI;

namespace MitzMuzica.UnitTests;

public class PlaylistTests
{
    private string testDatabasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                      + "/testDB.db";

    private IDatabase db = new Database();
    [SetUp]
    public void Setup()
    {
        db.CreateDatabase(testDatabasePath);
    }
    

    [Test]
    public void Test1CreatePlaylist()
    {
        IPlaylist playlist = new Playlist("Test", db);
        List<int> songlist = new List<int>();
        playlist.AddSongs(songlist);
    }
    
    [Test]
    public void Test2CreatePlaylistWithSqlInjection()
    {
        db.InsertNewSong("test", "path");
        IPlaylist playlist = new Playlist("Delete from songs;", db);
        List<int> songlist = new List<int>();
        songlist.Add(db.GetSongId("test"));
        playlist.AddSongs(songlist);
        
        Assert.IsNotEmpty(playlist.GetSongs(), "Injectia SQL a fost un succes!");
    }
    
    [Test]
    public void Test3CreatePlaylistWithInexistentSongs()
    {
        IPlaylist playlist = new Playlist("Test2", db);
        List<int> songlist = new List<int> { -1 };
        
        var ex = Assert.Catch<Exception>(() => playlist.AddSongs(songlist));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test4GetSonglist()
    {
        db.InsertNewSong("test22", "path");
        
        IPlaylist playlist = new Playlist("Test4", db);
        List<int> songlist = new List<int>();
        
        songlist.Add(db.GetSongId("test22"));
        playlist.AddSongs(songlist);
        List<int> results = playlist.GetSongs();
        
        Assert.IsNotEmpty(results, 
            "Nu au fost gasite elemente!");
    }
    
    [Test]
    public void Test5DeletePlaylist()
    {
        IPlaylist playlist1 = new Playlist("Test", db);
        playlist1.DeletePlaylist();
        
        IPlaylist playlist2 = new Playlist("Test2", db);
        playlist2.DeletePlaylist();
    }
    
    
}
