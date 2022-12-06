﻿#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ImportMPDC2566Window.xaml
    /// </summary>
    public partial class ImportMPDC2566Window : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportMPDC2566Window()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private ExcelModel model = new ExcelModel();
        private bool onImporting = false;

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //import.OnSampleDataChanged += Import_OnSampleDataChanged;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //import.OnSampleDataChanged -= Import_OnSampleDataChanged;
        }

        #endregion

        #region Window Closing

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = onImporting;
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {
            if (Imports())
            {
                DialogResult = true;
            }
        }

        private void cmdChooseExcel_Click(object sender, RoutedEventArgs e)
        {
            ChooseExcelFile();
        }

        #endregion

        #region NExcelImport Handlers

        private void Import_OnSampleDataChanged(object sender, EventArgs e)
        {
            /*
            if (null != wsMap && null != wsMap.ImportModel)
            {
                var model = wsMap.ImportModel;
                lvMapPreview.Setup(import);

                items = XlsMProvince.LoadWorksheetTable(import, model.Worksheet.SheetName, model.Maps);
                if (null != items)
                {

                }
                lvMapPreview.UpdateItems(model.Maps, items);
            }
            */
        }

        #endregion

        #region Private Methods

        private void ChooseExcelFile()
        {

        }

        private void EanbleButtons(bool enable)
        {
            cmdCancel.IsEnabled = enable;
            cmdFinish.IsEnabled = enable;
            cmdChooseExcel.IsEnabled = enable;
        }

        private bool Imports()
        {

            onImporting = true;
            EanbleButtons(false); // while import disable all buttons.


            EanbleButtons(true); // completed import enable all buttons.
            onImporting = false;

            return true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {

        }

        #endregion
    }
}
