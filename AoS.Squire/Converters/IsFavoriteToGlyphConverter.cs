using System.Globalization;

namespace AoS.Squire.Converters;

public class IsFavoriteToGlyphConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isFavorite)
        {
            if (isFavorite)
            {
                return "\ue9d9";
            }
            else
            {
                return "\ue9d7";
            }
        }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}