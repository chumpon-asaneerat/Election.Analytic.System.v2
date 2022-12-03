#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
            RefreshList();
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
            RefreshList();
        }

        private void Export()
        {

        }

        private void Refresh()
        {

        }

        private void Search()
        {
            if (sPartyNameFilter.Trim() != txtPartyNameFilter.Text.Trim())
            {
                sPartyNameFilter = txtPartyNameFilter.Text.Trim();
                RefreshList();
            }
        }

        private void Print()
        {

        }

        private void Edit(MParty item)
        {
            if (null == item)
                return;
            var editor = PPRPApp.Windows.PartyEditor;
            editor.ShowDialog();
            editor.Setup(item);
            RefreshList();
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
                RefreshList();
            }
        }

        private void RefreshList()
        {
            lvParties.ItemsSource = null;
            var parties = MParty.Gets(sPartyNameFilter, iPageNo, iRowsPerPage);
            lvParties.ItemsSource = (null != parties) ? parties.Value : new List<MParty>();

            if (null != parties)
            {
                lvParties.SelectedIndex = 0;
                lvParties.ScrollIntoView(lvParties.SelectedItem);
            }

            iPageNo = (null != parties) ? parties.PageNo : 1;
            iMaxPage = (null != parties) ? parties.MaxPage : 1;

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
                iPageNo = 1;
                iMaxPage = 1;

                RefreshList();
            }
        }

        #endregion
    }
}
