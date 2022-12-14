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
        [ExcelColumn("หมายเลขผู้สมัคร", 4)]
        public int CandidateNo { get; set; }
        /// <summary>
        /// Gets or sets PartyName.
        /// </summary>
        [ExcelColumn("ชื่อพรรค", 5)]
        public string PartyName { get; set; }
        /// <summary>
        /// Gets or sets FullName.
        /// </summary>
        [ExcelColumn("ชื่อผู้สมัคร", 3)]
        public string FullName { get; set; }
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
        /// Gets or sets CandidateNo.
        /// </summary>
        [ExcelColumn("หมายเลขผู้สมัคร", 5)]
        public int CandidateNo { get; set; }
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
                var data = cnn.Query<MPDVoteSummary>("GetMPDVoteSummaries", p,
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
                rets.data = new List<MPDVoteSummary>();
            }

            return rets;
        }

        public static NDbResult<MPDVoteSummary> Get(int thaiYear, string fullName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                sFullName = null;
            }

            NDbResult<MPDVoteSummary> rets = new NDbResult<MPDVoteSummary>();

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
                var items = cnn.Query<MPDVoteSummary>("GetMPDVoteSummaryByFullName", p,
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

        #endregion
    }

    #endregion

    #region MPDPrintVoteSummary

    public class MPDPrintVoteSummary
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear { get; set; }

        public int RowNo { get; set; }
        public int RankNo { get; set; }
        public string ProvinceNameTH { get; set; }
        public string ProvinceName { get { return ProvinceNameTH; } }
        public int PollingUnitNo { get; set; }
        public string FullName { get; set; }
        public int VoteNo { get; set; }
        public string PartyName { get; set; }
        public int VoteCount { get; set; }
        public int RevoteNo { get; set; }

        #endregion

        #region Static Methods

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="thaiYear"></param>
        /// <param name="provinceNameTH"></param>
        /// <param name="partyName"></param>
        /// <param name="fullName"></param>
        /// <returns></returns>

        public static NDbResult<List<MPDPrintVoteSummary>> Gets(
            int thaiYear,
            string provinceNameTH = null,
            string partyName = null,
            string fullName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceNameTH;
            if (string.IsNullOrWhiteSpace(sProvinceName) || sProvinceName.Contains("ทุกจังหวัด"))
            {
                sProvinceName = null;
            }

            string sPartyName = partyName;
            if (string.IsNullOrWhiteSpace(sPartyName))
            {
                sPartyName = null;
            }

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(sFullName))
            {
                sFullName = null;
            }


            NDbResult<List<MPDPrintVoteSummary>> rets = new NDbResult<List<MPDPrintVoteSummary>>();

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
            p.Add("@RegionId", null);
            p.Add("@RegionName", null);
            p.Add("@ProvinceNameTH", sProvinceName);
            p.Add("@PartyName", sPartyName);
            p.Add("@FullName", sFullName);

            try
            {
                var data = cnn.Query<MPDPrintVoteSummary>("GetMPDVoteSummaries", p,
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
                rets.data = new List<MPDPrintVoteSummary>();
            }

            return rets;
        }

        #endregion

        #endregion
    }

    #endregion

    #region MPDPersonalVoteSummary

    public class MPDPersonalVoteSummary : NInpc
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

        public int VoteNo { get; set; }
        public int VoteCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPDPersonalVoteSummary>> Gets(int thaiYear, string adm1Code, 
            int pollingUnitNo, int top = 6)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPDPersonalVoteSummary>> rets = new NDbResult<List<MPDPersonalVoteSummary>>();

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
            p.Add("@Top", top);

            try
            {
                var items = cnn.Query<MPDPersonalVoteSummary>("GetMPDTopVoteSummaries", p,
                    commandType: CommandType.StoredProcedure);
                var results = (null != items) ? items.ToList() : new List<MPDPersonalVoteSummary>();
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
                rets.data = new List<MPDPersonalVoteSummary>();
            }

            return rets;
        }

        public static int GetTotalVotes(int thaiYear, string adm1Code, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            int ret = 0;

            IDbConnection cnn = DbServer.Instance.Db;
            if (null == cnn || !DbServer.Instance.Connected)
            {
                string msg = "Connection is null or cannot connect to database server.";
                med.Err(msg);
                return ret;
            }

            var p = new DynamicParameters();
            p.Add("@ThaiYear", thaiYear);
            p.Add("@ADM1Code", adm1Code);
            p.Add("@PollingUnitNo", pollingUnitNo);

            try
            {
                ret = cnn.ExecuteScalar<int>("GetMPDTotalVotes", p,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                med.Err(ex);
            }

            return ret;
        }

        #endregion
    }

    #endregion

    #region MPDPrintVoteSummary2 (for analytic app)

    public class MPDPrintVoteSummary2
    {
        #region Public Properties

        #region Province/PollingUnitNo

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }

        #endregion

        #region Person 1

        public byte[] Logo1 { get; set; }
        public byte[] PersonImage1 { get; set; }
        public string PartyName1 { get; set; }
        public string FullName1 { get; set; }
        public int VoteCount1 { get; set; }

        #endregion

        #region Person 2

        public byte[] Logo2 { get; set; }
        public byte[] PersonImage2 { get; set; }
        public string PartyName2 { get; set; }
        public string FullName2 { get; set; }
        public int VoteCount2 { get; set; }

        #endregion

        #region Person 3

        public byte[] Logo3 { get; set; }
        public byte[] PersonImage3 { get; set; }
        public string PartyName3 { get; set; }
        public string FullName3 { get; set; }
        public int VoteCount3 { get; set; }

        #endregion

        #region Person 4

        public byte[] Logo4 { get; set; }
        public byte[] PersonImage4 { get; set; }
        public string PartyName4 { get; set; }
        public string FullName4 { get; set; }
        public int VoteCount4 { get; set; }

        #endregion

        #region Person 5

        public byte[] Logo5 { get; set; }
        public byte[] PersonImage5 { get; set; }
        public string PartyName5 { get; set; }
        public string FullName5 { get; set; }
        public int VoteCount5 { get; set; }

        #endregion

        #region Person 6

        public byte[] Logo6 { get; set; }
        public byte[] PersonImage6 { get; set; }
        public string PartyName6 { get; set; }
        public string FullName6 { get; set; }
        public int VoteCount6 { get; set; }

        #endregion

        #region Candidate

        public byte[] CandidateImage { get; set; }
        public string CandidateFullName { get; set; }
        public string CandidateSubGroup { get; set; }
        public string CandidatePrevYear { get; set; }
        public string CandidatePrevStatus { get; set; }
        public string CandidatePrevVote { get; set; }
        public string CandidateRemark { get; set; }

        #endregion

        #region General Vote information

        public int PrevVoteYear { get; set; }
        public int PollingUnitCount { get; set; }
        public int RightCount { get; set; }
        public int ExerciseCount { get; set; }
        public decimal ExercisePercent { get; set; }
        public int DifferenceVoteFromNo2 { get; set; }
        public int VoteCount7toLast { get; set; }

        #endregion

        #endregion
    }

    #endregion
}
