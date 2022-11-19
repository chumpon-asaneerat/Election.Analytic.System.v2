#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

using System.Windows.Media;
using System.Windows.Threading;

using NLib;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    #region MPD2562VoteSummary

    public class MPD2562VoteSummary
    {
        #region Public Properties

        public int RowNo { get; set; }
        public int RankNo { get; set; }
        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public string FullName { get; set; }
        public int VoteNo { get; set; }
        public string PartyName { get; set; }
        public int VoteCount { get; set; }
        public int RevoteNo { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2562VoteSummary>> Gets(string provinceName = null,
            string partyName = null, string fullName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;
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

            NDbResult<List<MPD2562VoteSummary>> rets = new NDbResult<List<MPD2562VoteSummary>>();

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
            p.Add("@PartyName", sPartyName);
            p.Add("@FullName", sFullName);

            try
            {
                var items = cnn.Query<MPD2562VoteSummary>("GetMPD2562VoteSummaries", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPD2562VoteSummary>();
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
                rets.Value = new List<MPD2562VoteSummary>();
            }

            return rets;
        }

        public static NDbResult<MPD2562VoteSummary> Get(string fullName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                sFullName = null;
            }

            NDbResult<MPD2562VoteSummary> rets = new NDbResult<MPD2562VoteSummary>();

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
            p.Add("@FullName", sFullName);

            try
            {
                var items = cnn.Query<MPD2562VoteSummary>("GetMPD2562VoteSummaryByFullName", p,
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
                rets.Value = null;
            }

            return rets;
        }

        public static NDbResult Save(MPD2562VoteSummary value)
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
            p.Add("@FullName", value.FullName);
            p.Add("@VoteNo", value.VoteNo);
            p.Add("@PartyName", value.PartyName);
            p.Add("@VoteCount", value.VoteCount);
            p.Add("@RevoteNo", value.RevoteNo);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMPD2562VoteSummary", p, commandType: CommandType.StoredProcedure);
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

        public static NDbResult Import(MPD2562VoteSummary value)
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
            p.Add("@FullName", value.FullName);
            p.Add("@VoteNo", value.VoteNo);
            p.Add("@PartyName", value.PartyName);
            p.Add("@VoteCount", value.VoteCount);
            p.Add("@RevoteNo", value.RevoteNo);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPD2562VoteSummary", p, commandType: CommandType.StoredProcedure);
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

    #region MPD2562PrintVoteSummary

    public class MPD2562PrintVoteSummary
    {
        #region Public Properties

        public int RowNo { get; set; }
        public int RankNo { get; set; }
        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public string FullName { get; set; }
        public int VoteNo { get; set; }
        public string PartyName { get; set; }
        public int VoteCount { get; set; }
        public int RevoteNo { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2562PrintVoteSummary>> Gets(string provinceName = null,
            string partyName = null, string fullName = null)
        {
            string sProvinceName = provinceName;
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

            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPD2562PrintVoteSummary>> rets = new NDbResult<List<MPD2562PrintVoteSummary>>();

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
            p.Add("@PartyName", sPartyName);
            p.Add("@FullName", sFullName);

            try
            {
                var items = cnn.Query<MPD2562PrintVoteSummary>("GetMPD2562VoteSummaries", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPD2562PrintVoteSummary>();
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
                rets.Value = new List<MPD2562PrintVoteSummary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MPD2562PersonalVoteSummary

    public class MPD2562PersonalVoteSummary : NInpc
    {
        #region Internal Variables

        // for party logo
        private byte[] _LogoData = null;
        private bool _PartyLogoLoading = false;
        private ImageSource _PartyLogo = null;
        // for person image
        private byte[] _PersonImageData = null;
        private bool _PersonImageLoading = false;
        private ImageSource _PersonImage = null;

        #endregion

        #region Public Properties

        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
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

        public byte[] LogoData 
        { 
            get { return _LogoData; }
            set
            {
                _LogoData = value;
                if (null == _LogoData)
                {
                    _PartyLogo = null;
                }
            }
        }
        public ImageSource PartyLogo 
        { 
            get 
            {
                if (null == _PartyLogo && null != _LogoData && !_PartyLogoLoading)
                {
                    _PartyLogoLoading = true;

                    Defaults.RunInBackground(() =>
                    {
                        _PartyLogo = ByteUtils.GetImageSource(LogoData);
                        _PartyLogoLoading = false;
                        Raise(() => PartyLogo);
                    });
                }
                return _PartyLogo;
            }
            set { } 
        }

        public int VoteNo { get; set; }
        public int VoteCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPD2562PersonalVoteSummary>> Gets(int top, string provinceId, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPD2562PersonalVoteSummary>> rets = new NDbResult<List<MPD2562PersonalVoteSummary>>();

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
            p.Add("@ProvinceId", provinceId);
            p.Add("@PollingUnitNo", pollingUnitNo);
            p.Add("@Top", top);

            try
            {
                var items = cnn.Query<MPD2562PersonalVoteSummary>("GetMPD2562TopVoteSummaries", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPD2562PersonalVoteSummary>();
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
                rets.Value = new List<MPD2562PersonalVoteSummary>();
            }

            return rets;
        }

        public static int GetTotalVotes(string provinceId, int pollingUnitNo)
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
            p.Add("@ProvinceId", provinceId);
            p.Add("@PollingUnitNo", pollingUnitNo);

            try
            {
                ret = cnn.ExecuteScalar<int>("GetMPD2562TotalVotes", p,
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
}
