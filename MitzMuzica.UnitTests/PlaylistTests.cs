using MitzMuzica.DatabaseAPI;
using MitzMuzica.PlaylistAPI;

namespace MitzMuzica.UnitTests;

public class PlaylistTests
{
    [Test]
    public void Test1CreatePlaylist()
    {
        IPlaylist playlist = new Playlist("Test");
        List<int> songlist = new List<int>();
        playlist.AddSongs(songlist);
    }
    
    [Test]
    public void Test2GetSonglist()
    {
        IPlaylist playlist = new Playlist("Test2");
        List<int> songlist = new List<int>();
        playlist.AddSongs(songlist);
        List<int> results = playlist.GetSongs();
        
        Assert.IsTrue("" == string.Join(", ", results), 
            $"Elementele gasite: {results}");
    }
    
    [Test]
    public void Test3DeletePlaylist()
    {
        IPlaylist playlist1 = new Playlist("Test");
        IPlaylist playlist2 = new Playlist("Test2");

        playlist1.DeletePlaylist();
        playlist2.DeletePlaylist();
    }
}
