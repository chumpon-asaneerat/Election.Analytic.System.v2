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

namespace PPRP.Models
{
    #region MAge

    /// <summary>
    /// The MAge class.
    /// </summary>
    public class MAge : NInpc
    {
        #region Internal Variables

        private int _AgeId = 0;
        private int _AgeMin = 0;
        private int _AgeMax = 150;
        private string _Description = string.Empty;
        private int _SortOrder = 0;
        private int _Active = 0;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MAge() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MAge()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets AgeId.
        /// </summary>
        public int AgeId
        {
            get { return _AgeId; }
            set
            {
                if (_AgeId != value)
                {
                    _AgeId = value;
                    // Raise Event
                    Raise(() => AgeId);
                }
            }
        }
        /// <summary>
        /// Gets or sets min age range.
        /// </summary>
        public int AgeMin
        {
            get { return _AgeMin; }
            set
            {
                if (_AgeMin != value)
                {
                    _AgeMin = value;
                    // Raise Event
                    Raise(() => AgeMin);
                }
            }
        }
        /// <summary>
        /// Gets or sets max age range.
        /// </summary>
        public int AgeMax
        {
            get { return _AgeMax; }
            set
            {
                if (_AgeMax != value)
                {
                    _AgeMax = value;
                    // Raise Event
                    Raise(() => AgeMax);
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
        /// <summary>
        /// Gets or sets sort order.
        /// </summary>
        public int SortOrder
        {
            get { return _SortOrder; }
            set
            {
                if (_SortOrder != value)
                {
                    _SortOrder = value;
                    // Raise Event
                    Raise(() => SortOrder);
                }
            }
        }
        /// <summary>
        /// Gets or sets Active status.
        /// </summary>
        public int Active
        {
            get { return _Active; }
            set
            {
                if (_Active != value)
                {
                    _Active = value;
                    // Raise Event
                    Raise(() => Active);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="active">The filter active status. Default is 1.</param>
        /// <returns>Returns list of MAge instance.</returns>
        public static NDbResult<List<MAge>> Gets(int active = 1)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MAge>> rets = new NDbResult<List<MAge>>();

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

            p.Add("@active", active);

            try
            {
                rets.Value = cnn.Query<MAge>("GetMAges", p,
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
                rets.Value = new List<MAge>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
