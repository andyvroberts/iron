using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.Windows;

namespace Obdurate.converters
{
  [ValueConversion(typeof(bool), typeof(Visibility))]
  internal class InvertBooleanToVisibility: IValueConverter
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