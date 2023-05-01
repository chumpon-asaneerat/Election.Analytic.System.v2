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
    /// Interaction logic for MPDCOfficialWindow.xaml
    /// </summary>
    public partial class MPDCOfficialWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDCOfficialWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private PollingUnitMenuItem _item = null;

        #endregion

        #region Public Methods

        public void Setup(PollingUnitMenuItem item)
        {
            _item = item;
            this.DataContext = _item;
        }

        #endregion
    }
}
