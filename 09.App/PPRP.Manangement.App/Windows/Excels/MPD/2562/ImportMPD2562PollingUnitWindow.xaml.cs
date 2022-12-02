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
    /// Interaction logic for ImportMPD2562PollingUnitWindow.xaml
    /// </summary>
    public partial class ImportMPD2562PollingUnitWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportMPD2562PollingUnitWindow()
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

        private bool Imports()
        {
            var items = lvMapPreview.Items;
            if (null == items || items.Count <= 0)
            {
                var mbox = PPRPApp.Windows.MessageBox;
                mbox.Owner = this;
                string msg = string.Empty;
                msg += "กรุณากดปุ่มอ่านข้อมูลตัวอย่าง" + Environment.NewLine;
                msg += "และทำการตรวจสอบข้อมูล" + Environment.NewLine;
                msg += "ก่อนทำการนำเข้าข้อมูล";
                mbox.Setup(msg, "PPRP");
                mbox.ShowDialog();
                return false; // No items
            }

            var errors = new List<ImportError>();

            var prog = PPRPApp.Windows.ProgressDialog;
            prog.Owner = this;
            prog.Setup(items.Count);
            prog.Show();

            int year = 2562;

            int iCnt = 2; // excel first row is column name.
            foreach (var item in items)
            {
                var obj = item as PollingUnit;
                if (null != obj)
                {
                    obj.ThaiYear = year;
                    var ret = PollingUnit.Import(obj);
                    if (ret.HasError)
                    {
                        // get debug string.
                        string dataString = obj.DebugString();
                        errors.Add(new ImportError()
                        {
                            RowNo = iCnt,
                            ErrMsg = ret.ErrMsg,
                            DataString = dataString
                        });
                    }
                }
                prog.Increment();

                iCnt++;
            }
            // Close progress dialog.
            prog.Close();

            if (null != errors && errors.Count > 0)
            {
                var errWin = PPRPApp.Windows.ImportReport;
                errWin.Owner = this;
                errWin.Setup(errors);
                errWin.ShowDialog();
            }

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
