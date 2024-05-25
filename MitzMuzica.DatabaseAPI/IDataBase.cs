using MitzMuzica.PlaylistAPI;
using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public interface IDatabase
{
    private static SQLiteConnection? _connection = null;

    public void EstablishConnection(string path) { }

    public void InsertNewSong(string title, string path);
    
    public (int, string, string) GetSong(int songId);

    public int GetSongID(string title);
    
    public void DeleteSong(int songId);
    
    public void InsertNewPlaylist(IPlaylist playlist);
    
    public IPlaylist GetPlaylist(string playlistId);
    
    public void DeletePlaylist(string playlist);
}
