using Avalonia.Data.Converters;
using Avalonia.Media;
using SalutemCRM.Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Converter;

public class RowStatusConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
            return Brushes.White;

        return value.EnumCast<Stock_Status>() switch {
            Stock_Status.Enough         => Brushes.GreenYellow,
            Stock_Status.CloseToLimit   => Brushes.Yellow,
            Stock_Status.NotEnough      => Brushes.Orange,
            Stock_Status.ZeroWarning    => Brushes.OrangeRed,
            _                           => Brushes.White    
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}