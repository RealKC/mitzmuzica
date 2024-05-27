using MitzMuzica.PlaylistAPI;

namespace MitzMuzica.UnitTests;

public class PlaylistTests
{
    [Test]
    public void Test1CreatePlaylist()
    {
        IPlaylist playlist = new Playlist("bobi11");
        List<int> songlist = new List<int>();
        songlist.Add(3);
        songlist.Add(1);
        playlist.CreatePlaylist(songlist);
    }
    
    [Test]
    public void Test2DeletePlaylist()
    {
        IPlaylist playlist = new Playlist("bobi10");
        playlist.DeletePlaylist();
    }
    
    [Test]
    public void Test3CascadeDeleteInQueue()
    {
        IPlaylist playlist = new Playlist("bobi12");
        List<int> songlist = new List<int>();
        songlist.Add(3);
        playlist.CreatePlaylist(songlist);
        playlist.DeletePlaylist();
    }
    
}
