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
public class NumberInversionConverterxxx : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return -(double)value! as object;
        /*
        return value?
            .Do(v => {
                return new SmartType(targetType) switch
                {
                    SmartType<int> x when x.GetType() == targetType => -(int)value!,
                    SmartType<double> x when x.GetType() == targetType => -(double)value!,
                    SmartType<float> x when x.GetType() == targetType => -(float)value!,
                    _ => value
                } as object;
            });
        */
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}