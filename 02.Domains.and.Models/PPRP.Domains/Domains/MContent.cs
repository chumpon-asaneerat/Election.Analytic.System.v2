#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;
using System.Reflection;

using NLib;
using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    public class MContent
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MContent() : base()
        {

        }

        #endregion

        #region Public Properties

        public Guid? ContentId { get; set; }
        public FileTypes FileTypeId { get; set; }
        public FileSubTypes FileSubTypeId { get; set; }
        public byte[] Data { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<MContent> Get(Guid contentId)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<MContent> ret = new NDbResult<MContent>();
            ret.Value = null;

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
            p.Add("@ContentId", contentId, dbType: DbType.Guid, direction: ParameterDirection.Input);
            p.Add("@FileTypeId", null, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@FileSubTypeId", null, dbType: DbType.Int32, direction: ParameterDirection.Input);

            try
            {
                ret.Value = cnn.Query<MContent>("GetMContents", p,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                med.Err(ex);
                ret.Value = null;
                // Set error number/message
                ret.ErrNum = 9999;
                ret.ErrMsg = ex.Message;
            }

            return ret;
        }

        public static NDbResult Save(MContent value)
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
            p.Add("@Data", value.Data, dbType: DbType.Binary, direction: ParameterDirection.Input, size: -1);
            p.Add("@FileTypeId", value.FileTypeId, dbType: DbType.Int32, direction: ParameterDirection.Input);
            p.Add("@FileSubTypeId", value.FileSubTypeId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            p.Add("@ContentId", dbType: DbType.Guid, direction: ParameterDirection.InputOutput);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);
            try
            {
                cnn.Execute("SaveMContent", p, commandType: CommandType.StoredProcedure);

                value.ContentId = p.Get<Guid>("@ContentId");
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
