#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

using System.Windows.Media;

using NLib;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    #region MGender

    /// <summary>
    /// The MGender class.
    /// </summary>
    public class MGender : NInpc
    {
        #region Internal Variables

        private int _GenderId = 0;
        private string _Description = string.Empty;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MGender() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MGender()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets GenderId.
        /// </summary>
        public int GenderId
        {
            get { return _GenderId; }
            set
            {
                if (_GenderId != value)
                {
                    _GenderId = value;
                    // Raise Event
                    Raise(() => GenderId);
                }
            }
        }
        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    // Raise Event
                    Raise(() => Description);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <returns>Returns list of MGender instance.</returns>
        public static NDbResult<List<MGender>> Gets()
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MGender>> rets = new NDbResult<List<MGender>>();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                // Set error number/message
                rets.ErrNum = 8000;
                rets.ErrMsg = msg;

                return rets;
            }

            var p = new DynamicParameters();

            try
            {
                rets.Value = cnn.Query<MGender>("GetMGenders", p,
                    commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                rets.ErrNum = 9999;
                rets.ErrMsg = ex.Message;
            }

            if (null == rets.Value)
            {
                // create empty list.
                rets.Value = new List<MGender>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
