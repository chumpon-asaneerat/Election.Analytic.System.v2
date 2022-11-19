#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Linq;

using Dapper;
using Newtonsoft.Json;

#endregion

namespace PPRP.Domains
{
    #region UserRole

    public class UserRole
    {
        #region Public Properties

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion

    #region UserInfo

    public class UserInfo
    {
        #region Public Properties

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Active { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion
}
