#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using System.Windows.Media;

using NLib;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    #region MPD2562x350UnitSummary

    public class MPD2562x350UnitSummary
    {
        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int PollingUnitCount { get; set; }
        public int RightCount { get; set; }
        public int ExerciseCount { get; set; }
        public int InvalidCount { get; set; }
        public int NoVoteCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2562x350UnitSummary>> Gets(string provinceName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

            NDbResult<List<MPD2562x350UnitSummary>> rets = new NDbResult<List<MPD2562x350UnitSummary>>();

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
            p.Add("@ProvinceName", sProvinceName);

            try
            {
                var items = cnn.Query<MPD2562x350UnitSummary>("GetMPD2562x350UnitSummaries", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPD2562x350UnitSummary>();
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
                rets.Value = new List<MPD2562x350UnitSummary>();
            }

            return rets;
        }

        public static NDbResult<MPD2562x350UnitSummary> Get(string provinceName, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<MPD2562x350UnitSummary> rets = new NDbResult<MPD2562x350UnitSummary>();

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
            p.Add("@ProvinceName", provinceName);
            p.Add("@PollingUnitNo", pollingUnitNo);

            try
            {
                var items = cnn.Query<MPD2562x350UnitSummary>("GetMPD2562x350UnitSummary", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.FirstOrDefault() : null;
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
                rets.Value = new MPD2562x350UnitSummary();
            }

            return rets;
        }

        public static NDbResult Save(MPD2562x350UnitSummary value)
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
            p.Add("@ProvinceName", value.ProvinceName);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@RightCount", value.RightCount);
            p.Add("@ExerciseCount", value.ExerciseCount);
            p.Add("@InvalidCount", value.InvalidCount);
            p.Add("@NoVoteCount", value.NoVoteCount);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMPD2562x350UnitSummary", p, commandType: CommandType.StoredProcedure);
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

        public static NDbResult Import(MPD2562x350UnitSummary value)
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
            p.Add("@ProvinceName", value.ProvinceName);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@RightCount", value.RightCount);
            p.Add("@ExerciseCount", value.ExerciseCount);
            p.Add("@InvalidCount", value.InvalidCount);
            p.Add("@NoVoteCount", value.NoVoteCount);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPD2562x350UnitSummary", p, commandType: CommandType.StoredProcedure);
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

    #endregion

    #region MPD2562x350PrintUnitSummary

    public class MPD2562x350PrintUnitSummary
    {
        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int PollingUnitCount { get; set; }
        public int RightCount { get; set; }

        public int ExerciseCount { get; set; }
        public decimal ExercisePercent
        {
            get
            {
                if (RightCount <= 0) return decimal.Zero;
                decimal val = Math.Round(Convert.ToDecimal((double)((double)ExerciseCount / (double)RightCount) * (double)100), 2);
                return val;
            }
            set { }
        }
        public int InvalidCount { get; set; }
        public decimal InvalidPercent
        {
            get
            {
                if (ExerciseCount <= 0) return decimal.Zero;
                decimal val = Math.Round(Convert.ToDecimal((double)((double)InvalidCount / (double)ExerciseCount) * (double)100), 2);
                return val;
            }
            set { }
        }
        public int NoVoteCount { get; set; }
        public decimal NoVotePercent
        {
            get
            {
                if (ExerciseCount <= 0) return decimal.Zero;
                decimal val = Math.Round(Convert.ToDecimal((double)((double)NoVoteCount / (double)ExerciseCount) * (double)100), 2);
                return val;
            }
            set { }
        }

        public string FullName { get; set; }
        public string PartyName { get; set; }
        public int VoteCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2562x350PrintUnitSummary>> Gets(string provinceName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

            NDbResult<List<MPD2562x350PrintUnitSummary>> rets = new NDbResult<List<MPD2562x350PrintUnitSummary>>();

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
            p.Add("@ProvinceName", sProvinceName);

            try
            {
                var items = cnn.Query<MPD2562x350PrintUnitSummary>("GetMPD2562x350UnitFullSummaries", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPD2562x350PrintUnitSummary>();
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
                rets.Value = new List<MPD2562x350PrintUnitSummary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
