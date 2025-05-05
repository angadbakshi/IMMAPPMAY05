using Syncfusion.Maui.Chat;
using System.Globalization;

namespace CanadianVisaChatbot.Mobile.Converters;

public class MessageBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is MessagePosition position)
        {
            return position switch
            {
                MessagePosition.Right => Colors.LightBlue,
                MessagePosition.Left => Colors.White,
                _ => Colors.White
            };
        }
        return Colors.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}