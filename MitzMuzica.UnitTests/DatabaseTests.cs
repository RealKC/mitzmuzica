using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.UnitTests;

public class DatabaseTests
{
    IDatabase db = new Database();

    [SetUp]
    public void Setup()
    {
        string path = "E:\\AC\\IP\\mitzmuzica\\MitzMuzica\\Resources\\playlistsDB.db";
        db.EstablishConnection(path);
    }

    [Test]
    public void TestGetSong()
    {
        int songId = 1;
        string title, path;

        (songId, title, path) = db.GetSong(songId);
        
        Assert.IsTrue(songId == 1 
                      && title == "Cantecul bobi dog"
                      && path == "penis", $"Valorile gasite sunt: {songId}, {title}, {path}");
    }
    
    [Test]
    public void TestInsertNewSong()
    {
        string title = "Paco", path = "taco 2 3";

        db.InsertNewSong(title, path);
    }
}
