using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using MitzMuzica.DatabaseAPI;
using MitzMuzica.PluginAPI;

namespace MitzMuzica.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    private Playlist _selectedPlaylist;
    [ObservableProperty] 
    private string _nowPlaying= "Now playing: nothing";
    
    private PluginLoader.PluginLoader _pluginLoader = new PluginLoader.PluginLoader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/plugins");
    public IAudioPlugin AudioPlugin;
    public IAudioFile? PlayingFile = null;
    
    public MainViewModel()
    {
        DB.CreateDatabase(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/testDB.db");
        List<string> playlists = DB.GetPlaylistNames();
        foreach (var playlist in playlists)
        {
            List<Song> songs = [];
            List<int> songIDs = DB.GetPlaylist(playlist);
           
            foreach (var songID in songIDs)
            {
                 songs.Add(new Song(DB.GetSongTitle(songID), DB.GetSongPath(songID)));
            }
            _playlistsTest.Add(new Playlist(playlist, songs));
        }
        Playlists = new ObservableCollection<Playlist>(_playlistsTest);
        _pluginLoader.LoadPlugins();
        AudioPlugin = _pluginLoader.AudioPlugins[0];
    }

    public static readonly Database DB = new Database();
    private readonly List<Playlist> _playlistsTest = [];

    public class Song(string title, string path) //TODO replace this with AudioFile class
    {
        public string Title
        {
            get => title;
            set => title = value;
        }

        public string Path
        {
            get => path;
            set => path = value;
        }

    }

    public class Playlist
    {
        private string _title;
        private PlaylistAPI.Playlist _api;
        
        public PlaylistAPI.Playlist API
        {
            get => _api;
            set => _api = value;
        }

        private List<Song> _songs = [];

        public List<Song> Songs => _songs;
        
        

        public Playlist(string title, List<Song> songs)
        {
            _title = title;
            _songs = songs;
            _api = new PlaylistAPI.Playlist(title, DB);
        }

        public string Title
        {
            get => _title;
            set => _title = value;
        }
        
        public void Add(Song song)
        {
            _songs.Add(song);
        }

        public void Remove(Song song)
        {
            _songs.Remove(song);
        }
    }
    public Playlist SelectedPlaylist
    {
        get { return _selectedPlaylist; }
        set
        {
            if (_selectedPlaylist == value)
            {
                return;
            }

            _selectedPlaylist = value;
            OnPropertyChanged(nameof(SelectedPlaylist));
        }
    }

    [ObservableProperty] 
    private bool _isPlaying;
    
    private ObservableCollection<Playlist> _playlists;
    public new event PropertyChangedEventHandler PropertyChanged;
    public ObservableCollection<Playlist> Playlists
    {
        get { return _playlists; }
        set
        {
            _playlists = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Playlists)));
        }
    }
}
