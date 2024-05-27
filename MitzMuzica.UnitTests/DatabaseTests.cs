using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.UnitTests;

public class DatabaseTests
{
    IDatabase db = new Database();
    [SetUp]
    public void Setup()
    {
        db.CreateDatabase();
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
    public void Test1InsertNewSong()
    {
        string title = "Test", path = "225200";

        db.InsertNewSong(title, path);
    }
    
    [Test]
    public void Test3DeleteSong()
    {
        
        int songId = db.InsertNewSong("Test2", "225201");
        
        db.DeleteSong(songId);
        
        db.InsertNewSong("Test3", "225202");
        
        db.DeleteSong("Test3");
    }
}
