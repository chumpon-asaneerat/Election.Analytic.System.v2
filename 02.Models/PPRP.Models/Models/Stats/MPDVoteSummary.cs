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
    #region MPDVoteSummaryImport


    /// <summary>
    /// The MPDVoteSummaryImport class.
    /// </summary>
    public class MPDVoteSummaryImport
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }
        /// <summary>
        /// Gets or sets ProvinceNameTH.
        /// </summary>
        [ExcelColumn("จังหวัด")]
        public string ProvinceNameTH { get; set; }
        /// <summary>
        /// Gets or sets PollingUnitNo.
        /// </summary>
        [ExcelColumn("เขตเลือกตั้ง")]
        public int PollingUnitNo { get; set; }
        /// <summary>
        /// Gets or sets CandidateNo.
        /// </summary>
        [ExcelColumn("หมายเลขผู้สมัคร")]
        public int CandidateNo { get; set; }
        /// <summary>
        /// Gets or sets PartyName.
        /// </summary>
        [ExcelColumn("ชื่อพรรค")]
        public string PartyName { get; set; }
        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [ExcelColumn("ชื่อผู้สมัคร")]
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets VoteCount.
        /// </summary>
        [ExcelColumn("ผลคะแนน")]
        public int VoteCount { get; set; }
        /// <summary>
        /// Gets or sets RevoteNo.
        /// </summary>
        public int RevoteNo { get; set; }
        /// <summary>
        /// Gets or sets RankNo.
        /// </summary>
        public int RankNo { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Import MPDVoteSummary.
        /// </summary>
        /// <param name="value">The MPDVoteSummaryImport value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(MPDVoteSummaryImport value)
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
            // checks
            if (value.VoteCount <= 0) value.VoteCount = 0;
            if (value.RevoteNo <= 0) value.RevoteNo = 0;

            var p = new DynamicParameters();
            p.Add("@ThaiYear", value.ThaiYear);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@CandidateNo", value.CandidateNo);
            p.Add("@PartyName", value.PartyName);
            p.Add("@FullName", value.FullName);
            p.Add("@VoteCount", value.VoteCount);
            p.Add("@RevoteNo", value.RevoteNo);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPDVoteSummary", p, commandType: CommandType.StoredProcedure);
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

    #region MPDVoteSummary

    /// <summary>
    /// The MPDVoteSummary class.
    /// </summary>
    public class MPDVoteSummary
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }

        /// <summary>
        /// Gets or sets RegionName.
        /// </summary>
        [ExcelColumn("ภาค")]
        public string RegionName { get; set; }
        /// <summary>
        /// Gets or sets GeoGroup.
        /// </summary>
        public string GeoGroup { get; set; }
        /// <summary>
        /// Gets or sets GeoSubGroup.
        /// </summary>
        public string GeoSubGroup { get; set; }

        /// <summary>
        /// Gets or sets ProvinceNameTH.
        /// </summary>
        [ExcelColumn("จังหวัด")]
        public string ProvinceNameTH { get; set; }
        /// <summary>
        /// Gets or sets PollingUnitNo.
        /// </summary>
        [ExcelColumn("เขตเลือกตั้ง")]
        public int PollingUnitNo { get; set; }
        /// <summary>
        /// Gets or sets CandidateNo.
        /// </summary>
        [ExcelColumn("หมายเลขผู้สมัคร")]
        public int CandidateNo { get; set; }
        /// <summary>
        /// Gets or sets PartyName.
        /// </summary>
        [ExcelColumn("ชื่อพรรค")]
        public string PartyName { get; set; }
        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [ExcelColumn("ชื่อผู้สมัคร")]
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets VoteCount.
        /// </summary>
        [ExcelColumn("ผลคะแนน")]
        public int VoteCount { get; set; }
        /// <summary>
        /// Gets or sets RowNo.
        /// </summary>
        public int RowNo { get; set; }
        /// <summary>
        /// Gets or sets RankNo.
        /// </summary>
        public int RankNo { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="thaiYear"></param>
        /// <param name="regionId"></param>
        /// <param name="regionName"></param>
        /// <param name="provinceNameTH"></param>
        /// <param name="partyName"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>

        public static NDbResult<List<MPDVoteSummary>> Gets(
            int thaiYear,
            string regionId = null, string regionName = null,
            string provinceNameTH = null,
            string partyName = null, 
            string fullName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPDVoteSummary>> rets = new NDbResult<List<MPDVoteSummary>>();

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
            p.Add("@RegionId", regionId);
            p.Add("@RegionName", regionName);
            p.Add("@ProvinceNameTH", provinceNameTH);
            p.Add("@PartyName", partyName);
            p.Add("@FullName", fullName);

            try
            {
                rets.Value = cnn.Query<MPDVoteSummary>("GetMPDVoteSummaries", p,
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
                rets.Value = new List<MPDVoteSummary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
