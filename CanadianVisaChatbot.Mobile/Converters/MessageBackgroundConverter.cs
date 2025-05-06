using System.Globalization;

namespace CanadianVisaChatbot.Mobile.Converters;

public class MessageBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isFromUser)
        {
            return isFromUser ? Colors.LightBlue : Colors.White;
        }
        return Colors.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class MessageAlignmentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isFromUser)
        {
            return isFromUser ? LayoutOptions.End : LayoutOptions.Start;
        }
        return LayoutOptions.Start;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}