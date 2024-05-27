namespace MitzMuzica.PlaylistAPI;

/// <summary>
/// Interface used for playlist interaction.
/// </summary>
public interface IPlaylist
{
    /// <summary>
    /// Gets the name of the playlist.
    /// </summary>
    /// <returns>The name of the playlist.</returns>
    public string PlaylistName { get; }

    /// <summary>
    /// Adds songs to the playlist.
    /// Throws an sql exception if any invalid song Ids are provided.
    /// </summary>
    /// <param name="songIds">The list of song Ids to add.</param>
    public void AddSongs(List<int> songIds);

    /// <summary>
    /// Retrieves the list of songs in the playlist.
    /// </summary>
    /// <returns>The list of song Ids in the playlist.</returns>
    public List<int> GetSongs();

    /// <summary>
    /// Deletes the playlist.
    /// Throws an sql exception if the playlist name doesn't exist.
    /// </summary>
    public void DeletePlaylist();
}
