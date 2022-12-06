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

#endregion

namespace PPRP.Models
{
    #region LProvince

    /// <summary>
    /// The LProvince class
    /// </summary>
    public class LProvince : NTable<LProvince>
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
        /// Gets or sets ADM1Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets ProvinceName.
        /// </summary>
        [MaxLength(200)]
        public string ProvinceName { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<LProvince>> Gets(string RegionId = null)
        {
            NDbResult<List<LProvince>> ret = new NDbResult<List<LProvince>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;

                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LProvince ";
                    if (!string.IsNullOrWhiteSpace(RegionId))
                    {
                        cmd += " WHERE RegionId = ? ";
                        var results = NQuery.Query<LProvince>(cmd, RegionId).ToList();
                        ret.Success(results);
                    }
                    else
                    {
                        var results = NQuery.Query<LProvince>(cmd).ToList();
                        ret.Success(results);
                    }
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<LProvince> Get(
            string RegionId, string ADM0Code)
        {
            NDbResult<LProvince> ret = new NDbResult<LProvince>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(RegionId)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LProvince ";
                    cmd += " WHERE RegionId = ? ";
                    cmd += "   AND ADM0Code = ? ";
                    var results = NQuery.Query<LProvince>(cmd,
                        RegionId, ADM0Code).FirstOrDefault();
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
