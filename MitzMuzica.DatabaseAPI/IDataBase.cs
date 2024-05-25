using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public interface IDatabase
{
    private static SQLiteConnection? _connection = null;

    public void CreateDatabase();
    
    public void EstablishConnection(string path) { }
    
    public void InsertNewSong(string title, string path);
    
    public string GetSongPath(int songId);

    public int GetSongID(string title);
    
    public void DeleteSong(int songId);

    public int InsertNewPlaylist(string name, int[] songIds);

    public List<int> GetPlaylist(int playlistId);

    public void DeletePlaylist(int playlistId);
}
