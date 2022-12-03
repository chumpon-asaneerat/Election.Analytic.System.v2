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
    /// Interaction logic for PersonImageManagePage.xaml
    /// </summary>
    public partial class PersonImageManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PersonImageManagePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private string sPrefixFilter = string.Empty;
        private string sFirstNameFilter = string.Empty;
        private string sLastNameFilter = string.Empty;
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
            var item = btn.DataContext as MPerson;
            Edit(item);
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPerson;
            Delete(item);
        }

        #endregion

        #region TextBox Handlers

        private void txtPrefixFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
                txtPrefixFilter.Text = string.Empty;
                Search();
            }
        }

        private void txtFirstNameFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
                txtFirstNameFilter.Text = string.Empty;
                Search();
            }
        }

        private void txtLastNameFilter_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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
                txtLastNameFilter.Text = string.Empty;
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
            var win = PPRPApp.Windows.ImportPersonImage;
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

            }), DispatcherPriority.Render);
        }

        private void Search()
        {
            bool refresh = false;

            if (sPrefixFilter.Trim() != txtPrefixFilter.Text.Trim())
            {
                sPrefixFilter = txtPrefixFilter.Text.Trim();
                refresh = true;
            }
            if (sFirstNameFilter.Trim() != txtFirstNameFilter.Text.Trim())
            {
                sFirstNameFilter = txtFirstNameFilter.Text.Trim();
                refresh = true;
            }
            if (sLastNameFilter.Trim() != txtLastNameFilter.Text.Trim())
            {
                sLastNameFilter = txtLastNameFilter.Text.Trim();
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

        }

        private void Edit(MPerson item)
        {
            if (null == item)
                return;
            var editor = PPRPApp.Windows.PersonEditor;
            editor.Setup(item);
            if (editor.ShowDialog() == true)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(true);
                }), DispatcherPriority.Render);
            }
        }

        private void Delete(MPerson item)
        {
            if (null == item)
                return;
            string confitmMsg = string.Format("ต้องการลบข้อมูล '{0}' ?", item.FullName);
            var confirmWin = PPRPApp.Windows.MessageBoxOKCancel;
            confirmWin.Setup(confitmMsg, "ยืนยันการลบข้อมูล");

            if (confirmWin.ShowDialog() == true)
            {
                var ret = MPerson.Delete(item);
                if (null != ret && ret.HasError)
                {
                    string msg = string.Empty;
                    msg += string.Format("ไม่สามารถลบข้อมูล '{0}' ได้", item.FullName) + Environment.NewLine;
                    msg += "เนื่องจากข้อมูลดังกล่าว ถูกอ้างอิงถึงในส่วนอื่น ๆ ของฐานข้อมูล";
                    var msgWin = PPRPApp.Windows.MessageBox;
                    msgWin.Setup(msg, "PPRP");
                    msgWin.ShowDialog();
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(true);
                }), DispatcherPriority.Render);
            }
        }

        private void RefreshList(bool refresh)
        {
            if (refresh)
            {
                iPageNo = 1;
            }

            lvPersons.ItemsSource = null;
            var persons = MPerson.Gets(sPrefixFilter, sFirstNameFilter, sLastNameFilter, 
                iPageNo, iRowsPerPage);
            lvPersons.ItemsSource = (null != persons) ? persons.Value : new List<MPerson>();

            var sv = lvPersons.GetChildOfType<ScrollViewer>();
            if (null != sv)
            {
                sv.ScrollToHome();
            }

            iPageNo = (null != persons) ? persons.PageNo : 1;
            iMaxPage = (null != persons) ? persons.MaxPage : 1;

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
                sPrefixFilter = string.Empty;
                sFirstNameFilter = string.Empty;
                sLastNameFilter = string.Empty;

                iPageNo = 1;
                iMaxPage = 1;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    RefreshList(true);
                }), DispatcherPriority.Render);
            }
        }

        #endregion
    }
}
