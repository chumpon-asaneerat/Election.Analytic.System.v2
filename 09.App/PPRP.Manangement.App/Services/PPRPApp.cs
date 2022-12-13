#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PPRP.Pages;

#endregion

namespace PPRP
{
    /// <summary>
    /// The PPRPApp class.
    /// </summary>
    public static class PPRPApp
    {
        /// <summary>
        /// Variables Static class.
        /// </summary>
        public static class Variables
        {
            /*
            /// <summary>Chief Revenue Entry Prerender DateTime.</summary>
            public static DateTime ChiefRevenueLastRenderTime = DateTime.MinValue;
            /// <summary>Collector Revenue Entry Prerender DateTime.</summary>
            public static DateTime CollectorRevenueLastRenderTime = DateTime.MinValue;
            */
        }
        /// <summary>
        /// Permissions Static class.
        /// </summary>
        public static class Permissions
        {
        }
        /// <summary>
        /// Pages Static class.
        /// </summary>
        public static class Pages
        {
            #region Main Menu

            private static MainMenuPage _MainMenu;

            /// <summary>Gets Main Menu Page.</summary>
            public static MainMenuPage MainMenu
            {
                get
                {
                    if (null == _MainMenu)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MainMenu = new MainMenuPage();
                        }
                    }
                    return _MainMenu;
                }
            }

            #endregion

            #region SignIn

            private static SignInPage _SignIn;

