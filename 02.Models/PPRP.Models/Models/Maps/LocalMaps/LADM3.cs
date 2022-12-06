﻿#region Using

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
    #region LADM3

    /// <summary>
    /// The ADM3 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("LADM3")]
    public class LADM3 : NTable<LADM3>
    {
        #region Public Properties

        #region ADM0

        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [MaxLength(20)]
        [Indexed]
        public string ADM0Code { get; set; }

        #endregion

        #region ADM1

        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [MaxLength(20)]
        [Indexed]
        public string ADM1Code { get; set; }

        #endregion

        #region ADM2

        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [MaxLength(20)]
        [Indexed]
        public string ADM2Code { get; set; }

        #endregion

        #region ADM3 (ADM3Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM3 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        [Indexed]
        public string ADM3Code { get; set; }
        /// <summary>
        /// Gets or sets Subdistrict Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string SubdistrictNameEN { get; set; }
        /// <summary>
        /// Gets or sets Subdistrict Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string SubdistrictNameTH { get; set; }

        #endregion

        #region Bounds

        /// <summary>
        /// Gets Bound Left position.
        /// </summary>
        public double LF { get; set; }
        /// <summary>
        /// Gets Bound Top position.
        /// </summary>
        public double TP { get; set; }
        /// <summary>
        /// Gets Bound Right position.
        /// </summary>
        public double RT { get; set; }
        /// <summary>
        /// Gets Bound Bottom position.
        /// </summary>
        public double BT { get; set; }
        /// <summary>
        /// Gets Bound Width.
        /// </summary>
        [Ignore]
        public double WD
        {
            get { return RT - LF; }
            set { }
        }
        /// <summary>
        /// Gets Bound Height.
        /// </summary>
        [Ignore]
        public double HT
        {
            get { return BT - TP; }
            set { }
        }
        /// <summary>
        /// Gets Bound Center X.
        /// </summary>
        [Ignore]
        public double CX
        {
            get { return LF + (double)(WD / (double)2); }
            set { }
        }
        /// <summary>
        /// Gets Bound Center Y.
        /// </summary>
        public double CY
        {
            get { return TP + (double)(HT / (double)2); }
            set { }
        }

        #endregion

        #endregion

        #region Static Methods

        #endregion
    }

    #endregion

    #region LADM3Part

    public class LADM3Part : NTable<LADM3Part>
    {
        #region Public Properties

        /// <summary>
        /// Gets or set Id.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM0Code { get; set; }
        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets ADM3 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM3Code { get; set; }
        /// <summary>
        /// Gets or sets RecordId.
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets Point count.
        /// </summary>
        public int PointCount { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<LADM3Part> Get(
            string ADM0Code, string ADM1Code, string ADM2Code, string ADM3Code,
            int recordId)
        {
            NDbResult<LADM3Part> ret = new NDbResult<LADM3Part>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM3Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM3Part ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND ADM2Code = ? ";
                    cmd += "   AND ADM3Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    var results = NQuery.Query<LADM3Part>(cmd,
                        ADM0Code, ADM1Code, ADM2Code, ADM3Code,
                        recordId).FirstOrDefault();
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

    #region LADM3Point

    public class LADM3Point : NTable<LADM3Point>
    {
        #region Public Properties

        /// <summary>
        /// Gets or set Id.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM0Code { get; set; }
        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets ADM3 Code.
        /// </summary>
        [MaxLength(20)]
        public string ADM3Code { get; set; }
        /// <summary>
        /// Gets or sets RecordId.
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets Point Id.
        /// </summary>
        public int PointId { get; set; }
        /// <summary>
        /// Gets or sets Point X position.
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Gets or sets Point Y position.
        /// </summary>
        public double Y { get; set; }

        #endregion

        #region Static Methods

        public static NDbResult<LADM3Point> Get(
            string ADM0Code, string ADM1Code, string ADM2Code, string ADM3Code,
            int recordId, int pointId)
        {
            NDbResult<LADM3Point> ret = new NDbResult<LADM3Point>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM3Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM3Point ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND ADM2Code = ? ";
                    cmd += "   AND ADM3Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PointId = ? ";
                    var results = NQuery.Query<LADM3Point>(cmd,
                        ADM0Code, ADM1Code, ADM2Code, ADM3Code,
                        recordId, pointId).FirstOrDefault();
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
