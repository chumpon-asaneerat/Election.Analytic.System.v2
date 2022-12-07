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
    public class MPDCImport : NInpc
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }
        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        [ExcelColumn("จังหวัด", 1)]
        public string ProvinceNameTH { get; set; }
        /// <summary>
        /// Gets or sets PollingUnitNo.
        /// </summary>
        [ExcelColumn("เขตเลือกตั้ง", 2)]
        public int PollingUnitNo { get; set; }
        /// <summary>
        /// Gets or sets CandidateNo.
        /// </summary>
        [ExcelColumn("ลำดับที่", 3)]
        public int CandidateNo { get; set; }
        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [ExcelColumn("ชื่อผู้สมัคร", 4)]
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets Prev PartyName.
        /// </summary>
        [ExcelColumn("สังกัดพรรคเดิม", 5)]
        public string PrevPartyName { get; set; }
        /// <summary>
        /// Gets or sets Education Level.
        /// </summary>
        [ExcelColumn("ระดับการศึกษา", 6)]
        public string EducationLevel { get; set; }
        /// <summary>
        /// Gets or sets SubGroup.
        /// </summary>
        [ExcelColumn("ผู้แนะนำ", 7)]
        public string SubGroup { get; set; }
        /// <summary>
        /// Gets or sets Remark.
        /// </summary>
        [ExcelColumn("หมายเหตุ", 8)]
        public string Remark { get; set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Import.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static NDbResult Import(MPDCImport value)
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
            p.Add("@ThaiYear", value.ThaiYear);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@CandidateNo", value.CandidateNo);
            p.Add("@FullName", value.FullName);
            p.Add("@PrevPartyName", value.PrevPartyName);
            p.Add("@Remark", value.Remark);
            p.Add("@SubGroup", value.SubGroup);
            p.Add("@EducationLevel", value.EducationLevel);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPDC", p, commandType: CommandType.StoredProcedure);
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


    public class MPDCPollingUnit : NInpc
    {

    }

    public class MPDC : NInpc
    {
    }
}
