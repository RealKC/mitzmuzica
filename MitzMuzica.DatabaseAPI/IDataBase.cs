using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public interface IDatabase
{
    private static SQLiteConnection? _connection = null;
    /// <summary>
    /// Creates the database if it doesn't exist already, does nothing otherwise
    /// </summary>
    public void CreateDatabase();
    /// <summary>
    /// Establishes a connection to the database
    /// </summary>
    /// <param name="path"></param>
    public void EstablishConnection(string path) { }
    
    /// <summary>
    /// Inserts a new song in the database and returns a songId,
    /// duplicate or null title/path will throw an sql exception
    /// </summary>
    /// <param name="title"></param>
    /// <param name="path"></param>
    public int InsertNewSong(string title, string path);
    
    public string GetSongPath(int songId);
    public string GetSongPath(string title);

    public int GetSongID(string title);
    
    public void DeleteSong(int songId);
    public void DeleteSong(string title);

    public int InsertNewPlaylist(string name, List<int> songIds);

    public List<int> GetPlaylist(int playlistId);
    public List<int> GetPlaylist(string name);

    public void DeletePlaylist(string name);
}
