#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;

#endregion

namespace PPRP.Models
{
    public class ExcelImport
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

        public void Map() { }

        #endregion
    }
}
