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
        #region Constructor (resize window related to screen resolution

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDCOfficialWindow()
        {
            InitializeComponent();
            this.Height = SystemParameters.PrimaryScreenHeight * 0.75;
            this.Width = SystemParameters.PrimaryScreenWidth * 0.75;
        }

        #endregion

        #region Internal Variables

        private PollingUnitMenuItem _item = null;

        #endregion

        #region Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Public Methods

        public void Setup(PollingUnitMenuItem item)
        {
            _item = item;
            this.DataContext = _item;

            int thaiYear = 2566;
            int prevThaiYear = 2562;
            lstSummary.ItemsSource = null;
            if (null != _item)
            {
                lstSummary.ItemsSource = MPDCOfficialVoteSummary.Gets(
                    thaiYear, prevThaiYear, _item.ADM1Code, _item.PollingUnitNo, 6).Value();
            }
        }

        #endregion
    }
}
