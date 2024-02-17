using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
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
        /*
        if ((double?)value is double size)
            return size switch
            {
                (>= 0) and (< 700) => 1,
                (>= 700) and (< 1400) => 2,
                _ => 3
            };
        else
            return 1;
        */
        if ((double?)value is double size)
            return (int)size / 600 + 1;
        else
            return 1;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return 1;
    }
}