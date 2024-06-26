﻿/**************************************************************************
 *                                                                        *
 *  Description: Main window logic                                        *
 *  Website:     https://github.com/RealKC/mitzmuzica                     *
 *  Copyright:   (c) 2024, Mihalache Mihai                                *
 *  SPDX-License-Identifier: AGPL-3.0-only                                *
 *                                                                        *
 **************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using MitzMuzica.ViewModels;

namespace MitzMuzica.Views;

public partial class MainView : UserControl
{
    private readonly DispatcherTimer timer;
    private readonly int x = 0;

    public MainView()
    {
        InitializeComponent();
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;
        timer.Start();
        PlaylistBox.SelectionChanged += PlaylistBox_SelectionChanged;
    }

    public static FilePickerFileType AudioAll { get; } = new("All audio file")
    {
        Patterns = new[] { "*.mp3", "*.aac", "*.wav", "*.flac" }, MimeTypes = new[] { "audio/*" }
    };

    private void Timer_Tick(object sender, EventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            var seconds = x % 60 < 10 ? "0" + (x % 60) : (x % 60).ToString();
            TimePlayed.Content = (x / 60) + ":" + seconds;
            if (ProgressSlider.Value < 100)
            {
                if (viewModel.IsPlaying)
                {
                    ProgressSlider.Value++;
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
                if (viewModel.IsPlaying)
                {
                    viewModel.PlayingFile.Stop();
                }
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
        var selectedSong = (sender as Button).DataContext as MainViewModel.Song;
        if (DataContext is MainViewModel viewModel)
        {
            var file = viewModel.AudioPlugin.Open(selectedSong.Path);

            viewModel.PlayingFile = file;
            viewModel.PlayingFile.Start();
            viewModel.NowPlaying =
                "Now playing: " + viewModel.PlayingFile.Title + " by " + viewModel.PlayingFile.Author;
            viewModel.IsPlaying = true;
            var t = TimeSpan.FromMilliseconds(viewModel.PlayingFile.Length);
            var secs = t.Seconds < 10 ? "0" + t.Seconds : t.Seconds.ToString();
            SongLength.Content = t.Minutes + ":" + secs;
            ProgressSlider.Maximum = t.TotalSeconds;
            ProgressSlider.Value = 0;
        }
    }

    private void AddPlaylist(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            viewModel.Playlists.Add(new MainViewModel.Playlist(PlaylistName.Text!, []));
        }
    }

    private async void AddSong(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainViewModel viewModel)
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var files = await topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
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
                songIds.Add(MainViewModel.DB.InsertNewSong(WebUtility.UrlDecode(song.Title),
                    WebUtility.UrlDecode(file.Path.AbsolutePath)));
                songs.Add(new MainViewModel.Song(WebUtility.UrlDecode(song.Title),
                    WebUtility.UrlDecode(file.Path.AbsolutePath)));
            }

            MainViewModel.DB.InsertNewPlaylist(PlaylistName.Text!, songIds);
            var last = viewModel.Playlists.Last();
            viewModel.Playlists.Remove(last);
            viewModel.Playlists.Add(new MainViewModel.Playlist(PlaylistName.Text!, songs));
        }
    }

    private void ProgressChanged(object? sender, RangeBaseValueChangedEventArgs rangeBaseValueChangedEventArgs)
    {
        if (DataContext is MainViewModel viewModel)
        {
            var y = (int)ProgressSlider.Value;
            var seconds = y % 60 < 10 ? "0" + (y % 60) : (y % 60).ToString();
            TimePlayed.Content = (y / 60) + ":" + seconds;

            //viewModel.PlayingFile?.SeekTo((long)ProgressSlider.Value);
        }
    }

    private void Help_OnClick(object? sender, RoutedEventArgs e)
    {
        var helpWindow = new HelpWindow();
        helpWindow.Show();
    }
}
