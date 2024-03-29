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
    #region LADM2

    /// <summary>
    /// The ADM2 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("LADM2")]
    public class LADM2 : NTable<LADM2>, IADM
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

        #region ADM2 (ADM2Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM2 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        [Indexed]
        public string ADM2Code { get; set; }
        /// <summary>
        /// Gets or sets District Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string DistrictNameEN { get; set; }
        /// <summary>
        /// Gets or sets District Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string DistrictNameTH { get; set; }

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

        #region Interface Implements

        [Ignore]
        string IADM.ADMCode { get { return ADM2Code; } }
        [Ignore]
        string IADM.Name { get { return string.IsNullOrWhiteSpace(DistrictNameTH) ? DistrictNameEN : DistrictNameTH; } }
        List<IADMPart> IADM.GetADMParts()
        {
            return LADM2Part.Gets(ADM2Code).Value()?.ToList<IADMPart>();
        }

        #endregion

        #region Static Methods

        public static NDbResult<LADM2> Get(string ADM2Code)
        {
            NDbResult<LADM2> ret = new NDbResult<LADM2>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM2 ";
                    cmd += " WHERE ADM2Code = ? ";
                    var results = NQuery.Query<LADM2>(cmd, ADM2Code).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<List<LADM2>> Gets(string ADM1Code)
        {
            NDbResult<List<LADM2>> ret = new NDbResult<List<LADM2>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM2 ";
                    cmd += " WHERE ADM1Code = ? ";
                    cmd += " ORDER BY ADM2Code ";
                    var results = NQuery.Query<LADM2>(cmd, ADM1Code).ToList();
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

    #region LADM2Part

    public class LADM2Part : NTable<LADM2Part>, IADMPart
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
        /// Gets or sets RecordId.
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets PartId.
        /// </summary>
        public int PartId { get; set; }
        /// <summary>
        /// Gets or sets Point count.
        /// </summary>
        public int PointCount { get; set; }

        #endregion

        #region Interface Implements

        List<IADMPoint> IADMPart.GetADMPoints()
        {
            return LADM2Point.Gets(ADM2Code, RecordId, PartId).Value()?.ToList<IADMPoint>();
        }

        #endregion

        #region Static Methods

        public static NDbResult<LADM2Part> Get(
            string ADM0Code, string ADM1Code, string ADM2Code,
            int recordId, int partId)
        {
            NDbResult<LADM2Part> ret = new NDbResult<LADM2Part>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM2Part ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND ADM2Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    var results = NQuery.Query<LADM2Part>(cmd,
                        ADM0Code, ADM1Code, ADM2Code,
                        recordId, partId).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<List<LADM2Part>> Gets(string ADM2Code)
        {
            NDbResult<List<LADM2Part>> ret = new NDbResult<List<LADM2Part>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM2Part ";
                    cmd += " WHERE ADM2Code = ? ";
                    cmd += " ORDER BY ADM2Code, RecordId, PartId ";
                    var results = NQuery.Query<LADM2Part>(cmd, ADM2Code).ToList();
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

    #region LADM2Point

    public class LADM2Point : NTable<LADM2Point>, IADMPoint
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
        /// Gets or sets RecordId.
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// Gets or sets PartId.
        /// </summary>
        public int PartId { get; set; }
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

        public static NDbResult<LADM2Point> Get(
            string ADM0Code, string ADM1Code, string ADM2Code,
            int recordId, int partId, int pointId)
        {
            NDbResult<LADM2Point> ret = new NDbResult<LADM2Point>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM2Point ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND ADM2Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    cmd += "   AND PointId = ? ";
                    var results = NQuery.Query<LADM2Point>(cmd,
                        ADM0Code, ADM1Code, ADM2Code,
                        recordId, partId, pointId).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<List<LADM2Point>> Gets(string ADM2Code, int recordId, int partId)
        {
            NDbResult<List<LADM2Point>> ret = new NDbResult<List<LADM2Point>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM2Point ";
                    cmd += " WHERE ADM2Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    cmd += " ORDER BY ADM2Code, RecordId, PartId, PointId ";
                    var results = NQuery.Query<LADM2Point>(cmd, ADM2Code, recordId, partId).ToList();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<List<LADM2Point>> Gets(string ADM2Code)
        {
            NDbResult<List<LADM2Point>> ret = new NDbResult<List<LADM2Point>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM2Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM2Point ";
                    cmd += " WHERE ADM2Code = ? ";
                    cmd += " ORDER BY ADM2Code, RecordId, PartId, PointId ";
                    var results = NQuery.Query<LADM2Point>(cmd, ADM2Code).ToList();
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
