namespace TriviaGoldMine.Client.Converters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows.Data;

    public class PathToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value?.ToString();
            return Path.GetFileNameWithoutExtension(path);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
