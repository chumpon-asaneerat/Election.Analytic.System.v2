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
}
