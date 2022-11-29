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
using PPRP.Models.Excel;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MProvinceManagePage.xaml
    /// </summary>
    public partial class MProvinceManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MProvinceManagePage()
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
            var win = PPRPApp.Windows.ImportMProvince;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            //LoadRegions();
        }

        private void Export()
        {

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

            }
        }

        #endregion
    }
}
