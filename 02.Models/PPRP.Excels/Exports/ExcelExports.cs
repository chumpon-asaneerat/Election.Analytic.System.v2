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
    public class ExcelExport : ExcelModel
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

        #region Public Methods


        #endregion

    }

    #endregion
}
