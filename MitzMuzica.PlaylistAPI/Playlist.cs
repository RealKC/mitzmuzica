using System.Data.SqlClient;
using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.PlaylistAPI;

public class Playlist : IPlaylist
{
    public string PlaylistName { get; }

    public Playlist(string name)
    {
        PlaylistName = name;
    }

    public void CreatePlaylist(int[] songIds)
    {
        IDatabase db = new Database();
        db.InsertNewPlaylist(PlaylistName, songIds);
    }
}
