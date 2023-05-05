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

    #region MPDCOfficialVoteSummary

    public class MPDCOfficialVoteSummary : NInpc
    {
        #region Internal Variables

        // for party logo
        private byte[] _PartyImageData = null;
        private bool _PartyImageLoading = false;
        private ImageSource _PartyImage = null;
        // for person image
        private byte[] _PersonImageData = null;
        private bool _PersonImageLoading = false;
        private ImageSource _PersonImage = null;

        #endregion

        #region Public Properties

        public int ThaiYear { get; set; }
        public string ADM1Code { get; set; }
        public string ProvinceNameTH { get; set; }
        public int PollingUnitNo { get; set; }

        public string FullName { get; set; }

        public byte[] PersonImageData
        {
            get { return _PersonImageData; }
            set
            {
                _PersonImageData = value;
                if (null == _PersonImageData)
                {
                    _PersonImage = null;
                }
            }
        }

        public ImageSource PersonImage
        {
            get
            {
                if (null == _PersonImage && !_PersonImageLoading)
                {
                    _PersonImageLoading = true;

                    Defaults.RunInBackground(() =>
                    {
                        ImageSource imgSrc;
                        if (null == _PersonImageData)
                        {
                            imgSrc = Defaults.Person;
                        }
                        else
                        {
                            imgSrc = ByteUtils.GetImageSource(PersonImageData);
                        }
                        _PersonImage = imgSrc;

                        _PersonImageLoading = false;
                        Raise(() => PersonImage);
                    });
                }

                return _PersonImage;
            }
            set { }
        }

        public string PartyId { get; set; }
        public string PartyName { get; set; }

        public byte[] PartyImageData
        {
            get { return _PartyImageData; }
            set
            {
                _PartyImageData = value;
                if (null == _PartyImageData)
                {
                    _PartyImageData = null;
                }
            }
        }
        public ImageSource PartyImage
        {
            get
            {
                if (null == _PartyImage && null != _PartyImage && !_PartyImageLoading)
                {
                    _PartyImageLoading = true;

                    Defaults.RunInBackground(() =>
                    {
                        _PartyImage = ByteUtils.GetImageSource(_PartyImageData);
                        _PartyImageLoading = false;
                        Raise(() => PartyImage);
                    });
                }
                return _PartyImage;
            }
            set { }
        }

        public int SortOrder { get; set; }
        public int VoteCount { get; set; }

        public string PrevADM1Code { get; set; }
        public string PrevProvinceNameTH { get; set; }
        public int? PrevPollingUnitNo { get; set; }

        public string PrevPartyId { get; set; }
        public string PrevPartyName { get; set; }
        public int? PrevVoteCount { get; set; }
        public int? PrevRankNo { get; set; }

        public string PrevInfoText
        {
            get 
            {
                string txt = string.Empty;
                if (PrevVoteCount.HasValue && PrevRankNo.HasValue)
                {
                    txt += "ข้อมูลปี 2562" + Environment.NewLine;
                    txt += string.Format("คะแนน: {0:n0} (ลำดับที่ {1})", PrevVoteCount.Value, PrevRankNo.Value) + Environment.NewLine;
                    txt += string.Format("พรรค: {0}", PrevPartyName) + Environment.NewLine;
                    txt += string.Format("จังหวัด: {0} เขต {1}", PrevProvinceNameTH, PrevPollingUnitNo);
                }

                return txt;
            }
            set { }
        }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPDCOfficialVoteSummary>> Gets(
            int thaiYear, int prevThaiYear, 
            string adm1Code,int pollingUnitNo, int top = 6)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPDCOfficialVoteSummary>> rets = new NDbResult<List<MPDCOfficialVoteSummary>>();

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
            p.Add("@PrevThaiYear", prevThaiYear);
            p.Add("@ADM1Code", adm1Code);
            p.Add("@PollingUnitNo", pollingUnitNo);
            p.Add("@Top", top);

            try
            {
                var items = cnn.Query<MPDCOfficialVoteSummary>("GetMPDCOfficialTopVoteSummaries", p,
                    commandType: CommandType.StoredProcedure);
                var results = (null != items) ? items.ToList() : new List<MPDCOfficialVoteSummary>();
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
                rets.data = new List<MPDCOfficialVoteSummary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MPDCOfficialPrintVoteSummary (for analytic app)

    public class MPDCOfficialPrintVoteSummary
    {
        #region Public Properties

        #region Province/PollingUnitNo

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }

        public string AreaInfo62 { get; set; }
        public string AreaInfo66 { get; set; }

        #endregion

        #region Person 1

        public byte[] Logo1 { get; set; }
        public byte[] PersonImage1 { get; set; }
        public string PartyName1 { get; set; }
        public string FullName1 { get; set; }
        public int VoteCount1 { get; set; }

        public string PrevProvinceName1 { get; set; }
        public int PrevPollingUnitNo1 { get; set; }

        public string PrevPartyName1 { get; set; }
        public int PrevVoteCount1 { get; set; }
        public int PrevRankNo1 { get; set; }

        #endregion

        #region Person 2

        public byte[] Logo2 { get; set; }
        public byte[] PersonImage2 { get; set; }
        public string PartyName2 { get; set; }
        public string FullName2 { get; set; }
        public int VoteCount2 { get; set; }

        public string PrevProvinceName2 { get; set; }
        public int PrevPollingUnitNo2 { get; set; }

        public string PrevPartyName2 { get; set; }
        public int PrevVoteCount2 { get; set; }
        public int PrevRankNo2 { get; set; }

        #endregion

        #region Person 3

        public byte[] Logo3 { get; set; }
        public byte[] PersonImage3 { get; set; }
        public string PartyName3 { get; set; }
        public string FullName3 { get; set; }
        public int VoteCount3 { get; set; }

        public string PrevProvinceName3 { get; set; }
        public int PrevPollingUnitNo3 { get; set; }

        public string PrevPartyName3 { get; set; }
        public int PrevVoteCount3 { get; set; }
        public int PrevRankNo3 { get; set; }

        #endregion

        #region Person 4

        public byte[] Logo4 { get; set; }
        public byte[] PersonImage4 { get; set; }
        public string PartyName4 { get; set; }
        public string FullName4 { get; set; }
        public int VoteCount4 { get; set; }

        public string PrevProvinceName4 { get; set; }
        public int PrevPollingUnitNo4 { get; set; }

        public string PrevPartyName4 { get; set; }
        public int PrevVoteCount4 { get; set; }
        public int PrevRankNo4 { get; set; }

        #endregion

        #region Person 5

        public byte[] Logo5 { get; set; }
        public byte[] PersonImage5 { get; set; }
        public string PartyName5 { get; set; }
        public string FullName5 { get; set; }
        public int VoteCount5 { get; set; }

        public string PrevProvinceName5 { get; set; }
        public int PrevPollingUnitNo5 { get; set; }

        public string PrevPartyName5 { get; set; }
        public int PrevVoteCount5 { get; set; }
        public int PrevRankNo5 { get; set; }

        #endregion

        #region Person 6

        public byte[] Logo6 { get; set; }
        public byte[] PersonImage6 { get; set; }
        public string PartyName6 { get; set; }
        public string FullName6 { get; set; }
        public int VoteCount6 { get; set; }

        public string PrevProvinceName6 { get; set; }
        public int PrevPollingUnitNo6 { get; set; }

        public string PrevPartyName6 { get; set; }
        public int PrevVoteCount6 { get; set; }
        public int PrevRankNo6 { get; set; }

        #endregion

        #endregion
    }

    #endregion
}
