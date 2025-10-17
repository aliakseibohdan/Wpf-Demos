using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ItemViewerApp.Utility
{
    /// <summary>
    /// Converts a boolean value to a <see cref="Visibility"/> value.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts a boolean value to a <see cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The type of the binding target property (expected <see cref="Visibility"/>).</param>
        /// <param name="parameter">A parameter string. If "Invert", the boolean is logically negated before conversion.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns><see cref="Visibility.Visible"/> if <paramref name="value"/> is <c>true</c>, otherwise <see cref="Visibility.Collapsed"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (parameter?.ToString() == "Invert")
                {
                    b = !b;
                }
                return b ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Not supported and throws a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts a string error message into a brush color for UI feedback.
    /// </summary>
    public class ErrorToBrushConverter : IValueConverter
    {
        /// <summary>
        /// Converts a string value (typically an error message) to a <see cref="Brush"/>.
        /// </summary>
        /// <param name="value">The string value to convert.</param>
        /// <param name="targetType">The type of the binding target property (expected <see cref="Brush"/>).</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns><see cref="Brushes.Red"/> if the string is not null or empty; otherwise, <see cref="Brushes.Gray"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrEmpty(value as string) ? Brushes.Gray : Brushes.Red;
        }

        /// <summary>
        /// Not supported and throws a <see cref="NotImplementedException"/>.
        /// </summary>
        /// <param name="value">The value produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}