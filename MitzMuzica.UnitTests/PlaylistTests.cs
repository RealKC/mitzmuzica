using MitzMuzica.PlaylistAPI;

namespace MitzMuzica.UnitTests;

public class PlaylistTests
{
    [Test]
    public void Test1CreatePlaylist()
    {
        IPlaylist playlist = new Playlist("Test");
        List<int> songlist = new List<int>();
        songlist.Add(1);
        playlist.AddSongs(songlist);
    }
    
    [Test]
    public void Test2GetSonglist()
    {
        IPlaylist playlist = new Playlist("Test2");
        List<int> songlist = new List<int>();
        songlist.Add(1);
        playlist.AddSongs(songlist);
        List<int> results = playlist.GetSongs();
        
        Assert.IsTrue("1" == string.Join(", ", results), 
            $"Elementele gasite: {results}");
    }
    
    [Test]
    public void Test3DeletePlaylist()
    {
        IPlaylist playlist = new Playlist("Test4");
        List<int> songlist = new List<int>();
        songlist.Add(1);
        playlist.AddSongs(songlist);
        playlist.DeletePlaylist();
    }
}
