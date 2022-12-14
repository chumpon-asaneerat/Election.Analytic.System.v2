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
    #region MPDStatVoter

    /// <summary>
    /// The MPDStatVoter class.
    /// </summary>
    public class MPDStatVoter
    {
        #region Public Properties

        #region ThaiYear

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }

        #endregion

        #region Region

        /// <summary>
        /// Gets or sets RegionId.
        /// </summary>
        public string RegionId { get; set; }
        /// <summary>
        /// Gets or sets RegionName.
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// Gets or sets GeoGroup.
        /// </summary>
        public string GeoGroup { get; set; }
        /// <summary>
        /// Gets or sets GeoSubGroup.
        /// </summary>
        public string GeoSubGroup { get; set; }

        #endregion

        #region Province

        /// <summary>
        /// Gets or sets ADM1Code.
        /// </summary>
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets ProvinceId.
        /// </summary>
        public string ProvinceId { get; set; }
        /// <summary>
        /// Gets or sets ProvinceNameEN.
        /// </summary>
        public string ProvinceNameEN { get; set; }

        #endregion

        #region Required for import

        /// <summary>
        /// Gets or sets ProvinceNameTH.
        /// </summary>
        [ExcelColumn("จังหวัด", 1)]
        public string ProvinceNameTH { get; set; }
        /// <summary>
        /// Gets or sets PollingUnitNo.
        /// </summary>
        [ExcelColumn("เขตเลือกตั้ง", 2)]
        public int PollingUnitNo { get; set; }
        /// <summary>
        /// Gets or sets RightCount.
        /// </summary>
        [ExcelColumn("ผู้มีสิทธิเลือกตั้ง", 3)]
        public int RightCount { get; set; }
        /// <summary>
        /// Gets or sets ExerciseCount.
        /// </summary>
        [ExcelColumn("ผู้ใช้สิทธิเลือกตั้ง", 4)]
        public int ExerciseCount { get; set; }
        /// <summary>
        /// Gets or sets InvalidCount.
        /// </summary>
        [ExcelColumn("บัตรเสีย", 5)]
        public int InvalidCount { get; set; }
        /// <summary>
        /// Gets or sets NoVoteCount.
        /// </summary>
        [ExcelColumn("บัตรไม่เลือกผู้สมัครผู้ใด", 6)]
        public int NoVoteCount { get; set; }

        #endregion

        #region Stats (Calc)

        /// <summary>
        /// Gets Exercise Percent.
        /// </summary>
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
        /// <summary>
        /// Gets Invalid Percent.
        /// </summary>
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
        /// <summary>
        /// Gets NoVote Percent.
        /// </summary>
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
        /// <summary>
        /// Gets or sets PollingUnitCount.
        /// </summary>
        public int PollingUnitCount { get; set; }

        #endregion

        #endregion

        #region Static Methods

        /// <summary>
        /// Import MPDStatVoterImport.
        /// </summary>
        /// <param name="value">The MPDStatVoterImport value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(MPDStatVoter value)
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

            if (null == value)
            {
                string msg = "Value is null.";
                med.Err(msg);
                // Set error number/message
                ret.ErrNum = 8000;
                ret.ErrMsg = msg;

                return ret;
            }

            var p = new DynamicParameters();
            p.Add("@ThaiYear", value.ThaiYear);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@RightCount", value.RightCount);
            p.Add("@ExerciseCount", value.ExerciseCount);
            p.Add("@InvalidCount", value.InvalidCount);
            p.Add("@NoVoteCount", value.NoVoteCount);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPDStatVoter", p, commandType: CommandType.StoredProcedure);
                ret.Success();
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
        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="thaiYear"></param>
        /// <param name="provinceNameTH"></param>
        /// <returns></returns>

        public static NDbResult<MPDStatVoter> Get(
            int thaiYear,
            string adm1Code = null,
            int pollingUnitNo = 0)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<MPDStatVoter> rets = new NDbResult<MPDStatVoter>();

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

            p.Add("@ThaiYear", thaiYear);
            p.Add("@ADM1Code", adm1Code);
            p.Add("@PollingUnitNo", pollingUnitNo);

            try
            {
                var data = cnn.Query<MPDStatVoter>("GetMPDStatVoterSummary", p,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
                rets.Success(data);
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
                rets.data = null;
            }

            return rets;
        }
        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="thaiYear"></param>
        /// <param name="provinceNameTH"></param>
        /// <returns></returns>

        public static NDbResult<List<MPDStatVoter>> Gets(
            int thaiYear,
            string provinceNameTH = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPDStatVoter>> rets = new NDbResult<List<MPDStatVoter>>();

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

            p.Add("@ThaiYear", thaiYear);
            p.Add("@ProvinceNameTH", provinceNameTH);

            try
            {
                var data = cnn.Query<MPDStatVoter>("GetMPDStatVoterSummaries", p,
                    commandType: CommandType.StoredProcedure).ToList();
                rets.Success(data);
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
                rets.data = new List<MPDStatVoter>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MPDStatVoterPrintSummary

    public class MPDStatVoterPrintSummary
    {
        #region Public Properties

        public string ProvinceNameTH { get; set; }
        public string ProvinceName { get { return ProvinceNameTH; } }
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

        public static NDbResult<List<MPDStatVoterPrintSummary>> Gets(int thaiYear, string provinceName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

            NDbResult<List<MPDStatVoterPrintSummary>> rets = new NDbResult<List<MPDStatVoterPrintSummary>>();

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
            p.Add("@ThaiYear", thaiYear);
            p.Add("@ProvinceNameTH", sProvinceName);

            try
            {
                var items = cnn.Query<MPDStatVoterPrintSummary>("GetMPDStatVoterSummaries", p,
                    commandType: CommandType.StoredProcedure);
                var results = (null != items) ? items.ToList() : new List<MPDStatVoterPrintSummary>();
                rets.Success(results);
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
                rets.data = new List<MPDStatVoterPrintSummary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
