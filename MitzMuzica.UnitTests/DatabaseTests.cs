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
    IDatabase db = new Database();
    [SetUp]
    public void Setup()
    {
        db.CreateDatabase(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) 
                          + "/testDB.db");
    }
    
    [Test]
    public void Test1InsertNewSong()
    {
        string title = "Test", path = "225200";

        db.InsertNewSong(title, path);
    }
    
    [Test]
    public void Test2GetSongPath()
    {
        string songTitle = "Test";
        string path;
        
        path = db.GetSongPath(songTitle);
        
        Assert.IsTrue(path == "225200", 
            $"Valorile gasite sunt: Path: {path}");
    }
    
    [Test]
    public void Test3DeleteSong()
    {
        
        int songId = db.InsertNewSong("Test2", "225201");
        
        db.DeleteSong(songId);
        
        db.DeleteSong("Test");
    }
}
