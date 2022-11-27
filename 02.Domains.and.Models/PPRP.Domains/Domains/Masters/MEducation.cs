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
    #region MEducation

    /// <summary>
    /// The MEducation class.
    /// </summary>
    public class MEducation : NInpc
    {
        #region Internal Variables

        private int _EducationId = 0;
        private string _Description = string.Empty;
        private int _SortOrder = 0;
        private int _Active = 0;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MEducation() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MEducation()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets EducationId.
        /// </summary>
        public int EducationId
        {
            get { return _EducationId; }
            set
            {
                if (_EducationId != value)
                {
                    _EducationId = value;
                    // Raise Event
                    Raise(() => EducationId);
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
        /// <returns>Returns list of MOccupation instance.</returns>
        public static NDbResult<List<MOccupation>> Gets(int active = 1)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MOccupation>> rets = new NDbResult<List<MOccupation>>();

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
                rets.Value = cnn.Query<MOccupation>("GetMEducations", p,
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
                rets.Value = new List<MOccupation>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
