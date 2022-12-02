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
    /// Interaction logic for MSubdistrictManagePage.xaml
    /// </summary>
    public partial class MSubdistrictManagePage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MSubdistrictManagePage()
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

        #endregion

        #region ComboBox Handlers

        private void cbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProvinces();
        }

        private void cbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            var win = PPRPApp.Windows.ImportMSubdistrict;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }
            LoadRegions();
        }

        private void Export()
        {

        }

        private void Refresh()
        {

        }

        private void Search()
        {

        }

        private void Print()
        {

        }

        private void LoadRegions()
        {
            cbRegion.ItemsSource = null;
            var regions = MRegion.Gets().Value;
            if (null != regions)
            {
                regions.Insert(0, new MRegion { RegionName = "ทุกภาค" });
            }
            cbRegion.ItemsSource = (null != regions) ? regions : new List<MRegion>();
            if (null != regions)
            {
                cbRegion.SelectedIndex = 0;
            }
        }

        private void LoadProvinces()
        {
            // Check region.
            var reion = cbRegion.SelectedItem as MRegion;
            string regionId = (null != reion) ? reion.RegionId : null;
            if (null != regionId && regionId.Contains("ทุกภาค"))
            {
                regionId = null;
            }

            cbProvince.ItemsSource = null;
            var provinces = MProvince.Gets(regionId: regionId).Value;
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
            // Check region.
            var reion = cbRegion.SelectedItem as MRegion;
            string regionId = (null != reion) ? reion.RegionId : null;
            if (null == regionId || string.IsNullOrWhiteSpace(regionId))
            {
                regionId = null;
            }

            // Check province.
            var province = cbProvince.SelectedItem as MProvince;
            string provinceName = (null != province) ? province.ProvinceNameTH : null;
            if (null != provinceName && provinceName.Contains("ทุกจังหวัด"))
            {
                provinceName = null;
            }

            lvSubdistricts.ItemsSource = null;
            var subdistricts = MSubdistrict.Gets(regionId: regionId, provinceNameTH: provinceName);
            lvSubdistricts.ItemsSource = (null != subdistricts) ? subdistricts.Value : new List<MSubdistrict>();
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
                LoadRegions();
            }
        }

        #endregion
    }
}
