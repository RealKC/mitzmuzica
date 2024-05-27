using System.Data.SQLite;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.DatabaseAPI;

/// <summary>
/// Interface for interacting with the database to manage songs and playlists.
/// </summary>
public interface IDatabase
{
    private static SQLiteConnection? _connection = null;
    
    /// <summary>
    /// Creates the database if it doesn't exist already
    /// and establishes a connection to it, only establishes a connection otherwise.
    /// </summary>
    /// <param name="databasePath">Path to the database</param>
    public void CreateDatabase(string databasePath);
    
    /// <summary>
    /// Establishes new a connection to the database if it doesn't exist.
    /// </summary>
    /// <param name="path"></param>
    private void EstablishConnection(string path) { }
    
    /// <summary>
    /// Inserts a new song in the database and retrieves its identifier.
    /// Throws an sql exception if title/path is a duplicate or null.
    /// </summary>
    /// <param name="title">The title of the song.</param>
    /// <param name="path">The path of the song.</param>
    /// <returns>The identifier of the inserted song.</returns>
    public int InsertNewSong(string title, string path);
    
    /// <summary>
    /// Retrieves the file path of a song based on its identifier.
    /// Throws an sql exception if the provided identifier doesn't exist.
    /// </summary>
    /// <param name="songId">The identifier of the song.</param>
    /// <returns>The file path of the song.</returns>
    public string GetSongPath(int songId);
    
    /// <summary>
    /// Retrieves the file path of a song based on its title.
    /// Throws an sql exception if the provided title doesn't exist.
    /// </summary>
    /// <param name="title">The title of the song.</param>
    /// <returns>The file path of the song.</returns>
    public string GetSongPath(string title);
    
    /// <summary>
    /// Retrieves the identifier of a song based on its title.
    /// Throws an sql exception if the provided title doesn't exist.
    /// </summary>
    /// <param name="title">The title of the song.</param>
    /// <returns>The identifier of the song.</returns>
    public int GetSongId(string title);
    
    /// <summary>
    /// Deletes a song from the database based on its identifier.
    /// Throws an sql exception if the provided identifier doesn't exist.
    /// </summary>
    /// <param name="songId">The identifier of the song to be deleted.</param>
    public void DeleteSong(int songId);

    /// <summary>
    /// Deletes a song from the database based on its title.
    /// Throws an sql exception if the provided title doesn't exist.
    /// </summary>
    /// <param name="title">The title of the song to be deleted.</param>
    public void DeleteSong(string title);
    
    /// <summary>
    /// Inserts a new playlist into the database and retrieves its identifier.
    /// Throws an sql exception if the provided name is a duplicate.
    /// </summary>
    /// <param name="name">The name of the new playlist.</param>
    /// <param name="songIds">A list of song identifiers to be included in the new playlist.</param>
    /// <returns>The identifier of the newly inserted playlist.</returns>
    public int InsertNewPlaylist(string name, List<int> songIds);
    
    /// <summary>
    /// Retrieves the list of song identifiers in a playlist based on its identifier.
    /// Throws an sql exception if the provided identifier doesn't exist.
    /// </summary>
    /// <param name="playlistId">The identifier of the playlist.</param>
    /// <returns>A list of song identifiers in the specified playlist.</returns>
    public List<int> GetPlaylist(int playlistId);
    
    /// <summary>
    /// Retrieves the list of song identifiers in a playlist based on its name.
    /// Throws an sql exception if the provided name doesn't exist.
    /// </summary>
    /// <param name="name">The name of the playlist.</param>
    /// <returns>A list of song identifiers in the specified playlist.</returns>
    public List<int> GetPlaylist(string name);
    
    /// <summary>
    /// Deletes a playlist from the database based on its name.
    /// /// Throws an sql exception if the provided name doesn't exist.
    /// </summary>
    /// <param name="name">The name of the playlist to be deleted.</param>
    public void DeletePlaylist(string name);
}
