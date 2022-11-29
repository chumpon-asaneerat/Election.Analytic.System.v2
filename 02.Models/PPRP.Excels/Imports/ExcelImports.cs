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
    #region ExcelImport

    /// <summary>
    /// The ExcelImport class.
    /// </summary>
    /// <typeparam name="T">The target item iype paramter.</typeparam>
    public class ExcelImport<T> : ExcelModel<T>
        where T: class
    {
        #region Constructor (Static)

        static ExcelImport() 
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelImport() : base() { }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ExcelImport() { }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// Open File.
        /// </summary>
        /// <returns>Returns true if file selected</returns>
        public bool Open() 
        {
            bool ret = false;

            string file = Dialogs.OpenDialog();
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
