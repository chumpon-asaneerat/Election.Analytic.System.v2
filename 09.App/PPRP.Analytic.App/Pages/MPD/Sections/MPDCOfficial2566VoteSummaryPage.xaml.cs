﻿#region Using

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

        #region Internal Class

        public class GeneralSummary
        {
            public string ProvinceName { get; set; }
            public int PollingUnitNo { get; set; }

            public List<MPDCOfficialVoteSummary> Top6 { get; set; }
        }

        #endregion

        #region Internal Variables

        //private ProvinceMenuItem _provinceItem = null;
        private MPDMainSummaryPage _parent = null;
        private PollingUnitMenuItem _pullingUnitItem = null;
        private GeneralSummary _generalSummary = null;

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
            var top6 = MPDCOfficialVoteSummary.Gets(
                    thaiYear, prevThaiYear, _pullingUnitItem.ADM1Code, _pullingUnitItem.PollingUnitNo, 6).Value();

            lstSummary.ItemsSource = top6;

            // Create cache summary for print.
            _generalSummary = new GeneralSummary();

            _generalSummary.ProvinceName = _pullingUnitItem.ProvinceNameTH;
            _generalSummary.PollingUnitNo = _pullingUnitItem.PollingUnitNo;

            _generalSummary.Top6 = top6;
        }

        private void GotoPrintPreview()
        {
            if (_parent == null) return;

            // prepare report item.
            MPDCOfficialPrintVoteSummary item = new MPDCOfficialPrintVoteSummary();
            if (null != _generalSummary)
            {
                // Province Name/PollingUnitNo
                item.ProvinceName = _generalSummary.ProvinceName;
                item.PollingUnitNo = _generalSummary.PollingUnitNo;

                if (null != _generalSummary.Top6 && _generalSummary.Top6.Count > 0)
                {
                    // Person 1
                    if (null != _generalSummary.Top6[0])
                    {
                        var p = _generalSummary.Top6[0];
                        item.Logo1 = p.PartyImageData;
                        item.PersonImage1 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName1 = "1." + p.PartyName;
                        item.FullName1 = p.FullName;
                        item.VoteCount1 = p.VoteCount;
                    }
                    // Person 2
                    if (null != _generalSummary.Top6[1])
                    {
                        var p = _generalSummary.Top6[1];
                        item.Logo2 = p.PartyImageData;
                        item.PersonImage2 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName2 = "2." + p.PartyName;
                        item.FullName2 = p.FullName;
                        item.VoteCount2 = p.VoteCount;
                    }
                    // Person 3
                    if (null != _generalSummary.Top6[2])
                    {
                        var p = _generalSummary.Top6[2];
                        item.Logo3 = p.PartyImageData;
                        item.PersonImage3 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName3 = "3." + p.PartyName;
                        item.FullName3 = p.FullName;
                        item.VoteCount3 = p.VoteCount;
                    }
                    // Person 4
                    if (null != _generalSummary.Top6[3])
                    {
                        var p = _generalSummary.Top6[3];
                        item.Logo4 = p.PartyImageData;
                        item.PersonImage4 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName4 = "4." + p.PartyName;
                        item.FullName4 = p.FullName;
                        item.VoteCount4 = p.VoteCount;
                    }
                    // Person 5
                    if (null != _generalSummary.Top6[4])
                    {
                        var p = _generalSummary.Top6[4];
                        item.Logo5 = p.PartyImageData;
                        item.PersonImage5 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName5 = "5." + p.PartyName;
                        item.FullName5 = p.FullName;
                        item.VoteCount5 = p.VoteCount;
                    }
                    // Person 6
                    if (null != _generalSummary.Top6[5])
                    {
                        var p = _generalSummary.Top6[5];
                        item.Logo6 = p.PartyImageData;
                        item.PersonImage6 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName6 = "6." + p.PartyName;
                        item.FullName6 = p.FullName;
                        item.VoteCount6 = p.VoteCount;
                    }
                }
            }
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
