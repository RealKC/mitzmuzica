namespace MitzMuzica.PlaylistAPI;

public interface IPlaylist
{
    public string PlaylistName { get; }

    public void AddSongs(List<int> songIds);
    public List<int> GetSongs();
    public void DeletePlaylist();
}
