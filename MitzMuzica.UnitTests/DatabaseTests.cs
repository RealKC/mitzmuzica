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
        int songId = 1;
        string songTitle = "Test";
        string path;

        path = db.GetSongPath(songId);
        
        Assert.IsTrue(path == "225200", 
                      $"Valorile gasite sunt: Path: {path}");
        
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
