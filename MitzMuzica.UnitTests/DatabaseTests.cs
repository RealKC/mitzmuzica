using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.UnitTests;

public class DatabaseTests
{
    IDatabase db = new Database();
    [SetUp]
    public void Setup()
    {
        string path = "..\\..\\..\\..\\MitzMuzica\\Resources\\playlistsDB.db";
        db.CreateDatabase();
        db.EstablishConnection(path);
    }
    [Test]
    public void Test2GetSongPath()
    {
        int songId = 1;
        string path;

        path = db.GetSongPath(songId);
        
        Assert.IsTrue(songId == 1 
                      && path == "225200", 
                      $"Valorile gasite sunt: ID: {songId}, Path: {path}");
    }
    
    [Test]
    public void Test1InsertNewSong()
    {
        string title = "Pacod", path = "225200";

        db.InsertNewSong(title, path);
    }
    
    [Test]
    public void Test3DeleteSong()
    {
        int s_id = db.GetSongID("Pacod");

        db.DeleteSong(s_id);
    }
    
    [Test]
    public void Test4GetPlaylist()
    {
        int p_id = 1;

        List<int> temp =  db.GetPlaylist(p_id);
        string results = string.Join(", ", temp);
        Assert.IsTrue("1, 2, 3" == results, $"Elementele gasite: {results}");
    }
}
