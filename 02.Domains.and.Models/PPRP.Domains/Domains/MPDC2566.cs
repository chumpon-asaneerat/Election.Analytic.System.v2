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
    #region MPDC2566PullingUnit

    public class MPDC2566PullingUnit : NInpc
    {
        #region Internal Variables

        private bool _loaded = false;
        private  List<MPDC2566> _items = null;

        #endregion

        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int TotalCandidates { get; set; }
        public string FullNameFilter { get; set; }
        public string GroupName
        {
            get { return string.Format("{0} เขต {1}", ProvinceName, PollingUnitNo); }
            set { }
        }
        public List<MPDC2566> Items
        {
            get 
            { 
                if (!_loaded)
                {
                    if (TotalCandidates > 0)
                    {
                        _items = MPDC2566.Gets(ProvinceName, PollingUnitNo, FullNameFilter).Value;
                    }
                    _loaded = true;

                }
                return _items;
            }
        }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPDC2566PullingUnit>> Gets(string provinceName = null, string fullName = null,
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

            NDbResult<List<MPDC2566PullingUnit>> rets = new NDbResult<List<MPDC2566PullingUnit>>();

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
            p.Add("@FullName", sFullName);

            p.Add("@pageNum", value: pageNo, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@pollingUnitPerPage", value: pollingUnitPerPage, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@totalRecords", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@maxPage", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                var items = cnn.Query<MPDC2566PullingUnit>("GetMPDC2566PullingUnitsPaging", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPDC2566PullingUnit>();

                // Get Paging parameters
                rets.PageNo = p.Get<int>("@pageNum");
                rets.RowsPerPage = p.Get<int>("@pollingUnitPerPage");
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

            if (null == rets.Value)
            {
                // create empty list.
                rets.Value = new List<MPDC2566PullingUnit>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MPDC2566

    public class MPDC2566 : NInpc
    {
        #region Internal Variables

        private bool _ImageLoading = false;
        private ImageSource _img = null;

        #endregion

        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int CandidateNo { get; set; }
        public string FullName { get; set; }
        public string PrevPartyName { get; set; }
        public string EducationLevel { get; set; }
        public string SubGroup { get; set; }
        public string Remark { get; set; }

        public string ImageFullName { get; set; }
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
        public string FullNameOri { get; set; }

        public string GroupName 
        { 
            get { return string.Format("{0} เขต {1}", ProvinceName, PollingUnitNo); }
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

        public static NDbResult<List<MPDC2566>> Gets(string provinceName, int pollingUnitNo, string fullName = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            string sProvinceName = provinceName;

            string sFullName = fullName;
            if (string.IsNullOrWhiteSpace(sFullName))
            {
                sFullName = null;
            }

            NDbResult<List<MPDC2566>> rets = new NDbResult<List<MPDC2566>>();

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
            p.Add("@PollingUnitNo", pollingUnitNo);
            p.Add("@FullName", sFullName);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                var items = cnn.Query<MPDC2566>("GetMPDC2566s", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPDC2566>();

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

            if (null == rets.Value)
            {
                // create empty list.
                rets.Value = new List<MPDC2566>();
            }

            return rets;
        }

        public static NDbResult Save(MPDC2566 value)
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
            p.Add("@CandidateNo", value.CandidateNo);
            p.Add("@FullName", value.FullName);
            p.Add("@PrevPartyName", value.PrevPartyName);
            p.Add("@EducationLevel", value.EducationLevel);
            p.Add("@SubGroup", value.SubGroup);
            p.Add("@Remark", value.Remark);

            p.Add("@Data", value.Data, dbType: DbType.Binary, direction: ParameterDirection.Input, size: -1);
            p.Add("@ProvinceNameOri", value.ProvinceNameOri);
            p.Add("@PollingUnitNoOri", value.PollingUnitNoOri);
            p.Add("@CandidateNoOri", value.CandidateNoOri);
            p.Add("@FullNameOri", value.FullNameOri);
            p.Add("@ImageFullNameOri", value.ImageFullName);


            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMPDC2566", p, commandType: CommandType.StoredProcedure);
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

        public static NDbResult Import(MPDC2566 value)
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
            p.Add("@CandidateNo", value.CandidateNo);
            p.Add("@FullName", value.FullName);
            p.Add("@PrevPartyName", value.PrevPartyName);
            p.Add("@EducationLevel", value.EducationLevel);
            p.Add("@SubGroup", value.SubGroup);
            p.Add("@Remark", value.Remark);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPDC2566", p, commandType: CommandType.StoredProcedure);
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

        public static NDbResult Delete(MPDC2566 value)
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
            p.Add("@CandidateNo", value.CandidateNo);
            p.Add("@FullName", value.FullName);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("DeleteMPDC2566", p, commandType: CommandType.StoredProcedure);
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

    #region MPDC2566Summary

    public class MPDC2566Summary : NInpc
    {
        #region Internal Variables

        // for person image
        private byte[] _PersonImageData = null;
        private bool _PersonImageLoading = false;
        private ImageSource _PersonImage = null;

        #endregion

        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int CandidateNo { get; set; }
        public string FullName { get; set; }
        public string PrevPartyName { get; set; }
        public string EducationLevel { get; set; }
        public string SubGroup { get; set; }
        public string Remark { get; set; }

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

        #endregion

        #region Static Methods

        public static NDbResult<List<MPDC2566Summary>> Gets(int top, string provinceId, int pollingUnitNo)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MPDC2566Summary>> rets = new NDbResult<List<MPDC2566Summary>>();

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
                var items = cnn.Query<MPDC2566Summary>("GetMPDC2566Summaries", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPDC2566Summary>();
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
                rets.Value = new List<MPDC2566Summary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion

    #region MPDC2566PrintSummary

    public class MPDC2566PrintSummary
    {
        #region Public Properties

        public string ProvinceName { get; set; }
        public int PollingUnitNo { get; set; }
        public int CandidateNo { get; set; }
        public string FullName { get; set; }
        public string PrevPartyName { get; set; }
        public string EducationLevel { get; set; }
        public string Remark { get; set; }
        public string SubGroup { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<MPDC2566PrintSummary>> Gets(string provinceName = null, string fullName = null)
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

            NDbResult<List<MPDC2566PrintSummary>> rets = new NDbResult<List<MPDC2566PrintSummary>>();

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
            p.Add("@FullName", sFullName);

            try
            {
                var items = cnn.Query<MPDC2566PrintSummary>("GetMPDC2566FullSummaries", p,
                    commandType: CommandType.StoredProcedure);
                rets.Value = (null != items) ? items.ToList() : new List<MPDC2566PrintSummary>();
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
                rets.Value = new List<MPDC2566PrintSummary>();
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
