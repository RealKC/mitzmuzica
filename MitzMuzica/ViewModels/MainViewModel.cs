using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;
using MitzMuzica.DatabaseAPI;
using MitzMuzica.PluginAPI;
using MitzMuzica.PluginLoader;
namespace MitzMuzica.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    private Playlist _selectedPlaylist;
    private string _nowPlaying= "Now playing: nothing";
    private PluginLoader.PluginLoader _pluginLoader = new PluginLoader.PluginLoader(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/plugins");
    public string NowPlaying
    {
        get => _nowPlaying;
        set => _nowPlaying = value ?? throw new ArgumentNullException(nameof(value));
    }

    public MainViewModel()
    {
        
        _db.CreateDatabase(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/testDB.db");
        List<string> playlists = _db.GetPlaylistNames();
        foreach (var playlist in playlists)
        {
            List<Song> songs = [];
            List<int> songIDs = _db.GetPlaylist(playlist);
           
            foreach (var songID in songIDs)
            {
                 songs.Add(new Song(_db.GetSongTitle(songID), _db.GetSongPath(songID)));
            }
            _playlistsTest.Add(new Playlist(playlist, songs));
        }
        Playlists = new ObservableCollection<Playlist>(_playlistsTest);
        _pluginLoader.LoadPlugins();
        List<IAudioPlugin> test = _pluginLoader.AudioPlugins;
        
        Console.WriteLine(test);
    }

    private readonly Database _db = new Database();
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
        private List<Song> _songs = [];

        public List<Song> Songs => _songs;

        public Playlist(string title, List<Song> songs)
        {
            _title = title;
            _songs = songs;
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
