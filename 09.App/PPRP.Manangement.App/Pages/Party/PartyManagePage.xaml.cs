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
    /// Interaction logic for PartyManagePage.xaml
    /// </summary>
    public partial class PartyManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PartyManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sPartyNameFilter = string.Empty;
        private int iPageNo = 1;
        private int iMaxPage = 1;
        private int iRowsPerPage = 40;

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

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MParty;
            Edit(item);
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MParty;
            Delete(item);
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

        #endregion

        #region Paging Handlers

        private void nav_PagingChanged(object sender, EventArgs e)
        {
            iPageNo = nav.PageNo;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                RefreshList(false);
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
            var win = PPRPApp.Windows.ImportPartyImage;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                RefreshList(true);
            }), DispatcherPriority.Render);
        }

        private void Export()
        {

        }

        private void Refresh()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                RefreshList(false);
            }), DispatcherPriority.Render);
        }

        private void Search()
        {
            bool refresh = false;
            if (sPartyNameFilter.Trim() != txtPartyNameFilter.Text.Trim())
            {
                sPartyNameFilter = txtPartyNameFilter.Text.Trim();
                refresh = true;
            }
            if (refresh)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(true);
                }), DispatcherPriority.Render);
            }
        }

        private void Print()
        {

        }

        private void AddNew()
        {
            MParty item = new MParty();
            var editor = PPRPApp.Windows.PartyEditor;
            editor.Setup(item, true); // add new mode
            if (editor.ShowDialog() == true)
            {
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                // there are case change current item image and cancel so need to refresh.
                RefreshList(true);
            }), DispatcherPriority.Render);
        }

        private void Edit(MParty item)
        {
            if (null == item)
                return;
            var editor = PPRPApp.Windows.PartyEditor;
            editor.Setup(item); // edit mode
            if (editor.ShowDialog() == true)
            {
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                // there are case change current item image and cancel so need to refresh.
                RefreshList(true);
            }), DispatcherPriority.Render);
        }

        private void Delete(MParty item)
        {
            if (null == item)
                return;
            string confitmMsg = string.Format("ต้องการลบข้อมูล '{0}' ?", item.PartyName);
            var confirmWin = PPRPApp.Windows.MessageBoxOKCancel;
            confirmWin.Setup(confitmMsg, "ยืนยันการลบข้อมูล");

            if (confirmWin.ShowDialog() == true)
            {
                var ret = MParty.Delete(item);
                if (null != ret && ret.HasError)
                {
                    string msg = string.Empty;
                    msg += string.Format("ไม่สามารถลบข้อมูล '{0}' ได้", item.PartyName) + Environment.NewLine;
                    msg += "เนื่องจากข้อมูลดังกล่าว ถูกอ้างอิงถึงในส่วนอื่น ๆ ของฐานข้อมูล";
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

        private void RefreshList(bool refresh)
        {
            if (refresh)
            {
                iPageNo = 1;
            }

            lvParties.ItemsSource = null;
            var parties = MParty.Gets(sPartyNameFilter, iPageNo, iRowsPerPage);
            lvParties.ItemsSource = (null != parties) ? parties.Value() : new List<MParty>();

            var sv = lvParties.GetChildOfType<ScrollViewer>();
            if (null != sv)
            {
                sv.ScrollToHome();
            }

            iPageNo = (null != parties) ? parties.PageNo : 1;
            iMaxPage = (null != parties) ? parties.MaxPage : 1;

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
                sPartyNameFilter = string.Empty;
                txtPartyNameFilter.Text = string.Empty;

                iPageNo = 1;
                iMaxPage = 1;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(true);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        txtPartyNameFilter.Focus(); // Focus on party name.
                    }), DispatcherPriority.Render);
                }), DispatcherPriority.Render);
            }
        }

        #endregion
    }
}
