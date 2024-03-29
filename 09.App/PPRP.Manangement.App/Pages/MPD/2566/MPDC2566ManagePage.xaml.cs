﻿#region Using

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
            AddNew();
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

        private void cmdDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            DeleteAll();
        }

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDC;
            Edit(item);
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDC;
            Delete(item);
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
            string msg = string.Empty;
            int thaiYear = 2566;
            var items = MPDC.GetExports(thaiYear).Value();

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
        }

        private void AddNew()
        {
            MProvince province = (null != cbProvince.SelectedItem) ? cbProvince.SelectedItem as MProvince : null;
            
            MPDC item = new MPDC();

            var editor = PPRPApp.Windows.MPDC2566Editor;
            editor.Setup(item, province, true);
            editor.ShowDialog();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(true);
                }), DispatcherPriority.Render);
        }

        private void Edit(MPDC item)
        {
            if (null == item)
                return;

            MProvince province = (null != cbProvince.SelectedItem) ? cbProvince.SelectedItem as MProvince : null;

            var editor = PPRPApp.Windows.MPDC2566Editor;
            editor.Setup(item, province, false);
            editor.ShowDialog();

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(true);
                }), DispatcherPriority.Render);
        }

        private void Delete(MPDC item)
        {
            if (null == item)
                return;

            string confitmMsg = string.Format("ต้องการลบข้อมูล '{0}' ?", item.FullName);
            var confirmWin = PPRPApp.Windows.MessageBoxOKCancel;
            confirmWin.Setup(confitmMsg, "ยืนยันการลบข้อมูล");

            if (confirmWin.ShowDialog() == true)
            {
                var ret = MPDC.Delete(item);
                if (null != ret && ret.HasError)
                {
                    string msg = string.Empty;
                    msg += string.Format("ไม่สามารถลบข้อมูล '{0}' ได้", item.FullName) + Environment.NewLine;
                    msg += ret.ErrMsg;
                    var msgWin = PPRPApp.Windows.MessageBox;
                    msgWin.Setup(msg, "PPRP");
                    msgWin.ShowDialog();
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(false);
                }), DispatcherPriority.Render);
            }
        }

        private void DeleteAll()
        {
            int thaiYear = 2566;
            string confitmMsg = string.Format("ต้องการลบข้อมูลว่าที่ผู้สมัครปี {0} ทั้งหมด ?", thaiYear);
            var confirmWin = PPRPApp.Windows.MessageBoxOKCancel;
            confirmWin.Setup(confitmMsg, "ยืนยันการลบข้อมูล");

            if (confirmWin.ShowDialog() == true)
            {
                var ret = MPDC.DeleteAll(thaiYear);
                if (null != ret && ret.HasError)
                {
                    string msg = string.Empty;
                    msg += string.Format("ไม่สามารถลบข้อมูล ว่าที่ผู้สมัครปี '{0}' ได้", thaiYear) + Environment.NewLine;
                    msg += ret.ErrMsg;
                    var msgWin = PPRPApp.Windows.MessageBox;
                    msgWin.Setup(msg, "PPRP");
                    msgWin.ShowDialog();
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(false);
                }), DispatcherPriority.Render);
            }
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
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            int thaiYear = 2566;
            var items = MPDC.GetExports(thaiYear, provinceName).Value();
            if (null == items)
            {
                // Show Dialog.
                return;
            }
            var page = PPRPApp.Pages.MPDC2566PreviewSummaryPage;
            page.Setup(items);
            PageContentManager.Instance.Current = page;
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

            if (iPageNo > iMaxPage) iPageNo = iMaxPage;

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
