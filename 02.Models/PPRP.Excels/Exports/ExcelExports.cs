#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;

#endregion

namespace PPRP.Models.Excel
{
    #region ExcelExport

    /// <summary>
    /// The ExcelExport class.
    /// </summary>
    /// <typeparam name="T">The target item iype paramter.</typeparam>
    public class ExcelExport<T> : ExcelModel<T>
        where T : class
    {
        #region Constructor (Static)

        static ExcelExport() 
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelExport() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ExcelExport() { }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Save File as.
        /// </summary>
        /// <returns>Returns true if file selected</returns>
        public bool SaveAs() 
        {
            bool ret = false;

            string file = Dialogs.SaveDialog();
            if (!string.IsNullOrWhiteSpace(file))
            {
                FileName = file;
            }

            return ret;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets File Name.
        /// </summary>
        public string FileName { get; protected set; }

        #endregion
    }

    #endregion
}
