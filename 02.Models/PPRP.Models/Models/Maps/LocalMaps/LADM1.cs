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
    #region LADM1

    /// <summary>
    /// The ADM1 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("LADM1")]
    public class LADM1 : NTable<LADM1>, IADM
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

        #region ADM1 (ADM1Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        [Indexed]
        public string ADM1Code { get; set; }
        /// <summary>
        /// Gets or sets Province Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string ProvinceNameEN { get; set; }
        /// <summary>
        /// Gets or sets Province Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string ProvinceNameTH { get; set; }

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
        string IADM.ADMCode { get { return ADM1Code; } }
        [Ignore]
        string IADM.Name { get { return ProvinceNameTH; } }

        List<IADMPart> IADM.GetADMParts()
        {
            return LADM1Part.Gets(ADM1Code).Value()?.ToList<IADMPart>();
        }

        #endregion

        #region Static Methods

        public static NDbResult<LADM1> Get(string ADM1Code)
        {
            NDbResult<LADM1> ret = new NDbResult<LADM1>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM1 ";
                    cmd += " WHERE ADM1Code = ? ";
                    var results = NQuery.Query<LADM1>(cmd, ADM1Code).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<List<LADM1>> Gets()
        {
            NDbResult<List<LADM1>> ret = new NDbResult<List<LADM1>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM1 ";
                    cmd += " ORDER BY ADM1Code ";
                    var results = NQuery.Query<LADM1>(cmd).ToList();
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

    #region LADM1Part

    public class LADM1Part : NTable<LADM1Part>, IADMPart
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
            return LADM1Point.Gets(ADM1Code, RecordId, PartId).Value()?.ToList<IADMPoint>();
        }

        #endregion

        #region Static Methods

        public static NDbResult<LADM1Part> Get(
            string ADM0Code, string ADM1Code, 
            int recordId, int partId)
        {
            NDbResult<LADM1Part> ret = new NDbResult<LADM1Part>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM1Part ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    var results = NQuery.Query<LADM1Part>(cmd,
                        ADM0Code, ADM1Code, 
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
        public static NDbResult<List<LADM1Part>> Gets(string ADM1Code)
        {
            NDbResult<List<LADM1Part>> ret = new NDbResult<List<LADM1Part>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM1Part ";
                    cmd += " WHERE ADM1Code = ? ";
                    cmd += " ORDER BY ADM1Code, RecordId, PartId ";
                    var results = NQuery.Query<LADM1Part>(cmd, ADM1Code).ToList();
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

    #region LADM1Point

    public class LADM1Point : NTable<LADM1Point>, IADMPoint
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

        public static NDbResult<LADM1Point> Get(
            string ADM0Code, string ADM1Code,
            int recordId, int partId, int pointId)
        {
            NDbResult<LADM1Point> ret = new NDbResult<LADM1Point>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM1Point ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND ADM1Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    cmd += "   AND PointId = ? ";
                    var results = NQuery.Query<LADM1Point>(cmd,
                        ADM0Code, ADM1Code,
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
        public static NDbResult<List<LADM1Point>> Gets(string ADM1Code, int recordId, int partId)
        {
            NDbResult<List<LADM1Point>> ret = new NDbResult<List<LADM1Point>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM1Point ";
                    cmd += " WHERE ADM1Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    cmd += " ORDER BY ADM1Code, RecordId, PartId, PointId ";
                    var results = NQuery.Query<LADM1Point>(cmd, ADM1Code, recordId, partId).ToList();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<List<LADM1Point>> Gets(string ADM1Code)
        {
            NDbResult<List<LADM1Point>> ret = new NDbResult<List<LADM1Point>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM1Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM1Point ";
                    cmd += " WHERE ADM1Code = ? ";
                    cmd += " ORDER BY ADM1Code, RecordId, PartId, PointId ";
                    var results = NQuery.Query<LADM1Point>(cmd, ADM1Code).ToList();
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
