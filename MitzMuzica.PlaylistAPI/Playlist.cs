using System.Data.SqlClient;
using MitzMuzica.DatabaseAPI;

namespace MitzMuzica.PlaylistAPI;

public class Playlist : IPlaylist
{
    public string PlaylistName { get; }
    
    private IDatabase db = new Database();

    public Playlist(string name)
    {
        PlaylistName = name;
        db.CreateDatabase();
    }

    public void CreatePlaylist(List<int> songIds)
    {
        int p_id = db.InsertNewPlaylist(PlaylistName, songIds);
    }
    
    public void DeletePlaylist()
    {
        db.DeletePlaylist(PlaylistName);
    }
}
