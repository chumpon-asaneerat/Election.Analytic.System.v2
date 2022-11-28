﻿#region Using

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

            #region MPD 2562 Vote, User Stat

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

            #region MPD 2562 User Stat (350 units)

            private static MPD2562x350UnitSummaryManagePage _MPD2562x350UnitSummaryManage;

            /// <summary>Gets MPD2562 350 Unit Summary Manage Page.</summary>
            public static MPD2562x350UnitSummaryManagePage MPD2562x350UnitSummaryManage
            {
                get
                {
                    if (null == _MPD2562x350UnitSummaryManage)
                    {
                        lock (typeof(PPRPApp))
                        {
                            _MPD2562x350UnitSummaryManage = new MPD2562x350UnitSummaryManagePage();
                        }
                    }
                    return _MPD2562x350UnitSummaryManage;
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
        }
    }
}