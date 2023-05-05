#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Reflection;

using NLib;
using NLib.Services;

using PPRP.Models;
using static PPRP.Pages.MPD2562VoteSummaryPage;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MPDMainSummaryPage.xaml
    /// </summary>
    public partial class MPDMainSummaryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDMainSummaryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Enum

        public enum View
        { 
            MPD2562,
            MPD2566
        }

        #endregion

        #region Internal Variables

        private ProvinceMenuItem _provinceItem = null;
        private PollingUnitMenuItem _pullingUnitItem = null;
        private View _view = View.MPD2562;
        private UserControl _currentPage = null;

        #endregion

        #region Helper Peroperties

        private PakMenuItem Current
        {
            get { return AreaNavi.Instance.Current; }
        }

        private List<ProvinceMenuItem> Provinces
        {
            get
            {
                var provinces = (null != AreaNavi.Instance.Current && null != AreaNavi.Instance.Current.Provinces) ?
                    AreaNavi.Instance.Current.Provinces : null;
                return provinces;
            }
        }

        #endregion

        #region Button Handlers

        private void cmdGotoThailandPage_Click(object sender, RoutedEventArgs e)
        {
            GotoThailandPage();
        }

        private void cmdGotoPrev_Click(object sender, RoutedEventArgs e)
        {
            GotoPrevPage();
        }

        #endregion

        #region lstPollingUnits Handlers

        private void lstPollingUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pollingUnit = lstPollingUnits.SelectedItem as PollingUnitMenuItem;
            RefreshContentPage(pollingUnit);
        }

        #endregion

        #region Private Methods

        private void GotoThailandPage()
        {
            var page = PPRPApp.Pages.Thailand;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPrevPage()
        {
            string regionId = (null != Current) ? Current.RegionId : string.Empty;
            AreaNavi.Instance.GotoPak(regionId);

            if (!string.IsNullOrWhiteSpace(regionId))
            {
                if (regionId == "01")
                {
                    var page = PPRPApp.Pages.Pak01;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "02")
                {
                    var page = PPRPApp.Pages.Pak02;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "03")
                {
                    var page = PPRPApp.Pages.Pak03;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "04")
                {
                    var page = PPRPApp.Pages.Pak04;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "05")
                {
                    var page = PPRPApp.Pages.Pak05;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "06")
                {
                    var page = PPRPApp.Pages.Pak06;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "07")
                {
                    var page = PPRPApp.Pages.Pak07;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "08")
                {
                    var page = PPRPApp.Pages.Pak08;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "09")
                {
                    var page = PPRPApp.Pages.Pak09;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
                else if (regionId == "10")
                {
                    var page = PPRPApp.Pages.Pak10;
                    page.Setup();
                    PageContentManager.Instance.Current = page;
                }
            }
        }

        private void RefreshContentPage(PollingUnitMenuItem item)
        {
            if (null == _currentPage) return;

            var pollingUnit = (null != item) ? item : lstPollingUnits.SelectedItem as PollingUnitMenuItem;

            if (_view == View.MPD2562 && _currentPage is MPD2562VoteSummaryPage)
            {
                // 2562
                (_currentPage as MPD2562VoteSummaryPage).Setup(this, pollingUnit);
            }
            else if (_view == View.MPD2566 && _currentPage is MPDCOfficial2566VoteSummaryPage)
            {
                // 2566
                (_currentPage as MPDCOfficial2566VoteSummaryPage).Setup(this, pollingUnit);
            }
        }

        #endregion

        #region Public Methods

        public void ChangeView(View view)
        {
            if (_view != view)
            {
                _view = view;
                if (_view == View.MPD2562)
                {
                    _currentPage = PPRPApp.Pages.MPD2562VoteSummary;
                    RefreshContentPage(_pullingUnitItem);
                    container.Content = _currentPage;
                }
                else
                {
                    _currentPage = PPRPApp.Pages.MPDCOfficial2566VoteSummary;
                    RefreshContentPage(_pullingUnitItem);
                    container.Content = _currentPage;
                }
            }
        }

        public void GotoMPD2562PrintPreview(MPDPrintVoteSummary2 item)
        {
            var page = PPRPApp.Pages.MPDPreviewVoteSummary;
            int idx = lstPollingUnits.SelectedIndex;
            page.Setup(_provinceItem, idx, item);
            PageContentManager.Instance.Current = page;
        }

        public void GotoMPD2566PrintPreview(MPDCOfficialPrintVoteSummary item)
        {
            var page = PPRPApp.Pages.MPDCOfficialPreviewVoteSummary;
            int idx = lstPollingUnits.SelectedIndex;
            page.Setup(_provinceItem, idx, item);
            PageContentManager.Instance.Current = page;
        }

        public void Setup(ProvinceMenuItem province)
        {
            _view = View.MPD2562; // default view
            _currentPage = PPRPApp.Pages.MPD2562VoteSummary;
            container.Content = _currentPage;

            txtProvinceName.Text = "จ.";
            _pullingUnitItem = null;
            lstPollingUnits.SelectedIndex = -1;
            lstPollingUnits.SelectedItem = null;
            lstPollingUnits.ItemsSource = null;

            _provinceItem = province;

            if (null == province)
                return;

            txtProvinceName.Text = "จ." + province.ProvinceNameTH;
            var items = PollingUnitMenuItem.Gets(province.RegionId, province.ADM1Code).Value();
            lstPollingUnits.ItemsSource = items;
            if (null != items && items.Count > 0)
            {
                lstPollingUnits.SelectedIndex = 0; // auto select first item.
                lstPollingUnits.ScrollIntoView(items[0]);
                RefreshContentPage(items[0]); // update display
            }
        }

        public void Setup(ProvinceMenuItem province, int selectIndex)
        {
            txtProvinceName.Text = "จ.";
            _pullingUnitItem = null;
            lstPollingUnits.SelectedIndex = -1;
            lstPollingUnits.SelectedItem = null;
            lstPollingUnits.ItemsSource = null;

            _provinceItem = province;

            if (null == province)
                return;

            txtProvinceName.Text = "จ." + province.ProvinceNameTH;
            var items = PollingUnitMenuItem.Gets(province.RegionId, province.ADM1Code).Value();
            lstPollingUnits.ItemsSource = items;
            if (null != items && items.Count > 0)
            {
                if (selectIndex > -1 && selectIndex < items.Count)
                {
                    lstPollingUnits.SelectedIndex = selectIndex; // auto select first item.
                    lstPollingUnits.ScrollIntoView(items[selectIndex]);
                    RefreshContentPage(items[selectIndex]); // update display
                }
                else
                {
                    lstPollingUnits.SelectedIndex = 0; // auto select first item.
                    lstPollingUnits.ScrollIntoView(items[0]);
                    RefreshContentPage(items[0]); // update display
                }
            }
        }

        #endregion
    }
}
