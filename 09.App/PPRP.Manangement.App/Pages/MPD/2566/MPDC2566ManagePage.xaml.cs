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
    /// Interaction logic for MPDC2566ManagePage.xaml
    /// </summary>
    public partial class MPDC2566ManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566ManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sFullNameFilter = string.Empty;
        private int iPageNo = 1;
        private int iMaxPage = 1;
        private int iRowsPerPage = 4;

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

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            // Edit selected item.
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            // Delete selected item.
        }

        #endregion

        #region ComboBox Handlers

        private void cbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                iPageNo = 1;
                iMaxPage = 1;

                RefreshList(true);
            }), DispatcherPriority.Render);
        }

        #endregion

        #region TextBox Handlers

        private void txtFullNameFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                e.Handled = true; // mark as handled
                // search
                Search();
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                e.Handled = true; // mark as handled
                // reset filter and search
                txtFullNameFilter.Text = string.Empty;
                Search();
            }
        }

        #endregion

        #region Paging Handlers

        private void nav_PagingChanged(object sender, EventArgs e)
        {
            iPageNo = nav.PageNo;
            RefreshList(false);
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
            var win = PPRPApp.Windows.ImportMPDC2566;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadProvinces();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtFullNameFilter.Focus();
                }), DispatcherPriority.Render);
            }), DispatcherPriority.Render);
        }

        private void Export()
        {
            /*
            string msg = string.Empty;
            int thaiYear = 2566;
            var items = MPDVoteSummary.Gets(thaiYear).Value();

            if (ExcelModel.SaveAs(items, "ว่าที่ผู้สมัคร " + thaiYear.ToString(), "รายชื่อว่าที่ผู้สมัครปี " + thaiYear + ".xlsx"))
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
            */
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

        private void Refresh()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {

            }), DispatcherPriority.Render);
        }

        private void Search()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (sFullNameFilter.Trim() != txtFullNameFilter.Text.Trim())
                {
                    sFullNameFilter = txtFullNameFilter.Text.Trim();
                    RefreshList(true);
                }
            }), DispatcherPriority.Render);
        }

        private void Print()
        {

        }

        private void RefreshList(bool refresh)
        {
            if (refresh)
            {
                iPageNo = 1;
            }

            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            int thaiYear = 2566;

            lvMPDC2566.ItemsSource = null;
            var candidates = MPDCPollingUnit.Gets(thaiYear, provinceName, sFullNameFilter, iPageNo, iRowsPerPage);

            lvMPDC2566.ItemsSource = (null != candidates) ? candidates.Value() : new List<MPDCPollingUnit>();
            sv.ScrollToHome(); // scroll to home position

            iPageNo = (null != candidates) ? candidates.PageNo : 1;
            iMaxPage = (null != candidates) ? candidates.MaxPage : 1;

            nav.Setup(iPageNo, iMaxPage);
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
                sFullNameFilter = string.Empty;

                txtFullNameFilter.Text = string.Empty;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoadProvinces();

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtFullNameFilter.Focus();
                    }), DispatcherPriority.Render);

                }), DispatcherPriority.Render);
            }
        }

        #endregion
    }
}
