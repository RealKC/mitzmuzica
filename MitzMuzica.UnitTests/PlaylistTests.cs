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

    private IDatabase _db = new Database();
    [SetUp]
    public void Setup()
    {
        _db.CreateDatabase(testDatabasePath);
    }
    
    [TearDown]
    public void TearDown()
    {
        if (File.Exists(testDatabasePath))
        {
            File.Delete(testDatabasePath);
        }
    }

    [Test]
    public void Test1CreatePlaylist()
    {
        _db.InsertNewSong("test", "path");
        IPlaylist playlist = new Playlist("Test", _db);
        List<int> songlist = new List<int>();
        songlist.Add(_db.GetSongId("test"));
        playlist.AddSongs(songlist);
    }
    
    [Test]
    public void Test1CreatePlaylistNotEnoughSongs()
    {
        IPlaylist playlist = new Playlist("Test", _db);
        List<int> songlist = new List<int>();
        var ex = Assert.Catch<Exception>(() => playlist.AddSongs(songlist));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test1CreatePlaylistWithSqlInjection()
    {
        _db.InsertNewSong("test", "path");
        IPlaylist playlist = new Playlist("Delete from songs;", _db);
        List<int> songlist = new List<int>();
        songlist.Add(_db.GetSongId("test"));
        playlist.AddSongs(songlist);
        
        Assert.IsNotEmpty(playlist.GetSongs(), "Injectia SQL a fost un succes!");
    }
    
    [Test]
    public void Test1CreatePlaylistWithInexistentSongs()
    {
        IPlaylist playlist = new Playlist("Test2", _db);
        List<int> songlist = new List<int> { -1 };
        
        var ex = Assert.Catch<Exception>(() => playlist.AddSongs(songlist));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test2GetSonglist()
    {
        _db.InsertNewSong("test22", "path2");
        
        IPlaylist playlist = new Playlist("Test4", _db);
        List<int> songlist = new List<int>();
        
        songlist.Add(_db.GetSongId("test22"));
        playlist.AddSongs(songlist);
        
        List<int> results = playlist.GetSongs();
        
        Assert.IsNotEmpty(results, 
            "Nu au fost gasite elemente!");
    }
    
    [Test]
    public void Test2GetSonglistInvalidTitle()
    {
        IPlaylist playlist = new Playlist("Test42", _db);
        var ex = Assert.Catch<Exception>(() => playlist.GetSongs());
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test3DeletePlaylist()
    {
        _db.InsertNewSong("test", "path");
        IPlaylist playlist1 = new Playlist("Test", _db);
        List<int> songlist = new List<int> { 1 };
        
        playlist1.AddSongs(songlist);
        
        playlist1.DeletePlaylist();
    }
    
    [Test]
    public void Test3DeleteInexistentPlaylist()
    {
        IPlaylist playlist = new Playlist("Test3", _db);
        var ex = Assert.Catch<Exception>(() => playlist.DeletePlaylist());
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
}
