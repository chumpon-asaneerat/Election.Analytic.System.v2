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
            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadProvinces();
            }), DispatcherPriority.Render);
        }

        private void cbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadDistricts();
            }), DispatcherPriority.Render);
        }

        private void cbDistrict_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                RefreshList();
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
            var win = PPRPApp.Windows.ImportMSubdistrict;
            win.Setup();
            if (win.ShowDialog() == false)
            {
                return;
            }

            Dispatcher.BeginInvoke(new Action(() =>
            {
                LoadRegions();
            }), DispatcherPriority.Render);
        }

        private void Export()
        {
            string msg = string.Empty;
            var items = MADM3.Gets().Value;

            if (ExcelModel.SaveAs(items, "adm3", "tha_adm_areas_adm3.xlsx"))
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
            RefreshList();
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

        private void LoadDistricts()
        {
            // Check region.
            var reion = cbRegion.SelectedItem as MRegion;
            string regionId = (null != reion) ? reion.RegionId : null;
            if (null != regionId && regionId.Contains("ทุกภาค"))
            {
                regionId = null;
            }

            // Check province
            var province = cbProvince.SelectedItem as MProvince;
            string adm1Code = (null != province) ? province.ADM1Code : null;
            if (string.IsNullOrWhiteSpace(adm1Code))
            {
                adm1Code = null;
            }

            cbDistrict.ItemsSource = null;
            var districts = MDistrict.Gets(regionId, adm1Code, null).Value;
            if (null != districts)
            {
                districts.Insert(0, new MDistrict { DistrictNameTH = "ทุกอำเภอ/เขต" });
            }
            cbDistrict.ItemsSource = (null != districts) ? districts : new List<MDistrict>();
            if (null != districts)
            {
                cbDistrict.SelectedIndex = 0;
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
            string adm1Code = (null != province) ? province.ADM1Code : null;
            if (string.IsNullOrWhiteSpace(adm1Code))
            {
                adm1Code = null;
            }

            // Check district.
            var district = cbDistrict.SelectedItem as MDistrict;
            string adm2Code = (null != district) ? district.ADM2Code : null;
            if (string.IsNullOrWhiteSpace(adm2Code))
            {
                adm2Code = null;
            }

            lvSubdistricts.ItemsSource = null;
            var subdistricts = MSubdistrict.Gets(regionId, adm1Code, adm2Code, null);
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
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    LoadRegions();
                }), DispatcherPriority.Render);
            }
        }

        #endregion
    }
}
