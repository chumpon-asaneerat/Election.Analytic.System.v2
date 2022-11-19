#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using NLib.Reflection;

#endregion

namespace PPRP.Controls.Utils
{
    // Note: This implements does not work because we cannot assigned the doublc click event handler
    // at design time.
    // now this file is set compile -> none
    #region MouseOptions

    /// <summary>
    /// Mouse Options.
    /// </summary>
    public class MouseOptions : DependencyObject
    {
        #region DoubleClick

        /// <summary>The DoubleClickProperty variable</summary>
        public static readonly DependencyProperty DoubleClickProperty = DependencyProperty.RegisterAttached("DoubleClick", typeof(InputBinding),
            typeof(MouseOptions), new PropertyMetadata(null, OnDoubleClickChanged));

        private static void OnDoubleClickChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = obj as FrameworkElement;
            if (null == element)
            {
                // Potentially throw an exception if an object is not a FrameworkElement (is null).
                return;
            }
            if (null != e && null != e.NewValue)
            {
                element.InputBindings.Add(e.NewValue as InputBinding);
            }
            if (null != e && null != e.OldValue)
            {
                element.InputBindings.Remove(e.OldValue as InputBinding);
            }
        }


        /// Gets DoubleClick Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <returns>Returns current proeprty value.</returns>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static InputBinding GetDoubleClick(FrameworkElement element)
        {
            return (InputBinding)element.GetValue(DoubleClickProperty);
        }
        /// Sets DoubleClick Value.
        /// </summary>
        /// <param name="obj">The target object.</param>
        /// <param name="value">The new value.</param>
        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static void SetDoubleClick(FrameworkElement element, InputBinding value)
        {
            element.SetValue(DoubleClickProperty, value);
        }

        #endregion
    }

    #endregion
}
