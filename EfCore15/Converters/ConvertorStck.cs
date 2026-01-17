using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EFCORE15.Converters
{
    public class ConvertorStck : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if (value is int stock && stock < 10)
            {
                return (Brush)new BrushConverter().ConvertFromString("#FFD129");
            }

            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            throw new NotImplementedException();
        }
    }
}
