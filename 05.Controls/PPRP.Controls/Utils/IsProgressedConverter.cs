#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows;

#endregion

namespace PPRP.Controls.Utils
{
    #region IsProgressedConverter

    /// <summary>
    /// IsProgressedConverter class.
    /// </summary>
    public class IsProgressedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((values[0] is ContentPresenter &&
                values[1] is int) == false)
            {
                return Visibility.Collapsed;
            }
            bool checkNextItem = System.Convert.ToBoolean(parameter.ToString());
            ContentPresenter contentPresenter = values[0] as ContentPresenter;
            int progress = (int)values[1];
            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(contentPresenter);
            if (null != itemsControl)
            {
                int index = itemsControl.ItemContainerGenerator.IndexFromContainer(contentPresenter);
                if (checkNextItem)
                {
                    index++;
                }
                WizardProgressBar wiz = itemsControl.TemplatedParent as WizardProgressBar;
                if (index <= progress)
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    #endregion
}
