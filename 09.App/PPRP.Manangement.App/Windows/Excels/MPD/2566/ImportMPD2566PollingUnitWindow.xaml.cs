#region Using

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
    /// Interaction logic for ImportMPD2566PollingUnitWindow.xaml
    /// </summary>
    public partial class ImportMPD2566PollingUnitWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportMPD2566PollingUnitWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private ExcelModel model = new ExcelModel();

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            model.SheetItemChanges += Model_SheetItemChanges;
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            model.SheetItemChanges -= Model_SheetItemChanges;
        }

        #endregion

        #region Button Handlers

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {
            Imports();
            DialogResult = true;
        }

        private void cmdChooseExcel_Click(object sender, RoutedEventArgs e)
        {
            ChooseExcelFile();
        }

        #endregion

        #region ExcelModel Handlers

        private void Model_SheetItemChanges(object sender, ExcelWorksheetArgs evt)
        {
            if (null != evt && null != evt.Sheet)
            {
                var sheet = evt.Sheet;
                lvMapPreview.Setup<PollingUnit>(sheet);
            }
        }

        #endregion

        #region Private Methods

        private void ChooseExcelFile()
        {
            if (model.Open())
            {
                wsMap.Setup<PollingUnit>(model);
            }

            txtFileName.Text = model.FileName;
        }

        private void Imports()
        {
            var items = lvMapPreview.Items;
            if (null == items || items.Count <= 0)
                return; // No items

            var prog = PPRPApp.Windows.ProgressDialog;
            prog.Owner = this;
            prog.Setup(items.Count);
            prog.Show();

            int year = 2566;
            foreach (var item in items)
            {
                var obj = item as PollingUnit;
                if (null != obj)
                {
                    obj.ThaiYear = year;
                    PollingUnit.Import(obj);
                }
                prog.Increment();
            }
            // Close progress dialog.
            prog.Close();
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
