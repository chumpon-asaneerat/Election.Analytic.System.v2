#region Using

using System;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace PPRP.Controls
{
    public class WizardProgressBar : ItemsControl
    {
        #region Dependency Properties

        public static DependencyProperty ProgressProperty =
            DependencyProperty.Register("Progress",
                                        typeof(int),
                                        typeof(WizardProgressBar),
                                        new FrameworkPropertyMetadata(0, null, CoerceProgress));

        private static object CoerceProgress(DependencyObject target, object value)
        {
            WizardProgressBar wiz = (WizardProgressBar)target;
            int progress = (int)value;
            if (progress < 0)
            {
                progress = 0;
            }
            else if (progress > 100)
            {
                progress = 100;
            }
            return progress;
        }

        #endregion

        #region Static Constructor

        /// <summary>
        /// Static Constructor.
        /// </summary>
        static WizardProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WizardProgressBar), new FrameworkPropertyMetadata(typeof(WizardProgressBar)));
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WizardProgressBar()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Progress step (Between 0-100).
        /// </summary>
        public int Progress
        {
            get { return (int)base.GetValue(ProgressProperty); }
            set { base.SetValue(ProgressProperty, value); }
        }

        #endregion // Properties
    }
}
