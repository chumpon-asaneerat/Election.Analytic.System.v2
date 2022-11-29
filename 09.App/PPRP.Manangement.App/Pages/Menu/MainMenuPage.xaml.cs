#region Using

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

using NLib;
using NLib.Services;

#endregion

namespace PPRP.Pages
{
    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : UserControl
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainMenuPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded/Unloaded

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Menu Handlers

        #region Group 0 - การจัดการข้อมูลการเลือกตั้ง

        private void mnuPollingUnit2562Manage_Click(object sender, RoutedEventArgs e)
        {
            GotoPullingUnit2562Manage();
        }

        private void mnuPollingUnit2566Manage_Click(object sender, RoutedEventArgs e)
        {
            GotoPullingUnit2566Manage();
        }

        private void mnuMPD2562VoteSummaryManage_Click(object sender, RoutedEventArgs e)
        {
            GotoMPD2562VoteSummaryManage();
        }

        private void mnuMPD2562UserStatSummaryManage_Click(object sender, RoutedEventArgs e)
        {
            GotoMPD2562UserStatSummaryManage();
        }

        private void mnuMPDC2566Manage_Click(object sender, RoutedEventArgs e)
        {
            GotoMPDC2566Manage();
        }

        #endregion

        #region Group 2 - ข้อมูลทางภูมิศาสตร์

        private void mnuADM1Manage_Click(object sender, RoutedEventArgs e)
        {
            GotoADM1Manage();
        }

        private void mnuADM2Manage_Click(object sender, RoutedEventArgs e)
        {
            GotoADM2Manage();
        }

        private void mnuADM3Manage_Click(object sender, RoutedEventArgs e)
        {
            GotoADM3Manage();
        }

        private void mnuADMPakManage_Click(object sender, RoutedEventArgs e)
        {
            GotoADMPakManage();
        }

        #endregion

        #region Group 4 - ข้อมูลหลัก

        private void mnuPartyImageManage_Click(object sender, RoutedEventArgs e)
        {
            GotoPartyImageManage();
        }

        private void mnuPersonImageManage_Click(object sender, RoutedEventArgs e)
        {
            GotoPersonImageManage();
        }

        #endregion

        #endregion

        #region Private Methods

        #region MPD Pulling Unit, MPD 2562 (Vote, User Stat)  ,MPDC 2566

        private void GotoPullingUnit2562Manage()
        {
            // ข้อมูลหน่วยเลือกตั้งแบบแบ่งเขต ปี 2562 - MPD Polling Unit summary.
            var page = PPRPApp.Pages.MPD2562PollingUnitManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPullingUnit2566Manage()
        {
            // ข้อมูลหน่วยเลือกตั้งแบบแบ่งเขต ปี 2566 - MPD Polling Unit summary.
            var page = PPRPApp.Pages.MPD2566PollingUnitManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPD2562VoteSummaryManage()
        {
            // ข้อมูลผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทน แบบแบ่งเขต ปี 2562 - MPD (summary)
            var page = PPRPApp.Pages.MPD2562VoteSummaryManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPD2562UserStatSummaryManage()
        {
            // ข้อมูลผู้ใช้สิทธิ 350 เขต ปี 2562 - MPD 350 Unit(summary)
            var page = PPRPApp.Pages.MPD2562x350UnitSummaryManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoMPDC2566Manage()
        {
            // ข้อมูลว่าที่ผู้สมัครรับเลือกตั้งสมาชิกสภาผู้แทน แบบแบ่งเขต ปี 2566 - MPD (candidate)
            var page = PPRPApp.Pages.MPDC2566Manage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region ADM1-ADM3, ADMPak Import

        private void GotoADM1Manage()
        {
            // ข้อมูลจังหวัด (MProvince-ADM1)
            var page = PPRPApp.Pages.MProvinceManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoADM2Manage()
        {
            // ข้อมูลอำเภอ (MDistrict-ADM2)
            var page = PPRPApp.Pages.MDistrictManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoADM3Manage()
        {
            // ข้อมูลตำบล (MSubdistrict-ADM3)
            var page = PPRPApp.Pages.MSubdistrictManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoADMPakManage()
        {
            // ข้อมูลภาค (ADM Pak)
            var page = PPRPApp.Pages.ADMPakManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #region Party/Person Image

        private void GotoPartyImageManage()
        {
            // ข้อมูลพรรคการเมือง
            var page = PPRPApp.Pages.PartyManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        private void GotoPersonImageManage()
        {
            // ข้อมูลรูปผู้สมัคร-ว่าที่ผู้สมัคร
            var page = PPRPApp.Pages.PersonImageManage;
            page.Setup();
            PageContentManager.Instance.Current = page;
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Setup.
        /// </summary>
        public void Setup() 
        {

        }

        #endregion
    }
}
