using MitzMuzica.PlaylistAPI;

namespace MitzMuzica.UnitTests;

public class PlaylistTests
{
    [Test]
    public void TestDeleteSong()
    {
        IPlaylist playlist = new Playlist("bobi5");
        int[] songlist = { 1, 2, 3 };
        playlist.CreatePlaylist(songlist);
    }
}
