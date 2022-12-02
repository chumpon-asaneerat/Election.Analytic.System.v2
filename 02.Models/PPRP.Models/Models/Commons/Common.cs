#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Models
{
    #region NDbResult

    /// <summary>
    /// The NDbResult class.
    /// </summary>
    public class NDbResult
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base()
        {
            ErrNum = 0;
            ErrMsg = string.Empty;
            PageNo = 1;
            RowsPerPage = 0;
            TotalRecords = 0;
            MaxPage = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Err Number.
        /// </summary>
        public int ErrNum { get; set; }
        /// <summary>
        /// Gets or sets Err Message.
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// Checks Has Error.
        /// </summary>
        public bool HasError
        {
            get { return ErrNum != 0; }
            set { }
        }

        /// <summary>
        /// Gets or sets Page No.
        /// </summary>
        public int PageNo { get; set; }
        /// <summary>
        /// Gets or sets Row Per Page.
        /// </summary>
        public int RowsPerPage { get; set; }
        /// <summary>
        /// Gets or sets Max Page.
        /// </summary>
        public int MaxPage { get; set; }
        /// <summary>
        /// Gets or sets Total Records.
        /// </summary>
        public int TotalRecords { get; set; }

        #endregion
    }

    #endregion

    #region NDbResult<T>

    /// <summary>
    /// The NDbResult (Generic) class.
    /// </summary>
    /// <typeparam name="T">The Target Result Type</typeparam>
    public class NDbResult<T> : NDbResult
        where T: class, new()
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NDbResult() : base() 
        {
            Value = default;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or set Result Value.
        /// </summary>
        public T Value { get; set; }

        #endregion
    }

    #endregion

    #region ImportError

    /// <summary>
    /// The ImportError class.
    /// </summary>
    public class ImportError
    {
        #region Public Properties

        /// <summary>
        /// Gets or set RowNo.
        /// </summary>
        public int RowNo { get; set; }
        /// <summary>
        /// Gets or set ErrMsg.
        /// </summary>
        public string ErrMsg { get; set; }
        /// <summary>
        /// Gets or set DataString.
        /// </summary>
        public string DataString { get; set; }

        #endregion
    }

    #endregion
}
