#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using NLib;
using NLib.Reflection;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Windows
{
    /// <summary>
    /// Interaction logic for ImportADMPakWindow.xaml
    /// </summary>
    public partial class ImportADMPakWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportADMPakWindow()
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
            model.SheetItemChanges += Model_SheetItemChanges;
        }
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            model.SheetItemChanges -= Model_SheetItemChanges;
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

        #region ExcelModel Handlers

        private void Model_SheetItemChanges(object sender, ExcelWorksheetArgs evt)
        {
            if (null != evt && null != evt.Sheet)
            {
                var sheet = evt.Sheet;
                lvMapPreview.Setup<MADMPak>(sheet);
            }
        }

        #endregion

        #region Private Methods

        private void ChooseExcelFile()
        {
            if (model.Open())
            {
                wsMap.Setup<MADMPak>(model);
            }

            txtFileName.Text = model.FileName;
        }

        private void EanbleButtons(bool enable)
        {
            cmdCancel.IsEnabled = enable;
            cmdFinish.IsEnabled = enable;
            cmdChooseExcel.IsEnabled = enable;
        }

        private bool Imports()
        {
            var items = lvMapPreview.Items;
            if (null == items || items.Count <= 0)
            {
                var mbox = PPRPApp.Windows.MessageBox;
                mbox.Owner = this;
                string msg = "กรุณาทำการ กดปุ่มอ่านข้อมูล และทำการตรวจสอบข้อมูล" + Environment.NewLine + "ก่อนทำการ กดปุ่มนำเข้าข้อมูล";
                mbox.Setup(msg, "PPRP");
                mbox.ShowDialog();
                return false; // No items
            }

            onImporting = true;
            EanbleButtons(false); // while import disable all buttons.

            var errors = new List<ImportError>();

            var prog = PPRPApp.Windows.ProgressDialog;
            prog.Owner = this;
            prog.Setup(items.Count);
            prog.Show();

            int iCnt = 2; // excel first row is column name.
            foreach (var item in items)
            {
                var ret = MADMPak.Import(item as MADMPak);
                if (ret.HasError)
                {
                    // get debug string.
                    string dataString = item.DebugString();
                    errors.Add(new ImportError()
                    {
                        RowNo = iCnt,
                        ErrMsg = ret.ErrMsg,
                        DataString = dataString
                    });
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
