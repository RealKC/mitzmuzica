using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using MitzMuzica.PlaylistAPI;
using MitzMuzica.PluginAPI;
using MitzMuzica.ViewModels;

namespace MitzMuzica.Views;

public partial class MainView : UserControl
{
    DispatcherTimer timer;
    private int x = 0;
    public MainView()
    {
        InitializeComponent();
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
        PlaylistBox.SelectionChanged += PlaylistBox_SelectionChanged;
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            TimePlayed.Content = x / 60 + ":" + x % 60;
            if (ProgressSlider.Value < 100)
            {
                if (viewModel.IsPlaying)
                {
                    ProgressSlider.Value++;
                    x++;
                }
            }
        }

    }
    private void PlaylistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            PlaylistContext.IsVisible = true;
            if (e.AddedItems.Count > 0)
            {
                viewModel.SelectedPlaylist = e.AddedItems[0] as MainViewModel.Playlist;
            }
            else
            {
                viewModel.SelectedPlaylist = null;
            }
        }
    }

    private void TogglePlayState(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
          
            if (viewModel.PlayingFile != null)
            {
                if(viewModel.IsPlaying)
                    viewModel.PlayingFile.Stop();
                else
                {
                    viewModel.PlayingFile.Start();
                }
            }
            viewModel.IsPlaying = !viewModel.IsPlaying;
        }
    }

    private void PlaySong(object? sender, RoutedEventArgs e)
    {
        MainViewModel.Song selectedSong = (sender as Button).DataContext as MainViewModel.Song;
        if (DataContext is MainViewModel viewModel)
        {
            IAudioFile file = viewModel.AudioPlugin.Open(selectedSong.Path);

            viewModel.PlayingFile = file;
            viewModel.PlayingFile.Start();
            viewModel.NowPlaying = 
                "Now playing: " + viewModel.PlayingFile.Title + " by " + viewModel.PlayingFile.Author;
            viewModel.IsPlaying = true;
        }
    }

    private void AddPlaylist(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.Playlists.Add(new MainViewModel.Playlist(PlaylistName.Text!, []));
        }
    }
    public static FilePickerFileType AudioAll { get; } = new("All audio file")
    {
        Patterns = new[] { "*.mp3", "*.aac", "*.wav", "*.flac"},
        MimeTypes = new[] { "audio/*" }
    };
    private async void AddSong(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Choose one or more songs to add to the playlist",
                AllowMultiple = true,
                //You can add either custom or from the built-in file types. See "Defining custom file types" on how to create a custom one.
                FileTypeFilter = new[] { AudioAll }
            });
            List<int> songIds = [];
            List<MainViewModel.Song> songs = [];
            foreach (var file in files)
            {
                var song = viewModel.AudioPlugin.Open(file.Path.AbsolutePath);
                songIds.Add(MainViewModel.DB.InsertNewSong(WebUtility.UrlDecode(song.Title), WebUtility.UrlDecode(file.Path.AbsolutePath)));
                songs.Add(new MainViewModel.Song(WebUtility.UrlDecode(song.Title), WebUtility.UrlDecode(file.Path.AbsolutePath)));
            }
            MainViewModel.DB.InsertNewPlaylist(PlaylistName.Text!, songIds);
            var last = viewModel.Playlists.Last();
            viewModel.Playlists.Remove(last);
            viewModel.Playlists.Add(new MainViewModel.Playlist(PlaylistName.Text!, songs));
        
        }
    }
}


