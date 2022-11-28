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
    #region UserInfo

    /// <summary>
    /// The UserInfo class.
    /// </summary>
    public class UserInfo : NInpc
    {
        #region Internal Variables

        private int _UserId = 0;

        private string _FullName = string.Empty;
        private string _UserName = string.Empty;
        private string _Password = string.Empty;

        private int _RoleId = 0;
        private string _RoleName = string.Empty;

        private int _Active = 1;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public UserInfo() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~UserInfo()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets UserId.
        /// </summary>
        public int UserId
        {
            get { return _UserId; }
            set
            {
                if (_UserId != value)
                {
                    _UserId = value;
                    Raise(() => UserId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Full Name.
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set
            {
                if (_FullName != value)
                {
                    _FullName = value;
                    Raise(() => FullName);
                }
            }
        }
        /// <summary>
        /// Gets or sets User Name.
        /// </summary>
        public string UserName
        {
            get { return _UserName; }
            set
            {
                if (_UserName != value)
                {
                    _UserName = value;
                    Raise(() => UserName);
                }
            }
        }
        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    Raise(() => Password);
                }
            }
        }
        /// <summary>
        /// Gets or sets Active.
        /// </summary>
        public int Active
        {
            get { return _Active; }
            set
            {
                if (_Active != value)
                {
                    _Active = value;
                    Raise(() => Active);
                }
            }
        }

        /// <summary>
        /// Gets or sets RoleId.
        /// </summary>
        public int RoleId
        {
            get { return _RoleId; }
            set
            {
                if (_RoleId != value)
                {
                    _RoleId = value;
                    Raise(() => RoleId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Role Name.
        /// </summary>
        public string RoleName
        {
            get { return _RoleName; }
            set
            {
                if (_RoleName != value)
                {
                    _RoleName = value;
                    Raise(() => RoleName);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets Users.
        /// </summary>
        /// <param name="fullName">Filter full name.</param>
        /// <param name="userName">Filter user name.</param>
        /// <param name="roleName">Filter role name.</param>
        /// <returns>Returns List of userinfo instance.</returns>
        public static NDbResult<List<UserInfo>> Gets(string fullName = null,
            string userName = null, string roleName =  null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<UserInfo>> rets = new NDbResult<List<UserInfo>>();

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
            p.Add("@FullName", fullName);
            p.Add("@UserName", userName);
            p.Add("@RoleName", roleName);
            p.Add("@RoleId", null);
            p.Add("@Active", 1);

            try
            {
                rets.Value = cnn.Query<UserInfo>("GetUsers", p,
                    commandType: CommandType.StoredProcedure).AsList();
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
                rets.Value = new List<UserInfo>();
            }

            return rets;
        }
        /// <summary>
        /// Gets User.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <returns>Returns match userinfo instance.</returns>
        public static NDbResult<UserInfo> Get(string userName, string password)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<UserInfo> rets = new NDbResult<UserInfo>();

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
            p.Add("@UserName", userName);
            p.Add("@Password", password);

            try
            {
                rets.Value = cnn.Query<UserInfo>("GetUser", p,
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
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
                // set to null.
                rets.Value = null;
            }

            return rets;
        }

        #endregion
    }

    #endregion
}
