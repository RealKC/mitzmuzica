using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Material.Icons;

namespace MitzMuzica.ViewModels;

public class PlayPauseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isPlaying = (bool)value;
        return isPlaying ? MaterialIconKind.Pause : MaterialIconKind.Play;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(bool)value;
    }
}
