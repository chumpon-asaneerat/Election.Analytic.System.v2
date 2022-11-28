#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Reflection;
using NLib.Services;
using PPRP.Models;
using PPRP.Models.Excel;

#endregion

namespace PPRP.Controls.Excels
{
    /// <summary>
    /// Interaction logic for ExcelColumnMapControl.xaml
    /// </summary>
    public partial class ExcelColumnMapControl : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelColumnMapControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Combobox Handlers

        private void cbSheets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*
            var item = cbSheets.SelectedItem as NExcelWorksheet;
            LoadSheetColumns(item);
            */
        }

        #endregion

        #region Button Handlers

        private void cmdResetMapProperty_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ctx = (null != button) ? button.DataContext : null;
            /*
            var map = (null != ctx) ? ctx as NExcelMapProperty : null;
            if (null != map)
            {
                map.SelectedColumn = null; // reset
            }
            */
        }
        private void cmdLoadExcelData_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (null == _import)
                return;
            _import.RaiseSampleDataChanged();
            */
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion

        #region Public Properties

        #endregion
    }
}
