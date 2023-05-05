#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Reflection;

using NLib;
using NLib.Services;

using PPRP.Models;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for Pak03Page.xaml
    /// </summary>
    public partial class Pak03Page : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public Pak03Page()
        {
            InitializeComponent();
        }

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

        private void cmdProvince_Click(object sender, RoutedEventArgs e)
        {
            var province = (sender as Hyperlink).DataContext as ProvinceMenuItem;
            GotoVoteSummaryPage(province);
        }

        #endregion

        #region Private Methods

        private void GotoThailandPage()
        {
            var page = PPRPApp.Pages.Thailand;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoVoteSummaryPage(ProvinceMenuItem province)
        {
            if (null == province)
                return;
            var page = PPRPApp.Pages.MPDMainSummary;
            page.Setup(province);
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Public Methods

        public void Setup()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            if (null == Provinces || Provinces.Count <= 0)
            {
                med.Info("Provinces is null or Count : 0");

            }
            else
            {
                med.Info("No of Provinces : {0}", Provinces.Count);
            }

            this.DataContext = Current;
            lstProvinces.ItemsSource = Provinces;
        }

        #endregion

        #region Nav Click

        private void cmCmd_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (sender as Button);
            string provinceName = btn.Tag.ToString();
            var province = Provinces.Find((prov) => { return prov.ProvinceNameTH == provinceName; });
            if (null != province)
            {
                GotoVoteSummaryPage(province);
            }
        }

        #endregion
    }
}
