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
    /// Interaction logic for ImportShapeMapsManangePage.xaml
    /// </summary>
    public partial class ImportShapeMapsManangePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportShapeMapsManangePage()
        {
            InitializeComponent();
        }

        #endregion

        #region Button Handlers

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {

        }

        #endregion

        private void cmdImportADM0_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdImportADM1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdImportADM2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdImportADM3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
