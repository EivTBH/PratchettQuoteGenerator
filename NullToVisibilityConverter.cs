using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace PratchettQuoteGenerator
{
    /// <summary>
    /// Converts null to visibility collapsed.
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {

        /// <summary>
        /// Checks if the value is null and returns collapsed if it is.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}