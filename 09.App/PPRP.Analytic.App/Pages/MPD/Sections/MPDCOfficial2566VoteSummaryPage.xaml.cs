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
using PPRP.Utils;

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

            public string Area62 { get; set; }
            public string Area66 { get; set; }  
            public List<MPDCOfficialVoteSummary> Top6 { get; set; }
        }

        #endregion

        #region Internal Variables

        //private ProvinceMenuItem _provinceItem = null;
        private MPDMainSummaryPage _parent = null;
        private PollingUnitMenuItem _pullingUnitItem = null;
        private GeneralSummary _generalSummary = null;

        private MPDCOfficialVoteSummary _currentEditItem = null;
        private TextBox _targetTextBox = null;

        #endregion

        #region Button Handlers

        private void cmdEdit_Click(object sender, RoutedEventArgs e)
        {
            ResetEditMode(); // reset prev item

            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDCOfficialVoteSummary;
            if (null == item) return;

            // Getting the currently selected ListBoxItem
            // Note that the ListBox must have
            // IsSynchronizedWithCurrentItem set to True for this to work
            ListBoxItem lstItem =
                (ListBoxItem)(lstSummary.ItemContainerGenerator.ContainerFromItem(lstSummary.Items.CurrentItem));

            // Getting the ContentPresenter of myListBoxItem
            ContentPresenter itemContentPresenter = FindVisualChild<ContentPresenter>(lstItem);
            if (null == itemContentPresenter) return;
            
            // Finding textBlock from the DataTemplate that is set on that ContentPresenter
            DataTemplate myDataTemplate = itemContentPresenter.ContentTemplate;
            if (null == myDataTemplate) return;

            TextBox targetTextBox = (TextBox)myDataTemplate.FindName("txtVoteCount", itemContentPresenter);
            if (null == targetTextBox) return;

            // change to edit mode
            item.BeginEdit();

            _currentEditItem = item; // keep item
            _targetTextBox = targetTextBox; // // keep control for check in LostFocus method.

            // Do something to the DataTemplate-generated TextBlock
            Dispatcher.Invoke(() =>
            {
                targetTextBox.Text = item.VoteCount.ToString();
                targetTextBox.Focus();
                targetTextBox.SelectAll();
            });
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDCOfficialVoteSummary;
            if (null == item) return;

            int voteCnt;
            string sVal = _targetTextBox.Text;
            if (int.TryParse(sVal, out voteCnt))
            {
                // commit changes
                item.VoteCount = voteCnt;
                item.Commit();
            }
            else
            {
                // invalid value so cancel.
                item.CancelEdit();
            }
            // save vote count.
            MPDCOfficialVoteSummary.UpdateVoteCount(item);

            _currentEditItem = null;
            _targetTextBox = null; // reset target

            LoadSummary(_pullingUnitItem); // reload.
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (null == btn) return;
            var item = btn.DataContext as MPDCOfficialVoteSummary;
            if (null == item) return;
            // Cancel Edit
            item.CancelEdit();

            _currentEditItem = null;
            _targetTextBox = null; // reset target
        }

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

        #region ListBox Handlers

        private void lstSummary_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetEditMode();
        }

        #endregion

        #region Private Methods

        #region WPF Helper

        private childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        #endregion

        private void ResetEditMode()
        {
            if (null != _currentEditItem)
            {
                _currentEditItem.CancelEdit();
            }
            _currentEditItem = null; // reset item
            _targetTextBox = null; // reset target
        }

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
            // province/polling unit.
            _generalSummary.ProvinceName = _pullingUnitItem.ProvinceNameTH;
            _generalSummary.PollingUnitNo = _pullingUnitItem.PollingUnitNo;
            // area 62
            var area62 = PollingUnit.Get(2562,
                    adm1code: _pullingUnitItem.ADM1Code,
                    pollingUnitNo: _pullingUnitItem.PollingUnitNo).Value();
            if (null != area62) _generalSummary.Area62 = area62.AreaRemark;
            // area 66
            var area66 = PollingUnit.Get(2566,
                    adm1code: _pullingUnitItem.ADM1Code,
                    pollingUnitNo: _pullingUnitItem.PollingUnitNo).Value();
            if (null != area66) _generalSummary.Area66 = area66.AreaRemark;

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
                item.AreaInfo62 = _generalSummary.Area62;
                item.AreaInfo66 = _generalSummary.Area66;

                int iCnt = _generalSummary.Top6.Count;
                if (null != _generalSummary.Top6 && iCnt > 0)
                {
                    // Person 1
                    if (iCnt >= 1 && null != _generalSummary.Top6[0])
                    {
                        var p = _generalSummary.Top6[0];
                        item.Logo1 = p.PartyImageData;
                        item.PersonImage1 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName1 = "1." + p.PartyName;
                        item.FullName1 = p.FullName;
                        item.VoteCount1 = p.VoteCount;

                        item.PrevPartyName1 = p.PrevPartyName;
                        item.PrevProvinceName1 = p.PrevProvinceNameTH;
                        item.PrevPollingUnitNo1 = (p.PrevPollingUnitNo.HasValue) ? p.PrevPollingUnitNo.Value : 0;
                        item.PrevVoteCount1 = (p.PrevVoteCount.HasValue) ? p.PrevVoteCount.Value : 0;
                        item.PrevRankNo1 = (p.PrevRankNo.HasValue) ? p.PrevRankNo.Value : 0;
                    }
                    // Person 2
                    if (iCnt >= 2 && null != _generalSummary.Top6[1])
                    {
                        var p = _generalSummary.Top6[1];
                        item.Logo2 = p.PartyImageData;
                        item.PersonImage2 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName2 = "2." + p.PartyName;
                        item.FullName2 = p.FullName;
                        item.VoteCount2 = p.VoteCount;

                        item.PrevPartyName2 = p.PrevPartyName;
                        item.PrevProvinceName2 = p.PrevProvinceNameTH;
                        item.PrevPollingUnitNo2 = (p.PrevPollingUnitNo.HasValue) ? p.PrevPollingUnitNo.Value : 0;
                        item.PrevVoteCount2 = (p.PrevVoteCount.HasValue) ? p.PrevVoteCount.Value : 0;
                        item.PrevRankNo2 = (p.PrevRankNo.HasValue) ? p.PrevRankNo.Value : 0;
                    }
                    // Person 3
                    if (iCnt >= 3 && null != _generalSummary.Top6[2])
                    {
                        var p = _generalSummary.Top6[2];
                        item.Logo3 = p.PartyImageData;
                        item.PersonImage3 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName3 = "3." + p.PartyName;
                        item.FullName3 = p.FullName;
                        item.VoteCount3 = p.VoteCount;

                        item.PrevPartyName3 = p.PrevPartyName;
                        item.PrevProvinceName3 = p.PrevProvinceNameTH;
                        item.PrevPollingUnitNo3 = (p.PrevPollingUnitNo.HasValue) ? p.PrevPollingUnitNo.Value : 0;
                        item.PrevVoteCount3 = (p.PrevVoteCount.HasValue) ? p.PrevVoteCount.Value : 0;
                        item.PrevRankNo3 = (p.PrevRankNo.HasValue) ? p.PrevRankNo.Value : 0;
                    }
                    // Person 4
                    if (iCnt >= 4 && null != _generalSummary.Top6[3])
                    {
                        var p = _generalSummary.Top6[3];
                        item.Logo4 = p.PartyImageData;
                        item.PersonImage4 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName4 = "4." + p.PartyName;
                        item.FullName4 = p.FullName;
                        item.VoteCount4 = p.VoteCount;

                        item.PrevPartyName4 = p.PrevPartyName;
                        item.PrevProvinceName4 = p.PrevProvinceNameTH;
                        item.PrevPollingUnitNo4 = (p.PrevPollingUnitNo.HasValue) ? p.PrevPollingUnitNo.Value : 0;
                        item.PrevVoteCount4 = (p.PrevVoteCount.HasValue) ? p.PrevVoteCount.Value : 0;
                        item.PrevRankNo4 = (p.PrevRankNo.HasValue) ? p.PrevRankNo.Value : 0;
                    }
                    // Person 5
                    if (iCnt >= 6 && null != _generalSummary.Top6[4])
                    {
                        var p = _generalSummary.Top6[4];
                        item.Logo5 = p.PartyImageData;
                        item.PersonImage5 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName5 = "5." + p.PartyName;
                        item.FullName5 = p.FullName;
                        item.VoteCount5 = p.VoteCount;

                        item.PrevPartyName5 = p.PrevPartyName;
                        item.PrevProvinceName5 = p.PrevProvinceNameTH;
                        item.PrevPollingUnitNo5 = (p.PrevPollingUnitNo.HasValue) ? p.PrevPollingUnitNo.Value : 0;
                        item.PrevVoteCount5 = (p.PrevVoteCount.HasValue) ? p.PrevVoteCount.Value : 0;
                        item.PrevRankNo5 = (p.PrevRankNo.HasValue) ? p.PrevRankNo.Value : 0;
                    }
                    // Person 6
                    if (iCnt >= 6 && null != _generalSummary.Top6[5])
                    {
                        var p = _generalSummary.Top6[5];
                        item.Logo6 = p.PartyImageData;
                        item.PersonImage6 = (null != p.PersonImageData) ? p.PersonImageData : Defaults.PersonBuffer;
                        item.PartyName6 = "6." + p.PartyName;
                        item.FullName6 = p.FullName;
                        item.VoteCount6 = p.VoteCount;

                        item.PrevPartyName6 = p.PrevPartyName;
                        item.PrevProvinceName6 = p.PrevProvinceNameTH;
                        item.PrevPollingUnitNo6 = (p.PrevPollingUnitNo.HasValue) ? p.PrevPollingUnitNo.Value : 0;
                        item.PrevVoteCount6 = (p.PrevVoteCount.HasValue) ? p.PrevVoteCount.Value : 0;
                        item.PrevRankNo6 = (p.PrevRankNo.HasValue) ? p.PrevRankNo.Value : 0;
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
