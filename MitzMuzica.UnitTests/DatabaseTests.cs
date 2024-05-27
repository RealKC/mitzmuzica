/**************************************************************************
 *                                                                        *
 *  Description: Unit Tests for the Database class                        *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Petrisor Eduard-Gabriel                        *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using System.Reflection;
using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.UnitTests;

public class DatabaseTests
{
    IDatabase _db = new Database();

    private string _testDatabasePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                                   + "/testDB.db";
    [SetUp]
    public void Setup()
    {
        _db.CreateDatabase(_testDatabasePath);
    }
    
    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testDatabasePath))
        {
            File.Delete(_testDatabasePath);
        }
    }
    
    [Test]
    public void Test1InsertNewSong()
    {
        string title = "Test", path = "225200";

        _db.InsertNewSong(title, path);
    }
    
    [Test]
    public void Test2GetSongPath()
    {
        string songTitle = "Test", path = "225200";

        _db.InsertNewSong(songTitle, path);
        
        path = _db.GetSongPath(songTitle);
        
        Assert.IsTrue(path == "225200", 
            $"Valorile gasite sunt: Path: {path}");
    }
    
    [Test]
    public void Test2GetSongPathInvalidTitle()
    {
        string songTitle = "Testzulul";
        
        var ex = Assert.Catch<Exception>(() => _db.GetSongPath(songTitle));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test2GetSongPathInvalidIdentifier()
    {
        int songId = -15;
        
        var ex = Assert.Catch<Exception>(() => _db.GetSongPath(songId));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test3GetSongTitle()
    {
        string songTitle = "Test", path = "225200";

        _db.InsertNewSong(songTitle, path);
        
        songTitle = _db.GetSongTitle(1);
        
        Assert.IsTrue(songTitle == "Test", 
            $"Valorile gasite sunt: Path: {songTitle}");
    }
    
    [Test]
    public void Test3GetSongTitleInvalidIdentifier()
    {
        int songId = -1;
        
        var ex = Assert.Catch<Exception>(() => _db.GetSongTitle(songId));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test4GetSongIdentifier()
    {
        string songTitle = "Test", path = "225200";
        _db.InsertNewSong(songTitle, path);
        
        int songId = _db.GetSongId(songTitle);
        
        Assert.IsTrue(songId == 1, 
            $"Valorile gasite sunt: Path: {songTitle}");
    }
    
    [Test]
    public void Test4GetSongIdentifierInvalidTitle()
    {
        string songTitle = "Testzulu";
        
        var ex = Assert.Catch<Exception>(() => _db.GetSongId(songTitle));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
    
    [Test]
    public void Test5DeleteSong()
    {
        int songId = _db.InsertNewSong("Test2", "225201");
        
        _db.DeleteSong(songId);
    }
    
    [Test]
    public void Test5DeleteInexistentSong()
    {
        var ex = Assert.Catch<Exception>(() => _db.DeleteSong("Test23"));
        
        Assert.That(ex, Is.TypeOf<Exception>());
    }
}
