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
    #region LSubdistrict

    /// <summary>
    /// The LSubdistrict class
    /// </summary>
    public class LDistrict : NTable<LDistrict>
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
        /// Gets or sets ADM2Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets DistrictName.
        /// </summary>
        [MaxLength(200)]
        public string DistrictName { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<List<LDistrict>> Gets(string RegionId, string ADM0Code = null)
        {
            NDbResult<List<LDistrict>> ret = new NDbResult<List<LDistrict>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;

                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LDistrict ";
                    cmd += " WHERE RegionId = ? ";
                    if (!string.IsNullOrWhiteSpace(ADM0Code))
                    {
                        cmd += "   AND ADM0Code = ? ";
                        var results = NQuery.Query<LDistrict>(cmd, RegionId, ADM0Code).ToList();
                        ret.Success(results);
                    }
                    else
                    {
                        var results = NQuery.Query<LDistrict>(cmd, RegionId).ToList();
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
        public static NDbResult<LDistrict> Get(
            string RegionId, string ADM0Code)
        {
            NDbResult<LDistrict> ret = new NDbResult<LDistrict>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(RegionId)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LDistrict ";
                    cmd += " WHERE RegionId = ? ";
                    cmd += "   AND ADM0Code = ? ";
                    var results = NQuery.Query<LDistrict>(cmd,
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