            /// <summary>Gets Sign In Page.</summary>
            public static SignInPage SignIn
            {
                get
                {
                    if (null == _SignIn)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _SignIn = new SignInPage();
                        }
                    }
                    return _SignIn;
                }
            }

            #endregion

            #region Polling Unit 2562

            private static MPD2562PollingUnitManagePage _MPD2562PollingUnitManage;

            /// <summary>Gets MPD2562 Polling Unit Manage Page.</summary>
            public static MPD2562PollingUnitManagePage MPD2562PollingUnitManage
            {
                get
                {
                    if (null == _MPD2562PollingUnitManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562PollingUnitManage = new MPD2562PollingUnitManagePage();
                        }
                    }
                    return _MPD2562PollingUnitManage;
                }
            }

            #endregion

            #region Polling Unit 2566

            private static MPD2566PollingUnitManagePage _MPD2566PollingUnitManage;

            /// <summary>Gets MPD2562 Polling Unit Manage Page.</summary>
            public static MPD2566PollingUnitManagePage MPD2566PollingUnitManage
            {
                get
                {
                    if (null == _MPD2566PollingUnitManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2566PollingUnitManage = new MPD2566PollingUnitManagePage();
                        }
                    }
                    return _MPD2566PollingUnitManage;
                }
            }

            #endregion

            #region MPD 2562 Vote Summary

            private static MPD2562VoteSummaryManagePage _MPD2562VoteSummaryManage;

            /// <summary>Gets MPD 2562 VoteSummary Manage Page.</summary>
            public static MPD2562VoteSummaryManagePage MPD2562VoteSummaryManage
            {
                get
                {
                    if (null == _MPD2562VoteSummaryManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562VoteSummaryManage = new MPD2562VoteSummaryManagePage();
                        }
                    }
                    return _MPD2562VoteSummaryManage;
                }
            }

            #endregion

            #region MPD 2562 Stat Voters

            private static MPD2562StatVoterManagePage _MPD2562StatVoterManagePage;

            /// <summary>Gets MPD 2562 Stat Voters Manage Page.</summary>
            public static MPD2562StatVoterManagePage MPD2562StatVoterManage
            {
                get
                {
                    if (null == _MPD2562StatVoterManagePage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562StatVoterManagePage = new MPD2562StatVoterManagePage();
                        }
                    }
                    return _MPD2562StatVoterManagePage;
                }
            }

            #endregion

            #region MPDC 2566

            private static MPDC2566ManagePage _MPDC2566Manage;

            /// <summary>Gets MPDC 2566 Manage Page.</summary>
            public static MPDC2566ManagePage MPDC2566Manage
            {
                get
                {
                    if (null == _MPDC2566Manage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPDC2566Manage = new MPDC2566ManagePage();
                        }
                    }
                    return _MPDC2566Manage;
                }
            }

            #endregion

            #region ADM1 - Province

            private static MProvinceManagePage _MProvinceManage;

            /// <summary>Gets MProvince Manage Page.</summary>
            public static MProvinceManagePage MProvinceManage
            {
                get
                {
                    if (null == _MProvinceManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MProvinceManage = new MProvinceManagePage();
                        }
                    }
                    return _MProvinceManage;
                }
            }

            #endregion

            #region ADM2 - District

            private static MDistrictManagePage _MDistrictManage;

            /// <summary>Gets MDistrict Manage Page.</summary>
            public static MDistrictManagePage MDistrictManage
            {
                get
                {
                    if (null == _MDistrictManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MDistrictManage = new MDistrictManagePage();
                        }
                    }
                    return _MDistrictManage;
                }
            }

            #endregion

            #region ADM3 - Subdistrict

            private static MSubdistrictManagePage _MSubdistrictManage;

            /// <summary>Gets MSubdistrict Manage Page.</summary>
            public static MSubdistrictManagePage MSubdistrictManage
            {
                get
                {
                    if (null == _MSubdistrictManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MSubdistrictManage = new MSubdistrictManagePage();
                        }
                    }
                    return _MSubdistrictManage;
                }
            }

            #endregion

            #region ADMPak - Pak

            private static ADMPakManagePage _ADMPakManage;

            /// <summary>Gets MSubdistrict Manage Page.</summary>
            public static ADMPakManagePage ADMPakManage
            {
                get
                {
                    if (null == _ADMPakManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _ADMPakManage = new ADMPakManagePage();
                        }
                    }
                    return _ADMPakManage;
                }
            }

            #endregion

            #region Party Image

            private static PartyManagePage _PartyManage;

            /// <summary>Gets Party Manage Page.</summary>
            public static PartyManagePage PartyManage
            {
                get
                {
                    if (null == _PartyManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _PartyManage = new PartyManagePage();
                        }
                    }
                    return _PartyManage;
                }
            }

            #endregion

            #region Person Image

            private static PersonImageManagePage _PersonImageManage;

            /// <summary>Gets Party Manage Page.</summary>
            public static PersonImageManagePage PersonImageManage
            {
                get
                {
                    if (null == _PersonImageManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _PersonImageManage = new PersonImageManagePage();
                        }
                    }
                    return _PersonImageManage;
                }
            }

            #endregion

            #region MPD 2562 Preview Vote Summary

            private static MPD2562PreviewVoteSummaryPage _MPD2562PreviewVoteSummary;

            /// <summary>Gets MPD 2562 Preview Vote Summary Page.</summary>
            public static MPD2562PreviewVoteSummaryPage MPD2562PreviewVoteSummary
            {
                get
                {
                    if (null == _MPD2562PreviewVoteSummary)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562PreviewVoteSummary = new MPD2562PreviewVoteSummaryPage();
                        }
                    }
                    return _MPD2562PreviewVoteSummary;
                }
            }

            #endregion

            #region MPD 2562 Preview Vote Stat Summary

            private static MPD2562PreviewStatVoterSummaryPage _MPD2562PreviewStatVoterSummary;

            /// <summary>Gets MPD 2562 Vote Stat Summary Preview Page.</summary>
            public static MPD2562PreviewStatVoterSummaryPage MPD2562PreviewStatVoterSummary
            {
                get
                {
                    if (null == _MPD2562PreviewStatVoterSummary)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562PreviewStatVoterSummary = new MPD2562PreviewStatVoterSummaryPage();
                        }
                    }
                    return _MPD2562PreviewStatVoterSummary;
                }
            }

            #endregion

            #region Shape Maps (Create)

            private static ImportShapeMapsManangePage _ImportShapeMapsManange;

            /// <summary>Gets Import ShapeMaps Manage Page.</summary>
            public static ImportShapeMapsManangePage ImportShapeMapsManange
            {
                get
                {
                    if (null == _ImportShapeMapsManange)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _ImportShapeMapsManange = new ImportShapeMapsManangePage();
                        }
                    }
                    return _ImportShapeMapsManange;
                }
            }

            #endregion

            #region Shape Maps (View)

            private static ShapeMapViewPage _ShapeMapView;

            /// <summary>Gets ShapeMaps View Page.</summary>
            public static ShapeMapViewPage ShapeMapView
            {
                get
                {
                    if (null == _ShapeMapView)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _ShapeMapView = new ShapeMapViewPage();
                        }
                    }
                    return _ShapeMapView;
                }
            }

            #endregion
        }
        /// <summary>
        /// Windows Static class.
        /// </summary>
        public static class Windows
        {
            #region Application Main Window

            /// <summary>Gets Application Main Window.</summary>
            public static Window MainWindow { get { return Application.Current.MainWindow; } }

            #endregion

            #region MessageBox

            /// <summary>Gets MessageBox Window.</summary>
            public static PPRP.Windows.MessageBoxWindow MessageBox
            {
                get
                {
                    var ret = new PPRP.Windows.MessageBoxWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region MessageBoxOKCancel

            /// <summary>Gets MessageBoxOkCancel Window.</summary>
            public static PPRP.Windows.MessageBoxOKCancelWindow MessageBoxOKCancel
            {
                get
                {
                    var ret = new PPRP.Windows.MessageBoxOKCancelWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Common Progress Dialog

            /// <summary>Gets Common Progress Dialog Window.</summary>
            public static PPRP.Windows.ProgressWindow ProgressDialog
            {
                get
                {
                    var ret = new PPRP.Windows.ProgressWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Error Dialog

            /// <summary>Gets Import Error Dialog Window.</summary>
            public static PPRP.Windows.ImportReportWindow ImportReport
            {
                get
                {
                    var ret = new PPRP.Windows.ImportReportWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Polling Unit 2562

            /// <summary>Gets MPD2562 Polling Unit Summary Import Window.</summary>
            public static PPRP.Windows.ImportMPD2562PollingUnitWindow ImportMPD2562PollingUnit
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2562PollingUnitWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Polling Unit 2566

            /// <summary>Gets MPD2566 Polling Unit Summary Import Window.</summary>
            public static PPRP.Windows.ImportMPD2566PollingUnitWindow ImportMPD2566PollingUnit
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2566PollingUnitWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPD 2562 Vote Summary

            /// <summary>Gets MPD2562 Vote Summary Import Window.</summary>
            public static PPRP.Windows.ImportMPD2562VoteSummaryWindow ImportMPD2562VoteSummary
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2562VoteSummaryWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPD 2562 Stat Voter

            /// <summary>Gets MPD 2562 Stat Voter Import Window.</summary>
            public static PPRP.Windows.ImportMPD2562StatVoterWindow ImportMPD2562StatVoter
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPD2562StatVoterWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import MPDC 2566

            /// <summary>Gets MPDC 2566 Import Window.</summary>
            public static PPRP.Windows.ImportMPDC2566Window ImportMPDC2566
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMPDC2566Window();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import ADM1 - MProvince

            /// <summary>Gets MProvince Import Window.</summary>
            public static PPRP.Windows.ImportMProvinceWindow ImportMProvince
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMProvinceWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import ADM2 - MDistrict

            /// <summary>Gets MDistrict Import Window.</summary>
            public static PPRP.Windows.ImportMDistrictWindow ImportMDistrict
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMDistrictWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import ADM3 - MSubdistrict

            /// <summary>Gets MSubdistrict Import Window.</summary>
            public static PPRP.Windows.ImportMSubdistrictWindow ImportMSubdistrict
            {
                get
                {
                    var ret = new PPRP.Windows.ImportMSubdistrictWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import ADMPak - Pak

            /// <summary>Gets ADM Pak Import Window.</summary>
            public static PPRP.Windows.ImportADMPakWindow ImportADMPak
            {
                get
                {
                    var ret = new PPRP.Windows.ImportADMPakWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Party Image

            /// <summary>Gets Import Party Image Window.</summary>
            public static PPRP.Windows.ImportPartyImageWindow ImportPartyImage
            {
                get
                {
                    var ret = new PPRP.Windows.ImportPartyImageWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Import Person Image

            /// <summary>Gets Person Image Import Window.</summary>
            public static PPRP.Windows.ImportPersonImageWindow ImportPersonImage
            {
                get
                {
                    var ret = new PPRP.Windows.ImportPersonImageWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Party Editor

            /// <summary>Gets Party Editor Window.</summary>
            public static PPRP.Windows.PartyEditorWindow PartyEditor
            {
                get
                {
                    var ret = new PPRP.Windows.PartyEditorWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region Person Editor

            /// <summary>Gets Person Editor Window.</summary>
            public static PPRP.Windows.PersonEditorWindow PersonEditor
            {
                get
                {
                    var ret = new PPRP.Windows.PersonEditorWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion

            #region MPDC 2566 Editor

            /// <summary>Gets MPDC 2566 Editor Window.</summary>
            public static PPRP.Windows.MPDC2566EditorWindow MPDC2566Editor
            {
                get
                {
                    var ret = new PPRP.Windows.MPDC2566EditorWindow();
                    ret.Owner = Application.Current.MainWindow;
                    return ret;
                }
            }

            #endregion
        }
    }
}
