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

        private ExcelModel _model = null;
        private Type _targetType = null;
        private NExcelWorksheet _sheet = null;

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
            var item = cbSheets.SelectedItem as NExcelWorksheet;
            LoadSheetColumns(item);
        }

        #endregion

        #region Button Handlers

        private void cmdResetMapProperty_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var ctx = (null != button) ? button.DataContext : null;
            var map = (null != ctx) ? ctx as NExcelMapProperty : null;
            if (null != map)
            {
                map.SelectedColumn = null; // reset
            }
        }
        private void cmdLoadExcelData_Click(object sender, RoutedEventArgs e)
        {
            if (null == _model || null == _sheet)
                return;
            _sheet.RefrehsWorksheet(); // Raise event.
        }

        #endregion

        #region Private Methods

        private void LoadWoksheets()
        {
            cbSheets.ItemsSource = null;
            if (null != _model)
            {
                cbSheets.ItemsSource = _model.Worksheets;
                if (null != _model.Worksheets && _model.Worksheets.Count > 0)
                {
                    // auto select first index.
                    cbSheets.SelectedIndex = 0;
                }
            }
        }
        private void LoadSheetColumns(NExcelWorksheet sheet)
        {
            lvMaps.ItemsSource = null;

            ResetMaps(sheet);
        }
        private void ResetMaps(NExcelWorksheet sheet)
        {
            _sheet = sheet;
            if (null == _sheet) return;
            if (null == _targetType) return;

            sheet.MapColumns(_targetType);
            // set map items source and data context for combobox columns.
            lvMaps.ItemsSource = sheet.Mappings;
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="model">The Excel model.</param>
        /// <param name="targetType">The target type.</param>
        public void Setup(ExcelModel model, Type targetType)
        {
            // setup model and target type.
            _model = model;
            _targetType = targetType;
            LoadWoksheets();
        }
        /// <summary>
        /// Setup.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="model">The Excel model.</param>
        public void Setup<T>(ExcelModel model)
            where T: class
        {
            Setup(model, typeof(T));
        }

        #endregion

        #region Public Properties

        #endregion
    }
}
