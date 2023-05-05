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
using System.Security.Policy;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MPDCOfficial2566VoteSummaryPage.xaml
    /// </summary>
    public partial class MPDCOfficial2566VoteSummaryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPDCOfficial2566VoteSummaryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        //private ProvinceMenuItem _provinceItem = null;
        private MPDMainSummaryPage _parent = null;
        private PollingUnitMenuItem _pullingUnitItem = null;

        #endregion

        #region Button Handlers

        private void cmdAreaInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowAreaInfo();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            GotoPrintPreview();
        }

        private void cmdSwitch2562_Click(object sender, RoutedEventArgs e)
        {
            ChangeView();
        }

        #endregion

        #region Private Methods

        private void ChangeView()
        {
            if (_parent == null) return;
            _parent.ChangeView(MPDMainSummaryPage.View.MPD2562);
        }

        private void ShowAreaInfo()
        {
            PollingUnit summary = null;

            if (null != _pullingUnitItem)
            {
                int thaiYear = 2566;
                summary = PollingUnit.Get(thaiYear,
                    adm1code: _pullingUnitItem.ADM1Code,
                    pollingUnitNo: _pullingUnitItem.PollingUnitNo).Value();
            }

            var win = PPRPApp.Windows.MPDCAreaRemark;
            win.Setup(summary);
            win.ShowDialog();
        }

        private void LoadSummary(PollingUnitMenuItem item)
        {
            _pullingUnitItem = item;
            txtPollingUnitInfo.Text = "-";

            if (null == _pullingUnitItem)
                return;

            txtPollingUnitInfo.Text = _pullingUnitItem.DisplayText;

            int thaiYear = 2566;
            int prevThaiYear = 2562;
            lstSummary.ItemsSource = null;
            if (null != _pullingUnitItem)
            {
                lstSummary.ItemsSource = MPDCOfficialVoteSummary.Gets(
                    thaiYear, prevThaiYear, _pullingUnitItem.ADM1Code, _pullingUnitItem.PollingUnitNo, 6).Value();
            }
        }

        private void GotoPrintPreview()
        {
            if (_parent == null) return;

            // prepare report item.
            MPDCOfficialPrintVoteSummary item = new MPDCOfficialPrintVoteSummary();

            _parent.GotoMPD2566PrintPreview(item);
        }

        #endregion

        #region Public Methods

        public void Setup(MPDMainSummaryPage parent, PollingUnitMenuItem value)
        {
            _parent = parent;
            _pullingUnitItem = value;
            LoadSummary(value);
        }

        #endregion
    }
}
