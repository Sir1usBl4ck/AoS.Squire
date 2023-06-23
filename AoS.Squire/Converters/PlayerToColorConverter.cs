using System.Globalization;

namespace AoS.Squire.Converters;

public class PlayerToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string playerType)
        {
            return playerType switch
            {
                "player" => Color.FromRgb(142, 200, 229),
                "opponent" => Color.FromRgb(237, 128, 128),
                _ => null
            };
        }

        return null;

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}