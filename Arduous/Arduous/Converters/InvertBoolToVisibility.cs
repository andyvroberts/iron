using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace Arduous.Converters
{
  [ValueConversion(typeof(bool), typeof(Visibility))]
  internal class InvertBoolToVisibility: IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      bool inType = (bool)value;
      if (inType)
        return Visibility.Collapsed;
      else
        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}
