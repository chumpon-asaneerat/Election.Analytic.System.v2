#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using NLib;
using NLib.Design;
using NLib.Reflection;

using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions;
// required for JsonIgnore attribute.
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

using PPRP.Models;
using PPRP.Services;

#endregion

namespace PPRP.Models
{
    #region LPAK

    /// <summary>
    /// The LPAK class
    /// </summary>
    public class LPAK : NTable<LPAK>
    {
        #region Public Properties

        /// <summary>
        /// Gets or set Id.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets RegionId.
        /// </summary>
        [MaxLength(20)]
        public string RegionId { get; set; }
        /// <summary>
        /// Gets or sets RegionName.
        /// </summary>
        [MaxLength(200)]
        public string RegionName { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<LPAK>> Gets()
        {
            NDbResult<List<LPAK>> ret = new NDbResult<List<LPAK>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;

                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LPAK ";
                    var results = NQuery.Query<LPAK>(cmd).ToList();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<LPAK> Get(
            string RegionId)
        {
            NDbResult<LPAK> ret = new NDbResult<LPAK>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(RegionId)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LPAK ";
                    cmd += " WHERE RegionId = ? ";
                    var results = NQuery.Query<LPAK>(cmd,
                        RegionId).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }

        #endregion
    }

    #endregion
}
