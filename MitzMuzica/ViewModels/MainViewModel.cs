using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using Avalonia.Data.Converters;
using CommunityToolkit.Mvvm.ComponentModel;
using Material.Icons;

namespace MitzMuzica.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    private Playlist _selectedPlaylist;

    public MainViewModel()
    {
        Playlists = new ObservableCollection<Playlist>(PlaylistsTest);
    }
    public List<Playlist> PlaylistsTest = new List<Playlist>
    {
        new Playlist("Manele", new List<Song>
        {
            new Song("Eee Aaa", "Tzanca"),
            new Song("Nu imi plac politisti", "Salam"),
            // Add more songs here...
            new Song("Song Title 1", "Artist 1"),
            new Song("Song Title 2", "Artist 2"),
            new Song("Song Title 3", "Artist 3"),
        }),
        new Playlist("Pop", new List<Song>
        {
            // Add songs here...
            new Song("Pop Song Title 1", "Pop Artist 1"),
            new Song("Pop Song Title 2", "Pop Artist 2"),
            new Song("Pop Song Title 3", "Pop Artist 3"),
        }),
        new Playlist("Rock", new List<Song>
        {
            // Add songs here...
            new Song("Rock Song Title 1", "Rock Artist 1"),
            new Song("Rock Song Title 2", "Rock Artist 2"),
            new Song("Rock Song Title 3", "Rock Artist 3"),
        }),
    };

    public class Song(string title, string author) //TODO replace this with AudioFile class
    {
        public string Title
        {
            get => title;
            set => title = value;
        }

        public string Author
        {
            get => author;
            set => author = value;
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
