using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.UnitTests;

public class DatabaseTests
{
    IDatabase db = new Database();

    [SetUp]
    public void Setup()
    {
        string path = "..\\..\\..\\..\\MitzMuzica\\Resources\\playlistsDB.db";
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
                      && path == "denis", 
                      $"Valorile gasite sunt: ID: {songId}, Title: {title}, Path: {path}");
    }
    
    [Test]
    public void TestInsertNewSong()
    {
        string title = "Pacod", path = "225200";

        db.InsertNewSong(title, path);
    }
    
    [Test]
    public void TestDeleteSong()
    {
        int s_id = db.GetSongID("Pacod");

        db.DeleteSong(s_id);
    }
}
