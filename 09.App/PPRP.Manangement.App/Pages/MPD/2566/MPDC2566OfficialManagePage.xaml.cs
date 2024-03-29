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
    /// Interaction logic for MPDC2566OfficialManagePage.xaml
    /// </summary>
    public partial class MPDC2566OfficialManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDC2566OfficialManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sFullNameFilter = string.Empty;
        private string sPartyNameFilter = string.Empty;

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

        private void cmdView_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDCOfficial;
            ViewDetail(item);
        }

        private void cmdDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            DeleteAll();
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

        #region TextBox Handlers

        private void txtPartyNameFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
                txtPartyNameFilter.Text = string.Empty;
                Search();
            }
        }

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

        #region Private Methods

        private void GotoMainMenuPage()
        {
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void Import()
        {
            var win = PPRPApp.Windows.ImportMPDC2566Official;
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
            int thaiYear = 2566;
            var items = MPDCOfficial.Gets(thaiYear).Value();

            if (ExcelModel.SaveAs(items, "ข้อมูลผู้สมัคร ส.ส อย่างเป็นทางการ ปี" + thaiYear.ToString(), "ข้อมูลผู้สมัคร ส.ส อย่างเป็นทางการปี " + thaiYear + ".xlsx"))
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

        private void DeleteAll()
        {
            int thaiYear = 2566;
            string confitmMsg = string.Format("ต้องการลบข้อมูลผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทนแบบแบ่งเขต อย่างเป็นทางการปี {0} ทั้งหมด ?", thaiYear);
            var confirmWin = PPRPApp.Windows.MessageBoxOKCancel;
            confirmWin.Setup(confitmMsg, "ยืนยันการลบข้อมูล");

            if (confirmWin.ShowDialog() == true)
            {
                var ret = MPDCOfficial.DeleteAll(thaiYear);
                if (null != ret && ret.HasError)
                {
                    string msg = string.Empty;
                    msg += string.Format("ไม่สามารถลบข้อมูลผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทนแบบแบ่งเขต อย่างเป็นทางการปี '{0}' ได้", thaiYear) + Environment.NewLine;
                    msg += ret.ErrMsg;
                    var msgWin = PPRPApp.Windows.MessageBox;
                    msgWin.Setup(msg, "PPRP");
                    msgWin.ShowDialog();
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList();
                }), DispatcherPriority.Render);
            }
        }

        private void Search()
        {
            bool refresh = false;
            if (sPartyNameFilter.Trim() != txtPartyNameFilter.Text.Trim())
            {
                sPartyNameFilter = txtPartyNameFilter.Text.Trim();
                refresh = true;
            }
            if (sFullNameFilter.Trim() != txtFullNameFilter.Text.Trim())
            {
                sFullNameFilter = txtFullNameFilter.Text.Trim();
                refresh = true;
            }

            if (refresh)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList();
                }), DispatcherPriority.Render);
            }
        }

        private void Print()
        {
            /*
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }
            int thaiYear = 2562;

            var items = MPDPrintVoteSummary.Gets(thaiYear, provinceName, sPartyNameFilter, sFullNameFilter).Value();
            if (null == items)
            {
                // Show Dialog.
                return;
            }

            var page = PPRPApp.Pages.MPD2562PreviewVoteSummary;
            page.Setup(items);
            PageContentManager.Instance.Current = page;
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

        private void RefreshList()
        {
            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            int thaiYear = 2566;
            lvMPDSummaries.ItemsSource = null;
            var summaries = MPDCOfficial.Gets(
                thaiYear: thaiYear,
                provinceNameTH: provinceName,
                partyName: sPartyNameFilter,
                fullName: sFullNameFilter).Value();
            lvMPDSummaries.ItemsSource = (null != summaries) ? summaries : new List<MPDCOfficial>();
        }

        private void ViewDetail(MPDCOfficial item)
        {
            var win = PPRPApp.Windows.MPDC2566OfficialViewer;
            win.Setup(item);
            if (win.ShowDialog() == false)
            {
                return;
            }
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
                sPartyNameFilter = string.Empty;
                sFullNameFilter = string.Empty;

                txtPartyNameFilter.Text = string.Empty;
                txtFullNameFilter.Text = string.Empty;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoadProvinces();

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtPartyNameFilter.Focus();
                    }), DispatcherPriority.Render);

                }), DispatcherPriority.Render);
            }
        }

        #endregion
    }
}
