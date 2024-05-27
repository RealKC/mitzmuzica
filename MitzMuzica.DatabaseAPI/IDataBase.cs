using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public interface IDatabase
{
    private static SQLiteConnection? _connection = null;

    public void CreateDatabase();
    
    public void EstablishConnection(string path) { }
    
    public void InsertNewSong(string title, string path);
    
    public string GetSongPath(int s_id);
    public string GetSongPath(string title);

    public int GetSongID(string title);
    
    public void DeleteSong(int s_id);
    public void DeleteSong(string title);

    public int InsertNewPlaylist(string name, List<int> songIds);

    public List<int> GetPlaylist(int playlistId);
    public List<int> GetPlaylist(string name);

    public void DeletePlaylist(string name);
}
