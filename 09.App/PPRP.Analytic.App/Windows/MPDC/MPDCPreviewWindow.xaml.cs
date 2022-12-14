#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Reflection;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for MPDCPreviewWindow.xaml
    /// </summary>
    public partial class MPDCPreviewWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDCPreviewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private MPDCSummary _person = null;

        #endregion

        #region Public Methods

        public void Setup(MPDCSummary person)
        {
            _person = person;
            this.DataContext = _person;
            if (null == _person)
            {
                
            }
        }

        #endregion
    }
}
