namespace TriviaGoldMine.Client.Converters
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Data;

    public class PathArrayToFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var arr = ((IEnumerable)value).Cast<string>().Select(Path.GetFileNameWithoutExtension).ToArray();
            return arr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
