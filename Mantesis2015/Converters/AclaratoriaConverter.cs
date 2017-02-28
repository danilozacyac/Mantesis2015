using System;
using System.Linq;
using System.Windows.Data;
using Mantesis2015.Singleton;
using Mantesis2015.Dto;

namespace Mantesis2015.Converters
{
    public class AclaratoriaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int nota = 0;
            int.TryParse(value.ToString(), out nota);

            if (nota == 0)
                return String.Empty;
            else
            {
                Aclaratoria notaReturn = AclaratoriaSingleton.Aclaratorias.SingleOrDefault(x => x.IdNota == nota);
                return notaReturn.Nota;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}