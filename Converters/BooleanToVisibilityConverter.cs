// -----------------------------------------------------------------------
// <copyright file="BooleanToVisibilityConverter.cs" company="Nokia">
// Initial implementation by Steve Robbins from Nokia - http://twitter.com/sr_gb
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Globalization;
using System.Windows;
#if WINRT
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
#endif

namespace ShufflerFM.Converters
{
    /// <summary>
    /// Converts a boolean to visibility
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
#if WINRT
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The System.Type of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The culture</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
#else
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The System.Type of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            bool boolValue = (bool)value;
            if (parameter != null)
            {
                boolValue = !boolValue;
            }
            
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

#if WINRT
        /// <summary>
        /// Modifies the target data before passing it to the source object. This method
        /// is called only in System.Windows.Data.BindingMode.TwoWay bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The System.Type of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The culture</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
#else
        /// <summary>
        /// Modifies the target data before passing it to the source object. This method
        /// is called only in System.Windows.Data.BindingMode.TwoWay bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The System.Type of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture</param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
#endif
        {
            return null;
        }
    }
}
