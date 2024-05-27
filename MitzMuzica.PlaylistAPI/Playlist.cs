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

    public void AddSongs(List<int> songIds)
    {
        db.InsertNewPlaylist(PlaylistName, songIds);
    }
    
    public List<int> GetSongs()
    {
        return db.GetPlaylist(PlaylistName);
    }
    
    public void DeletePlaylist()
    {
        db.DeletePlaylist(PlaylistName);
    }
}
