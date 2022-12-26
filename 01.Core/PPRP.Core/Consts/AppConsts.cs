#region Using

using System;

#endregion

namespace PPRP
{
    public static class AppConsts
    {
        // common properties
        public static string Version = "1";
        public static string Minor = "1";

        public static class Application
        {
            public static class Analytic
            {
                public static string ApplicationName = @"PPRP Analytic Application";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "2300";
                public static DateTime LastUpdate = new DateTime(2022, 12, 26, 08, 30, 00);
            }

            public static class Management
            {
                public static string ApplicationName = @"PPRP Management Application";
                // common
                public static string Version = AppConsts.Version;
                public static string Minor = AppConsts.Minor;
                public static string Build = "2300";
                public static DateTime LastUpdate = new DateTime(2022, 12, 26, 08, 30, 00);
            }
        }
    }
}
