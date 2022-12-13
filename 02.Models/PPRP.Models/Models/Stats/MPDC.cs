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
    #region MPDCImport

    /// <summary>
    /// The MPDCImport class.
    /// </summary>
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

    #endregion

    #region MPDCPollingUnit

    public class MPDCPollingUnit : NInpc
    {
        #region Internal Variables

        private bool _loaded = false;
        private List<MPDC> _items = null;

        #endregion

        #region Public Properties

        public int ThaiYear { get; set; }
        public string ProvinceNameTH { get; set; }
        public int PollingUnitNo { get; set; }
        public int TotalCandidates { get; set; }
        public string FullNameFilter { get; set; }
        public string GroupName
        {
            get { return string.Format("{0} เขต {1}", ProvinceNameTH, PollingUnitNo); }
            set { }
        }
        public List<MPDC> Items
        {
            get
            {
                if (!_loaded)
                {
                    if (TotalCandidates > 0)
                    {
                        _items = MPDC.Gets(ThaiYear, ProvinceNameTH, PollingUnitNo, FullNameFilter).Value();
                    }
                    _loaded = true;

                }
                return _items;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="thaiYear"></param>
        /// <param name="provinceName"></param>
        /// <param name="fullName"></param>
        /// <param name="pageNo"></param>
        /// <param name="pollingUnitPerPage"></param>
        /// <returns></returns>
        public static NDbResult<List<MPDCPollingUnit>> Gets(int thaiYear, string provinceName = null, 
            string fullName = null,
            int pageNo = 1, int pollingUnitPerPage = 4)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(sFullName))
            {
                sFullName = null;
            }

            NDbResult<List<MPDCPollingUnit>> rets = new NDbResult<List<MPDCPollingUnit>>();

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
            p.Add("@ProvinceName", sProvinceName);
            p.Add("@FullName", sFullName);

            p.Add("@pageNum", value: pageNo, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@rowsPerPage", value: pollingUnitPerPage, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@totalRecords", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@maxPage", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                var items = cnn.Query<MPDCPollingUnit>("GetMPDCPullingUnitsPaging", p,
                    commandType: CommandType.StoredProcedure);
                var results = (null != items) ? items.ToList() : new List<MPDCPollingUnit>();
                rets.Success(results);

                // Get Paging parameters
                rets.PageNo = p.Get<int>("@pageNum");
                rets.RowsPerPage = p.Get<int>("@rowsPerPage");
                //rets.TotalRecords = p.Get<int>("@totalRecords");
                rets.MaxPage = p.Get<int>("@maxPage");
                // Set error number/message
                rets.ErrNum = p.Get<int>("@errNum");
                rets.ErrMsg = p.Get<string>("@errMsg");
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
                rets.data = new List<MPDCPollingUnit>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MPDC

    /// <summary>
    /// The MPDC class.
    /// </summary>
    public class MPDC : NInpc
    {
        #region Internal Variables

        private bool _ImageLoading = false;
        private ImageSource _img = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }
        /// <summary>
        /// Gets or sets ADM1Code.
        /// </summary>
        public string ADM1Code { get; set; }
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
        /// Gets or sets CandidateNo.
        /// </summary>
        [ExcelColumn("ลำดับที่", 3)]
        public int CandidateNo { get; set; }

        /// <summary>
        /// Gets or sets PersonId.
        /// </summary>
        public int PersonId { get; set; }
        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [ExcelColumn("ชื่อผู้สมัคร", 4)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets PartyId.
        /// </summary>
        public int PartyId { get; set; }
        /// <summary>
        /// Gets or sets Prev PartyName.
        /// </summary>
        [ExcelColumn("สังกัดพรรคเดิม", 5)]
        public string PrevPartyName { get; set; }
        public string EducationName { get; set; }
        public string CandidateSubGroup { get; set; }
        public string CandidateRemark { get; set; }

        public byte[] Data { get; set; }

        public ImageSource Image
        {
            get
            {
                if (null == _img && !_ImageLoading)
                {
                    _ImageLoading = true;

                    Defaults.RunInBackground(() =>
                    {
                        ImageSource imgSrc;
                        if (null == Data)
                        {
                            imgSrc = Defaults.Person;
                        }
                        else
                        {
                            imgSrc = ByteUtils.GetImageSource(Data);
                        }
                        _img = imgSrc;

                        _ImageLoading = false;
                        Raise(() => Image);
                    });
                }
                return _img;
            }
            set { }
        }

        public string ProvinceNameOri { get; set; }
        public int PollingUnitNoOri { get; set; }
        public int CandidateNoOri { get; set; }

        public string GroupName
        {
            get { return string.Format("{0} เขต {1}", ProvinceNameTH, PollingUnitNo); }
            set { }
        }

        #endregion

        #region Public Methods

        public void LoadImageFile(string fileName)
        {
            this.Data = ByteUtils.GetFileBuffer(fileName);
            Raise(() => Data);
            _img = null;
            Raise(() => Image);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets
        /// </summary>
        /// <param name="thaiYear"></param>
        /// <param name="provinceName"></param>
        /// <param name="pollingUnitNo"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static NDbResult<List<MPDC>> Gets(int thaiYear, string provinceName, int pollingUnitNo, 
            string fullName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(sFullName))
            {
                sFullName = null;
            }

            NDbResult<List<MPDC>> rets = new NDbResult<List<MPDC>>();

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
            p.Add("@PollingUnitNo", pollingUnitNo);
            p.Add("@FullName", sFullName);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                var items = cnn.Query<MPDC>("GetMPDCs", p,
                    commandType: CommandType.StoredProcedure);
                var results = (null != items) ? items.ToList() : new List<MPDC>();
                rets.Success(results);

                // Set error number/message
                rets.ErrNum = p.Get<int>("@errNum");
                rets.ErrMsg = p.Get<string>("@errMsg");
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
                rets.data = new List<MPDC>();
            }

            return rets;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static NDbResult Delete(MPDC value)
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
            p.Add("@ADM1Code", value.ADM1Code);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@CandidateNo", value.CandidateNo);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("DeleteMPDC", p, commandType: CommandType.StoredProcedure);
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
