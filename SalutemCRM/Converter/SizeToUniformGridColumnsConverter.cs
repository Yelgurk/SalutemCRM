using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Converter;


/// <summary>
/// This is as converter which will convert parent control (container) size into
/// Uniform Grid columns count (child control)
/// </summary>
public class SizeToUniformGridColumnsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        int block = 600;

        if ((double?)value is double size)
        {
            if (parameter is not null && (int)(size / block) > Int32.Parse((string)parameter))
                return Int32.Parse((string)parameter);
            else
                return (int)(size / block) == 0 ? 1 : (int)(size / block);
        }

        return 10;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return 1;
    }
}