using MitzMuzica.PlaylistAPI;

namespace MitzMuzica.UnitTests;

public class PlaylistTests
{
    [Test]
    public void TestCreatePlaylist()
    {
        IPlaylist playlist = new Playlist("bobi10");
        int[] songlist = { 1, 2, 3 };
        playlist.CreatePlaylist(songlist);
    }
    
}
