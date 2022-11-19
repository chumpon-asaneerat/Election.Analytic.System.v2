#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
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

        public int ErrNum { get; set; }
        public string ErrMsg { get; set; }
        public bool HasError
        {
            get { return ErrNum != 0; }
            set { }
        }

        public int PageNo { get; set; }
        public int RowsPerPage { get; set; }
        public int TotalRecords { get; set; }
        public int MaxPage { get; set; }

        #endregion
    }

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

        public T Value { get; set; }

        #endregion
    }
}
