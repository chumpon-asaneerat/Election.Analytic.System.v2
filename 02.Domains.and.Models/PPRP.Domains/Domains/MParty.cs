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

namespace PPRP.Domains
{
    public class MParty : NInpc
    {
        #region Internal Variables

        private bool _ImageLoading = false;
        private ImageSource _img = null;

        #endregion

        #region Public Properties

        public int PartyId { get; set; }

        public string PartyName { get; set; }
        public Guid ContendId { get; set; }
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

        #endregion

        #region Static Methods

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
                rets.Value = (null != items) ? items.ToList() : null;

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

            if (null == rets.Value)
            {
                // create empty list.
                rets.Value = new List<MParty>();
            }

            return rets;
        }

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
            p.Add("@partyName", partyName);
            p.Add("@Data", data, dbType: DbType.Binary, direction: ParameterDirection.Input, size: -1);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportPartyImage", p, commandType: CommandType.StoredProcedure);
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
            p.Add("@partyName", value.PartyName);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("DeleteMParty", p, commandType: CommandType.StoredProcedure);
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
}
