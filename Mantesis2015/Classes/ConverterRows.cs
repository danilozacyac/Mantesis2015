using System;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace Mantesis2015.Classes
{
    public class ConverterRows : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int number = 0;
                int.TryParse(value.ToString(), out number);
                if (number == 0)
                    return new SolidColorBrush(Colors.White);

                if (number > 0)
                    return new SolidColorBrush(Colors.LightBlue);
            }

            return new SolidColorBrush(Colors.White);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
