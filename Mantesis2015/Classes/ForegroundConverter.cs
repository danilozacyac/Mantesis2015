using System;
using System.Linq;
using System.Windows.Data;

namespace Mantesis2015.Classes
{
    class ForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                bool isDelete = (bool)value;
                if (isDelete)
                    return "Red";

                if (!isDelete)
                    return "Black";
            }

            return "Black";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
