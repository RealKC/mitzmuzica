using MitzMuzica.PlaylistAPI;
using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

public interface IDataBase
{
    public SQLiteConnection Connection { get; }

    public void EstablishConnection(); 
    
    public IAudioFile GetSong(string songId);

    public void InsertNewSong(IAudioFile song);
    
    public void DeleteSong(string songId);
    

    public IPlaylist GetPlaylist(string playlistId);
    
    public void InsertNewPlaylist(IPlaylist playlist);

    public void DeletePlaylist(string playlist);
}
