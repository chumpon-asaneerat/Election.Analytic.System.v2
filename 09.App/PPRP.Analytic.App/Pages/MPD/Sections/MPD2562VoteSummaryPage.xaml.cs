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

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MPD2562VoteSummaryPage.xaml
    /// </summary>
    public partial class MPD2562VoteSummaryPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MPD2562VoteSummaryPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Class

        public class GeneralSummary
        {
            public string ProvinceName { get; set; }
            public int PollingUnitNo { get; set; }

            public List<MPDPersonalVoteSummary> Top6 { get; set; }
            public int PollingUnitCount { get; set; }
            public int RightCount { get; set; }
            public int ExerciseCount { get; set; }
            public int VoteCount7toLast { get; set; }
            public MPDCSummary CandidateNo1 { get; set; }
        }

        #endregion

        #region Internal Variables

        //private ProvinceMenuItem _provinceItem = null;
        private MPDMainSummaryPage _parent = null;
        private PollingUnitMenuItem _pullingUnitItem = null;
        private GeneralSummary _generalSummary = null;

        #endregion

        #region Button Handlers

        private void ChangeView()
        {
            if (_parent == null) return;
            _parent.ChangeView(MPDMainSummaryPage.View.MPD2566);
        }

        private void cmdAreaInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowAreaInfo();
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            GotoPrintPreview();
        }
        /*
        private void cmdMPDC2566OfficialInfo_Click(object sender, RoutedEventArgs e)
        {
            ShowMPDC2566OfficialView();
        }
        */
        private void cmdSwitch2566_Click(object sender, RoutedEventArgs e)
        {
            ChangeView();
        }

        #endregion

        #region lstCandidates ListBoxItem Handlers

        private void lstCandidates_ListBoxItem_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Uncomment code in xaml to use this method.
            var item = sender as ListBoxItem;
            if (null == item || null == item.DataContext) return;
            var inst = item.DataContext as MPDCSummary;
            ShowPreview(inst);
        }

        private void lstCandidates_ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as ListBoxItem;
            if (null == item || null == item.DataContext) return;
            var inst = item.DataContext as MPDCSummary;
            ShowPreview(inst);
        }

        #endregion

        #region Private Methods

        private void ShowPreview(MPDCSummary inst)
        {
            if (null == inst) return;

            var win = PPRPApp.Windows.MPDCPreview;
            win.Setup(inst);
            win.ShowDialog();
        }

        private void ShowAreaInfo()
        {
            PollingUnit summary = null;

            if (null != _pullingUnitItem)
            {
                int thaiYear = 2562;
                summary = PollingUnit.Get(thaiYear,
                    adm1code: _pullingUnitItem.ADM1Code, 
                    pollingUnitNo: _pullingUnitItem.PollingUnitNo).Value();
            }

            var win = PPRPApp.Windows.MPDCAreaRemark;
            win.Setup(summary);
            win.ShowDialog();
        }

        private void LoadSummary(PollingUnitMenuItem pollingUnit)
        {
            if (null != _pullingUnitItem && null != pollingUnit)
            {
                if (_pullingUnitItem.ADM1Code == pollingUnit.ADM1Code &&
                    _pullingUnitItem.PollingUnitNo == pollingUnit.PollingUnitNo)
                {
                    // same item.
                    return;
                }
            }

            txtPollingUnitInfo.Text = "-";
            txtTotalVotes.Text = "-";
            txtPollingUnitCount.Text = "-";
            txtRightCount.Text = "-";
            txtExerciseCount.Text = "-";

            _pullingUnitItem = pollingUnit; // keep current

            if (null == _pullingUnitItem)
                return;

            txtPollingUnitInfo.Text = pollingUnit.DisplayText;

            int MPDThaiYear = 2562;
            var top6 = MPDPersonalVoteSummary.Gets(MPDThaiYear,
                _pullingUnitItem.ADM1Code, _pullingUnitItem.PollingUnitNo, 6).Value();

            int sum6 = 0;
            if (null != top6 && top6.Count > 0)
            {
                txtSummaryNoInfo.Visibility = Visibility.Hidden;

                foreach (var item in top6)
                {
                    sum6 += item.VoteCount;
                }
            }
            else
            {
                txtSummaryNoInfo.Visibility = Visibility.Visible;
            }

            int sumAll = MPDPersonalVoteSummary.GetTotalVotes(MPDThaiYear, 
                _pullingUnitItem.ADM1Code, _pullingUnitItem.PollingUnitNo);

            int diff = sumAll - sum6; // sum from 7-last
            txtTotalVotes.Text = diff.ToString("n0");

            lstSummary.ItemsSource = top6;

            // get candidates.
            int candidateThaiYear = 2566;
            var candidates = MPDCSummary.Gets(
                candidateThaiYear, _pullingUnitItem.ADM1Code, _pullingUnitItem.PollingUnitNo, 4).Value();
            lstCandidates.ItemsSource = candidates;

            if (null == candidates || candidates.Count <= 0)
            {
                txtCnadidateNoInfo.Visibility = Visibility.Visible;
            }
            else
            {
                txtCnadidateNoInfo.Visibility = Visibility.Hidden;
            }

            // Create cache summary for print.
            _generalSummary = new GeneralSummary();

            _generalSummary.ProvinceName = pollingUnit.ProvinceNameTH;
            _generalSummary.PollingUnitNo = pollingUnit.PollingUnitNo;

            _generalSummary.Top6 = top6;
            _generalSummary.CandidateNo1 = (null != candidates && candidates.Count > 0) ? candidates[0] : null;
            _generalSummary.VoteCount7toLast = diff;

            UpdatePollingUnitSummary(MPDThaiYear, _pullingUnitItem.ADM1Code, _pullingUnitItem.PollingUnitNo);
        }

        private void UpdatePollingUnitSummary(int thaiYaer, string adm1Code, int pollingUnitNo)
        {
            txtPollingUnitCount.Text = "-";
            txtRightCount.Text = "-";
            txtExerciseCount.Text = "-";

            if (string.IsNullOrWhiteSpace(adm1Code) || pollingUnitNo <= 0)
                return;

            var unitSum = MPDStatVoter.Get(thaiYaer, adm1Code, pollingUnitNo).Value();
            if (null == unitSum) return;

            txtPollingUnitCount.Text = unitSum.PollingUnitCount.ToString("n0");
            txtRightCount.Text = unitSum.RightCount.ToString("n0");
            txtExerciseCount.Text = unitSum.ExerciseCount.ToString("n0");

            if (null != _generalSummary)
            {
                _generalSummary.PollingUnitCount = unitSum.PollingUnitCount;
                _generalSummary.RightCount = unitSum.RightCount;
                _generalSummary.ExerciseCount = unitSum.ExerciseCount;
            }
        }

        private void GotoPrintPreview()
        {
            if (_parent == null) return;

            // prepare report item.
            MPDPrintVoteSummary2 item = new MPDPrintVoteSummary2();
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

                    // Candidate
                    int prevThaiYear = 2562;
                    if (null != _generalSummary.CandidateNo1)
                    {
                        var candidate = _generalSummary.CandidateNo1;
                        item.CandidateImage = (null != candidate.PersonImageData) ? candidate.PersonImageData : Defaults.PersonBuffer;
                        item.CandidateFullName = candidate.FullName;
                        item.CandidateSubGroup = candidate.SubGroup;
                        item.CandidateRemark = candidate.Remark;

                        var prevYearInfo = MPDVoteSummary.Get(prevThaiYear, candidate.FullName).Value();
                        item.CandidatePrevYear = "2562"; // Hardcode Thai Year
                        if (null != prevYearInfo)
                        {
                            item.CandidatePrevVote = prevYearInfo.VoteCount.ToString("n0");
                            item.CandidatePrevStatus = (prevYearInfo.RowNo == 1) ?
                                "ผู้ชนะการเลือกตั้ง" : "ได้คะแนนเป็นลำดับที่ " + prevYearInfo.RowNo.ToString("n0");
                        }
                        else
                        {
                            item.CandidatePrevVote = "-";
                            item.CandidatePrevStatus = "-";
                        }

                    }

                    // General
                    item.PrevVoteYear = prevThaiYear;
                    item.PollingUnitCount = _generalSummary.PollingUnitCount;
                    item.RightCount = _generalSummary.RightCount;
                    item.ExerciseCount = _generalSummary.ExerciseCount;
                    if (item.RightCount != 0)
                    {
                        item.ExercisePercent = Math.Round(
                            Convert.ToDecimal((double)((double)item.ExerciseCount / (double)item.RightCount) * (double)100), 2);
                    }
                    else
                    {
                        item.ExercisePercent = decimal.Zero;
                    }

                    item.DifferenceVoteFromNo2 = (null != _generalSummary.Top6[0] && null != _generalSummary.Top6[1]) ?
                        _generalSummary.Top6[0].VoteCount - _generalSummary.Top6[1].VoteCount : 0;
                    item.VoteCount7toLast = _generalSummary.VoteCount7toLast;
                }
            }
            _parent.GotoMPD2562PrintPreview(item);
        }

        #endregion

        #region Public Methods

        public void Setup(MPDMainSummaryPage parent, PollingUnitMenuItem value)
        {
            _parent = parent;
            LoadSummary(value);
        }

        #endregion
    }
}
