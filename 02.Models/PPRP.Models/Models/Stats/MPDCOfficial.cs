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
    #region MPDCOfficialImport

    /// <summary>
    /// The MPDCOfficialImport class.
    /// </summary>
    public class MPDCOfficialImport
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }
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
        /// Gets or sets FullName.
        /// </summary>
        [ExcelColumn("ชื่อผู้สมัคร", 3)]
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets SortOrder.
        /// </summary>
        [ExcelColumn("ลำดับ", 4)]
        public int SortOrder { get; set; }
        /// <summary>
        /// Gets or sets PartyName.
        /// </summary>
        [ExcelColumn("ชื่อพรรค", 5)]
        public string PartyName { get; set; }
        /// <summary>
        /// Gets or sets VoteCount.
        /// </summary>
        [ExcelColumn("ผลคะแนน", 6)]
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
        /// Import MPDCOfficial.
        /// </summary>
        /// <param name="value">The MPDCOfficialImport value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(MPDCOfficialImport value)
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
            p.Add("@PartyName", value.PartyName);
            p.Add("@FullName", value.FullName);
            p.Add("@RevoteNo", value.RevoteNo);
            p.Add("@VoteCount", value.VoteCount);
            p.Add("@SortOrder", value.SortOrder);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPDCOfficial", p, commandType: CommandType.StoredProcedure);
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

        #endregion
    }

    #endregion

    #region MPDCOfficial

    /// <summary>
    /// The MPDCOfficial class.
    /// </summary>
    public class MPDCOfficial
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }

        /// <summary>
        /// Gets or sets RegionName.
        /// </summary>
        [ExcelColumn("ภาค", 1)]
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
        [ExcelColumn("จังหวัด", 2)]
        public string ProvinceNameTH { get; set; }
        /// <summary>
        /// Gets or sets PollingUnitNo.
        /// </summary>
        [ExcelColumn("เขตเลือกตั้ง", 3)]
        public int PollingUnitNo { get; set; }
        /// <summary>
        /// Gets or sets SortOrder.
        /// </summary>
        [ExcelColumn("ลำดับ", 5)]
        public int SortOrder { get; set; }
        /// <summary>
        /// Gets or sets PartyName.
        /// </summary>
        [ExcelColumn("ชื่อพรรค", 6)]
        public string PartyName { get; set; }
        /// <summary>
        /// Gets or sets Person Id.
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [ExcelColumn("ชื่อผู้สมัคร", 4)]
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets VoteCount.
        /// </summary>
        [ExcelColumn("ผลคะแนน", 7)]
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

        public static NDbResult<List<MPDCOfficial>> Gets(
            int thaiYear,
            string regionId = null, string regionName = null,
            string provinceNameTH = null,
            string partyName = null,
            string fullName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPDCOfficial>> rets = new NDbResult<List<MPDCOfficial>>();

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
                var data = cnn.Query<MPDCOfficial>("GetMPDCOfficials", p,
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
                rets.data = new List<MPDCOfficial>();
            }

            return rets;
        }

        public static NDbResult<MPDCOfficial> Get(int thaiYear, string fullName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                sFullName = null;
            }

            NDbResult<MPDCOfficial> rets = new NDbResult<MPDCOfficial>();

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
            p.Add("@FullName", sFullName);

            try
            {
                var items = cnn.Query<MPDCOfficial>("GetMPDCOfficialByFullName", p,
                    commandType: CommandType.StoredProcedure);
                var data = (null != items) ? items.FirstOrDefault() : null;
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
        /// Delete all by year.
        /// </summary>
        /// <param name="thaiYear">The thai year.</param>
        /// <returns></returns>
        public static NDbResult DeleteAll(int thaiYear)
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

            try
            {
                string sql = "DELETE FROM MPDCOfficial WHERE ThaiYear = @ThaiYear";

                var p = new DynamicParameters();
                p.Add("@ThaiYear", thaiYear);

                cnn.Execute(sql, p, commandType: CommandType.Text);
                ret.Success();
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
}
