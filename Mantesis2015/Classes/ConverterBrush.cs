using System;
using System.Linq;
using System.Windows.Data;

namespace Mantesis2015.Classes
{
    class ConverterBrush : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                int number = 0;
                int.TryParse(value.ToString(), out number);
                if (number == 0)
                    return "Yellow";

                if (number > 0)
                    return "Green";
            }

            return "White";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
