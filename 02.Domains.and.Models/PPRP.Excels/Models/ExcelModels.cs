#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;

#endregion

namespace PPRP.Models.Excel
{
    #region ExcelModel

    /// <summary>
    /// The Excel Model class.
    /// </summary>
    /// <typeparam name="T">The target item type.</typeparam>
    public class ExcelModel<T>
        where T : class
    {
        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExcelModel() : base() 
        {
            this.Columns = new List<NExcelColumn>();
            this.Items = new List<T>();
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~ExcelModel() 
        {
            // Free Columns.
            if (null != this.Columns)
            {
                lock (this)
                {
                    this.Columns.Clear();
                    this.Columns = null;
                }
            }
            // Free Items.
            if (null != this.Items)
            {
                lock (this)
                {
                    this.Items.Clear();
                    this.Items = null;
                }
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Excel Columns.
        /// </summary>
        public List<NExcelColumn> Columns { get; protected set; }

        /// <summary>
        /// Gets Items.
        /// </summary>
        public List<T> Items { get; protected set; }

        #endregion
    }

    #endregion

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
