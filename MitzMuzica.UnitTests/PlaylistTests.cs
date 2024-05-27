using MitzMuzica.PlaylistAPI;

namespace MitzMuzica.UnitTests;

public class PlaylistTests
{
    [Test]
    public void Test1CreatePlaylist()
    {
        IPlaylist playlist = new Playlist("bobi");
        List<int> songlist = new List<int>();
        songlist.Add(3);
        songlist.Add(4);
        playlist.AddSongs(songlist);
    }
    
    [Test]
    public void Test2GetSonglist()
    {
        IPlaylist playlist = new Playlist("bobi2");
        List<int> songlist = new List<int>();
        songlist.Add(3);
        songlist.Add(4);
        playlist.AddSongs(songlist);
    }
    
    [Test]
    public void Test3DeletePlaylist()
    {
        IPlaylist playlist = new Playlist("bobi3");
        playlist.DeletePlaylist();
    }
    
    [Test]
    public void Test4CascadeDeleteInQueue()
    {
        IPlaylist playlist = new Playlist("bobi4");
        List<int> songlist = new List<int>();
        songlist.Add(3);
        playlist.AddSongs(songlist);
        playlist.DeletePlaylist();
    }
    
}
