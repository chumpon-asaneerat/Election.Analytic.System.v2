#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#endregion

namespace PPRP.Excel.Utils
{
    public class Dialogs
    {
        #region Show Open Excel Dialog

        /// <summary>
        /// Show Open Excel File Dialog.
        /// </summary>
        /// <param name="title">The Dialog Title.</param>
        /// <param name="initDir">The initial directory path.</param>
        /// <returns>Returns FileName if user choose file otherwise return null.</returns>
        public static string OpenDialog(string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null)
        {
            return OpenDialog(null, title, initDir);
        }
        /// <summary>
        /// Show Open Excel File Dialog.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="title">The Dialog Title.</param>
        /// <param name="initDir">The initial directory path.</param>
        /// <returns>Returns FileName if user choose file otherwise return null.</returns>
        public static string OpenDialog(Window owner,
            string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null)
        {
            string fileName = null;

            // setup dialog options
            var od = new Microsoft.Win32.OpenFileDialog();
            od.Multiselect = false;
            od.InitialDirectory = initDir;
            od.Title = string.IsNullOrEmpty(title) ? "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล" : title;
            od.Filter = "Excel Files(*.xls, *.xlsx)|*.xls;*.xlsx";

            var ret = od.ShowDialog(owner) == true;
            if (ret)
            {
                // assigned to FileName
                fileName = od.FileName;
            }
            od = null;

            return fileName;
        }

        #endregion

        #region Show Save Excel Dialog

        /// <summary>
        /// Show Save Excel File Dialog.
        /// </summary>
        /// <param name="defaultFileName">The Default File Name.</param>
        /// <returns>Returns FileName if user choose file otherwise return null.</returns>
        public static string SaveDialog(string defaultFileName)
        {
            return SaveDialog(null, null, "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล", defaultFileName);
        }
        /// <summary>
        /// Show Save Excel File Dialog.
        /// </summary>
        /// <param name="title">The Dialog Title.</param>
        /// <param name="initDir">The initial directory path.</param>
        /// <returns>Returns FileName if user choose file otherwise return null.</returns>
        public static string ShowDialog(string title = "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล",
            string initDir = null)
        {
            return SaveDialog(null, title, initDir);
        }
        /// <summary>
        /// Show Save Excel File Dialog.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="title">The Dialog Title.</param>
        /// <param name="initDir">The initial directory path.</param>
        /// <param name="defaultFileName">The Default File Name.</param>
        /// <returns>Returns FileName if user choose file otherwise return null.</returns>
        public static string SaveDialog(Window owner,
            string title = "กรุณาเลือก excel file ที่ต้องการนำเข้าข้อมูล",
            string initDir = null,
            string defaultFileName = "")
        {
            string fileName = null;

            // setup dialog options
            var sd = new Microsoft.Win32.SaveFileDialog();
            sd.InitialDirectory = initDir;
            sd.Title = string.IsNullOrEmpty(title) ? "กรุณาระบุขื่อ excel file ที่ต้องการนำส่งออกข้อมูล" : title;
            sd.Filter = "Excel Files(*.xls, *.xlsx)|*.xls;*.xlsx";
            sd.FileName = defaultFileName;
            var ret = sd.ShowDialog(owner) == true;
            if (ret)
            {
                // assigned to FileName
                fileName = sd.FileName;
            }
            sd = null;

            return fileName;
        }

        #endregion
    }
}
