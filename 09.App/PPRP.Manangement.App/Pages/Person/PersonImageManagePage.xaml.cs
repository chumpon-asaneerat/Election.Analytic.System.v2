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

        private string sFullNameFilter = string.Empty;
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
            /*
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as PersonImage;
            Edit(item);
            */
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            /*
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as PersonImage;
            Delete(item);
            */
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
            var win = PPRPApp.Windows.ImportPersonImage;
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
            if (sFullNameFilter.Trim() != txtFullNameFilter.Text.Trim())
            {
                sFullNameFilter = txtFullNameFilter.Text.Trim();
                RefreshList();
            }
        }

        private void Print()
        {

        }

        private void AddNew()
        {

        }
        /*
        private void Edit(PersonImage item)
        {
            if (null == item)
                return;
            Console.WriteLine("Edit");
        }

        private void Delete(PersonImage item)
        {
            if (null == item)
                return;
            string msg = string.Format("ต้องการลบข้อมูล '{0}' ?", item.FullName);
            if (MessageBox.Show(msg, "ยืนยันการลบข้อมูล", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                PersonImage.Delete(item);
                RefreshList();
            }
        }
        */

        private void RefreshList()
        {
            /*
            lvPersons.ItemsSource = null;
            var persons = PersonImage.Gets(sFullNameFilter, iPageNo, iRowsPerPage);
            lvPersons.ItemsSource = (null != persons) ? persons.Value : new List<PersonImage>();

            if (null != persons)
            {
                lvPersons.SelectedIndex = 0;
                lvPersons.ScrollIntoView(lvPersons.SelectedItem);
            }

            iPageNo = (null != persons) ? persons.PageNo : 1;
            iMaxPage = (null != persons) ? persons.MaxPage : 1;

            nav.Setup(iPageNo, iMaxPage);
            */
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
                iPageNo = 1;
                iMaxPage = 1;

                RefreshList();
            }
        }

        #endregion
    }
}
