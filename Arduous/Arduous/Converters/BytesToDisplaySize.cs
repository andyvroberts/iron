using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Windows.Data;

namespace Arduous.Converters
{
  [ValueConversion(typeof(double), typeof(string))]
  internal class BytesToDisplaySize: IValueConverter
  {
    // convert a double to its data size representation as a string.
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string returnValue = string.Empty;

      if (value is double)
      {
        double convertValue = (double)value;
        returnValue = CalculateSize(convertValue);
      }
      else
      {
        returnValue = value.ToString();
      }
      return returnValue;
    }

    private string CalculateSize(double size)
    {
      string getSize = "cannot calculate";

      if (size < 1024)
        getSize = string.Format("{0} bytes", size);
      else
      {
        if (size > 1024 * 1024 * 1024)
          getSize = string.Format("{0} Gb", Math.Round(size / (1024 * 1024 * 1024), 2));
        else
        {
          if (size > 1024 * 1024)
            getSize = string.Format("{0} Mb", Math.Round(size / (1024 * 1024), 2));
          else
          {
            if (size > 1024)
              getSize = string.Format("{0} Kb", Math.Round(size / 1024, 2));
          }
        }
      }
      return getSize;
    }

    // This converter is used for display only and so should never convert back.
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException("Bytes to Display Size Converter - ConvertBack");
    }
  }
}
