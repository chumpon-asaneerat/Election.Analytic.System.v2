#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MPD2562StatVoterManagePage.xaml
    /// </summary>
    public partial class MPD2562StatVoterManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562StatVoterManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

        private void cmdAddNew_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdImport_Click(object sender, RoutedEventArgs e)
        {
            Import();
        }

        private void cmdExport_Click(object sender, RoutedEventArgs e)
        {
            Export();
        }

        private void cmdHome_Click(object sender, RoutedEventArgs e)
        {
            GotoMainMenuPage();
        }

        private void cmdRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            Print();
        }

        #endregion

        #region ComboBox Handlers

        private void cbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                RefreshList();
            }), DispatcherPriority.Render);
        }

        #endregion

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void Import()
        {
            var win = PPRPApp.Windows.ImportMPD2562StatVoter;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadProvinces();
            }), DispatcherPriority.Render);
        }

        private void Export()
        {
            string msg = string.Empty;
            int thaiYear = 2562;
            var items = MPDStatVoter.Gets(thaiYear).Value();

            if (ExcelModel.SaveAs(items, "ข้อมูลผู้ใช้สิทธิปี " + thaiYear.ToString(), "ข้อมูลผู้ใช้สิทธิปี " + thaiYear + ".xlsx"))
            {
                msg += "ส่งออกข้อมูลสำเร็จ";
            }
            else
            {
                msg += "ส่งออกข้อมูลไม่สำเร็จ" + Environment.NewLine;
                msg += "อาจเกิดจากปัญหา ไม่ได้ทำการเลือกชื่อไฟล์, " + Environment.NewLine;
                msg += "ทำการเปิดไฟล์ค้างไว้ หรือไม่มีข้อมูลสำหรับการส่งออก " + Environment.NewLine;
                msg += "กรุณาตรวจสอบสาเหตุดังกล่าวก่อน แล้วทำการส่งออกใหม่อีกครั้ง";
            }

            var msgBox = PPRPApp.Windows.MessageBox;
            msgBox.Setup(msg, "ผลการส่งออกข้อมูล");
            msgBox.ShowDialog();
        }

        private void Refresh()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                RefreshList();
            }), DispatcherPriority.Render);
        }

        private void Search()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {

            }), DispatcherPriority.Render);
        }

        private void Print()
        {
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            int thaiYear = 2562;
            var items = MPDStatVoterPrintSummary.Gets(thaiYear, provinceName).Value();
            if (null == items)
            {
                // Show Dialog.
                return;
            }
            var page = PPRPApp.Pages.MPD2562PreviewStatVoterSummary;
            page.Setup(items);
            PageContentManager.Instance.Current = page;
        }

        private void LoadProvinces()
        {
            cbProvince.ItemsSource = null;
            var provinces = MProvince.Gets().Value();
            if (null != provinces)
            {
                provinces.Insert(0, new MProvince { ProvinceNameTH = "ทุกจังหวัด" });
            }
            cbProvince.ItemsSource = (null != provinces) ? provinces : new List<MProvince>();
            if (null != provinces)
            {
                cbProvince.SelectedIndex = 0;
            }
        }

        private void RefreshList()
        {
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            lvMPDVoters.ItemsSource = null;
            int year = 2562;
            var summaries = MPDStatVoter.Gets(thaiYear: year, provinceNameTH: provinceName);
            lvMPDVoters.ItemsSource = (null != summaries) ? summaries.Value() : new List<MPDStatVoter>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        /// <param name="reload">True for reload items</param>
        public void Setup(bool reload = true)
        {
            if (reload)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoadProvinces();
                }), DispatcherPriority.Render);
            }
        }

        #endregion
    }
}
