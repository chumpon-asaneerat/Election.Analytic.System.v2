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
    public class ExcelImport: ExcelModel
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

        #region Public Methods


        #endregion
    }

    #endregion
}
