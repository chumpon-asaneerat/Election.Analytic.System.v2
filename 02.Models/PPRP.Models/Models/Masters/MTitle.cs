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
    #region MTitle

    /// <summary>
    /// The MTitle class.
    /// </summary>
    public class MTitle : NInpc
    {
        #region Internal Variables

        private int _TitleId = 0;
        private string _Description = string.Empty;
        private int? _GenderId = new int?();

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MTitle() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MTitle()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets TitleId.
        /// </summary>
        public int TitleId
        {
            get { return _TitleId; }
            set
            {
                if (_TitleId != value)
                {
                    _TitleId = value;
                    // Raise Event
                    Raise(() => TitleId);
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
        /// Gets or sets GenderId.
        /// </summary>
        public int? GenderId
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

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="description">The filter description.</param>
        /// <param name="genderId">The filter genderid.</param>
        /// <returns>Returns list of MTitle instance.</returns>
        public static NDbResult<List<MTitle>> Gets(string description = null, int? genderId = new int?())
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MTitle>> rets = new NDbResult<List<MTitle>>();

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

            p.Add("@description", description);
            p.Add("@genderId", genderId);

            try
            {
                rets.data = cnn.Query<MTitle>("GetMTitles", p,
                    commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                rets.ErrNum = 9999;
                rets.ErrMsg = ex.Message;
            }

            if (null == rets.data)
            {
                // create empty list.
                rets.data = new List<MTitle>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
