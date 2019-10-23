using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PrimeRuleEngine.View.Converters
{
    /// <summary>
    /// The <see cref="BoolToVisibilityConverter"/> converters a <see cref="Boolean"/> value to <see cref="Visibility"/>.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {

        /// <summary>
        /// This method converts a boolean 'true' value to <see cref="Visibility.Visibility"/> 
        /// And converts all other values to <see cref="Visibility.Collapsed"/> 
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
