#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using NLib;
using NLib.Services;

using PPRP.Models;
using PPRP.Models.ShapeFiles;
using PPRP.Services;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for ShapeMapViewPage.xaml
    /// </summary>
    public partial class ShapeMapViewPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ShapeMapViewPage()
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
            Disconnect();
            var page = PPRPApp.Pages.MainMenu;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void Connect()
        {
            ShapeMapDbService.Instance.Start();
        }

        private void Disconnect()
        {
            ShapeMapDbService.Instance.Shutdown();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            Connect();
        }

        #endregion
    }
}
