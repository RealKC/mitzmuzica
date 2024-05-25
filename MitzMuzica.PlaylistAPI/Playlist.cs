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
        int p_id = db.InsertNewPlaylist(PlaylistName, songIds);
    }
}
