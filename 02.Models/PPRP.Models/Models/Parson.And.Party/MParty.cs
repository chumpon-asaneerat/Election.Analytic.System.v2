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
    #region  MParty

    /// <summary>
    /// The MParty class.
    /// </summary>
    public class MParty : NInpc
    {
        #region Internal Variables

        private Action _partyNameChangdAction = null;

        private int _PartyId = 0;
        private string _PartyName = null;

        private bool _isDefault = true;
        private bool _ImageLoading = false;
        private byte[] _data = null;
        private ImageSource _img = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MParty() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MParty()
        {
            _partyNameChangdAction = null;
        }

        #endregion

        #region Public Methods

        public void WhenPartyNameChanged(Action partyNameChangdAction)
        {
            _partyNameChangdAction = partyNameChangdAction;
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets or sets PartyId.
        /// </summary>
        public int PartyId
        {
            get { return _PartyId; }
            set
            {
                if (_PartyId != value)
                {
                    _PartyId = value;
                    // Raise Event
                    Raise(() => PartyId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Party Name.
        /// </summary>
        public string PartyName
        {
            get { return _PartyName; }
            set
            {
                if (_PartyName != value)
                {
                    _PartyName = value;
                    // Raise Event
                    Raise(() => PartyName);

                    if (null != _partyNameChangdAction) _partyNameChangdAction(); // raise action callback
                }
            }
        }
        /// <summary>
        /// Gets or sets Image Data buffers.
        /// </summary>
        public byte[] Data 
        {
            get { return _data; }
            set
            {
                _data = value;
                _img = null; // reset image.
                Raise(() => Data);
                Raise(() => Image);
            }
        }
        /// <summary>
        /// Gets ImageSource.
        /// </summary>
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
                            _isDefault = true;
                            imgSrc = Defaults.Party;
                        }
                        else
                        {
                            _isDefault = false;
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
        /// <summary>
        /// Checks is default image.
        /// </summary>
        public bool IsDefault { get { return _isDefault; } }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="partyName">The party name filter.</param>
        /// <param name="pageNo">The Page No.</param>
        /// <param name="rowPerPage">The Row per page.</param>
        /// <returns>Returns list of MParty.</returns>
        public static NDbResult<List<MParty>> Gets(string partyName,
            int pageNo = 1, int rowPerPage = 10)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MParty>> rets = new NDbResult<List<MParty>>();

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
            p.Add("@PartyName", partyName);

            p.Add("@pageNum", value: pageNo, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@rowsPerPage", value: rowPerPage, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            p.Add("@totalRecords", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@maxPage", value: 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                var items = cnn.Query<MParty>("GetMParties", p,
                    commandType: CommandType.StoredProcedure);
                var data = (null != items) ? items.ToList() : null;
                rets.Success(data);

                // Get Paging parameters
                rets.PageNo = p.Get<int>("@pageNum");
                rets.RowsPerPage = p.Get<int>("@rowsPerPage");
                rets.TotalRecords = p.Get<int>("@totalRecords");
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
                rets.data = new List<MParty>();
            }

            return rets;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static NDbResult Delete(MParty value)
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
            p.Add("@PartyId", value.PartyId);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("DeleteMParty", p, commandType: CommandType.StoredProcedure);
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
        /// Import.
        /// </summary>
        /// <param name="partyName">The party name.</param>
        /// <param name="data">The byte array of image.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(string partyName, byte[] data)
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
            p.Add("@PartyName", partyName);
            p.Add("@Data", data, dbType: DbType.Binary, direction: ParameterDirection.Input, size: -1);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportMPartyImage", p, commandType: CommandType.StoredProcedure);
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
        /// Save
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static NDbResult Save(MParty value)
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
            int? partyId = (value.PartyId <= 0) ? new int?() : value.PartyId;
            p.Add("@PartyId", partyId);
            p.Add("@PartyName", value.PartyName);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMParty", p, commandType: CommandType.StoredProcedure);
                ret.Success();
                // Set error number/message
                ret.ErrNum = p.Get<int>("@errNum");
                ret.ErrMsg = p.Get<string>("@errMsg");

                partyId = p.Get<int>("@PartyId"); // in case addnew
                value.PartyId = (partyId.HasValue) ? partyId.Value : 0;
                if (value.PartyId > 0)
                {
                    SaveImage(value); // now save image.
                }
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
        /// Save Image.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static NDbResult SaveImage(MParty value)
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
            p.Add("@PartyId", value.PartyId);
            p.Add("@Data", value.Data, dbType: DbType.Binary, direction: ParameterDirection.Input, size: -1);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMPartyImage", p, commandType: CommandType.StoredProcedure);
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

        public static NDbResult<MParty> Get(string partyName)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<MParty> rets = new NDbResult<MParty>();

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
            p.Add("@PartyName", partyName);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                var items = cnn.Query<MParty>("GetMPartyByName", p,
                    commandType: CommandType.StoredProcedure);
                var data = (null != items) ? items.FirstOrDefault() : null;
                rets.Success(data);

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
                // set as null.
                rets.data = null;
            }

            return rets;
        }


        #endregion
    }

    #endregion
}
