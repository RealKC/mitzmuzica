using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
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
            viewModel.IsPlaying = !viewModel.IsPlaying;
        }
    }
}


