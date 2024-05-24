using MitzMuzica.PlaylistAPI;
using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public interface IDatabase
{
    private static SQLiteConnection? _connection = null;

    public void EstablishConnection(string path) { }

    public void InsertNewSong(IAudioFile song);
    
    public (int, string, string) GetSong(int songId);
    
    public void DeleteSong(string songId);
    
    
    public void InsertNewPlaylist(IPlaylist playlist);
    
    public IPlaylist GetPlaylist(string playlistId);
    
    public void DeletePlaylist(string playlist);
}
