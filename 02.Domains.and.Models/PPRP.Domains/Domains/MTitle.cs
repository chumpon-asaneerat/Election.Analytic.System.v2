#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

using NLib;
using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    public class MTitle
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MTitle() : base()
        {

        }

        #endregion

        #region Public Properties

        public int TitleId { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public string GenderName { get; set; }
        public int GenderId { get; set; }
        public int DLen { get; set; }
        public int SLen { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MTitle>> Gets(string desc = "", 
            string shortName = "", int? genderId = new int?())
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
            p.Add("@description", desc);
            p.Add("@shortname", shortName);
            p.Add("@genderid", genderId);

            try
            {
                rets.Value = cnn.Query<MTitle>("GetMTitles", p,
                    commandType: CommandType.StoredProcedure).AsList();
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
                rets.Value = new List<MTitle>();
            }

            return rets;
        }

        public static NDbResult Save(MTitle value)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult ret = new NDbResult();

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                // Set error number/message
                ret.ErrNum = 8000;
                ret.ErrMsg = msg;

                return ret;
            }

            var p = new DynamicParameters();
            p.Add("@Description", value.Description);
            p.Add("@ShortName", value.ShortName);
            p.Add("@GenderId", value.GenderId);

            p.Add("@TitleId", dbType: DbType.Int32, direction: ParameterDirection.InputOutput);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMTitle", p, commandType: CommandType.StoredProcedure);

                value.TitleId = p.Get<int>("@TitleId");

                // Set error number/message
                ret.ErrNum = p.Get<int>("@errNum");
                ret.ErrMsg = p.Get<string>("@errMsg");
            }
            catch (Exception ex)
            {
                med.Err(ex);
                // Set error number/message
                ret.ErrNum = 9999;
                ret.ErrMsg = ex.Message;
            }
            return ret;
        }

        #endregion
    }
}
