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

using PPRP.Controls;

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

        #region v1 (unused now)

        //private VisualHost host;

        private void UpdateMaps()
        {
            /*
            if (null == host)
            {
                host = new VisualHost();
                host.Visual = new ThailandDrawingVisual();
                host.Canvas = canvas;
                canvas.Children.Add(host);
            }
            //host.RefreshTransforms();
            */
        }

        #endregion

        private void LoadProvinces()
        {
            lv.ItemsSource = null;
            var provinces = LProvince.Gets().Value();
            lv.ItemsSource = provinces;
        }

        private void UpdateProvince(LProvince province)
        {
            if (null == province) return;
            string admCode = province.ADM1Code;
            var adm = LADM1.Get(admCode).Value();
            if (null == adm) return;

            DateTime dt = DateTime.Now;
            ADMShape shape = new ADMShape();
            shape.Load(adm);
            TimeSpan ts = DateTime.Now - dt;
            // update elapse time.
            txtElapse.Text = string.Format("Province: {0}, load time: {1:n3} ms.", 
                province.ProvinceName, ts.TotalMilliseconds);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup()
        {
            Connect();
            LoadProvinces();
        }

        #endregion

        #region v1 (unused now)

        private void cmdLoadTH_Click(object sender, RoutedEventArgs e)
        {
            UpdateMaps();
        }

        #endregion

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (null != host) host.RefreshTransforms(); // v1 (unused now)
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var province = lv.SelectedItem as LProvince;
            if (null == province) return;
            UpdateProvince(province);
        }
    }
}
