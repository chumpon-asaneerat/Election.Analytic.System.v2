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
    public class MRegion
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MRegion() : base()
        {

        }

        #endregion

        #region Public Properties

        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string GeoGroup { get; set; }
        public string GeoSubGroup { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MRegion>> Gets(string regionId = null, 
            string regionName = null, string geoGroup = null, string geoSubGroup = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MRegion>> rets = new NDbResult<List<MRegion>>();

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
            p.Add("@RegionId", regionId);
            p.Add("@RegionName", regionName);
            p.Add("@GeoGroup", geoGroup);
            p.Add("@GeoSubGroup", geoSubGroup);

            try
            {
                rets.Value = cnn.Query<MRegion>("GetMRegions", p,
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
                rets.Value = new List<MRegion>();
            }

            return rets;
        }

        public static NDbResult Save(MRegion value)
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
            p.Add("@RegionId", value.RegionId);
            p.Add("@RegionName", value.RegionName);
            p.Add("@GeoGroup", value.GeoGroup);
            p.Add("@GeoSubGroup", value.GeoSubGroup);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMRegion", p, commandType: CommandType.StoredProcedure);
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
