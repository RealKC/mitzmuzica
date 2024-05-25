namespace MitzMuzica.PlaylistAPI;

public interface IPlaylist
{
    public string PlaylistName { get; }

    public void CreatePlaylist(int[] songIds);
}
