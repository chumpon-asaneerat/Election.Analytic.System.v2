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
    #region Interfaces

    public interface IBound
    {
        double LF { get; set; }
        double TP { get; set; }
        double RT { get; set; }
        double BT { get; set; }
        double WD { get; set; }
        double HT { get; set; }
        double CX { get; set; }
        double CY { get; set; }
    }

    public interface IADM : IBound
    {
        string ADMCode { get; }
        string Name { get; }

        List<IADMPart> GetADMParts();
    }

    public interface IADMPart
    {
        int RecordId { get; set; }
        int PartId { get; set; }

        List<IADMPoint> GetADMPoints();
    }

    public interface IADMPoint
    {
        /*
        int RecordId { get; set; }
        int PartId { get; set; }
        */
        int PointId { get; set; }
        double X { get; set; }
        double Y { get; set; }
    }

    #endregion

    #region LADM0

    /// <summary>
    /// The ADM0 Data Model Class.
    /// </summary>
    [TypeConverter(typeof(PropertySorterSupportExpandableTypeConverter))]
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    //[Table("LADM0")]
    public class LADM0 : NTable<LADM0>, IADM
    {
        #region Public Properties

        #region ADM0 (ADM0Code Is PrimaryKey)

        /// <summary>
        /// Gets or sets ADM0 Code.
        /// </summary>
        [PrimaryKey, MaxLength(20)]
        public string ADM0Code { get; set; }
        /// <summary>
        /// Gets or sets Country Name EN.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string CountryNameEN { get; set; }
        /// <summary>
        /// Gets or sets Country Name TH.
        /// </summary>
        [MaxLength(200)]
        [Indexed]
        public string CountryNameTH { get; set; }

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

        #region Interface Implements

        [Ignore]
        string IADM.ADMCode { get { return ADM0Code; } }
        [Ignore]
        string IADM.Name { get { return CountryNameTH; } }

        List<IADMPart> IADM.GetADMParts()
        {
            return LADM0Part.Gets(ADM0Code).Value()?.ToList<IADMPart>();
        }

        #endregion

        #endregion

        #region Static Methods

        public static NDbResult<LADM0> Get(string ADM0Code = "TH")
        {
            NDbResult<LADM0> ret = new NDbResult<LADM0>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM0 ";
                    cmd += " WHERE ADM0Code = ? ";
                    var results = NQuery.Query<LADM0>(cmd, ADM0Code).FirstOrDefault();
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

    #region LADM0Part

    public class LADM0Part : NTable<LADM0Part>, IADMPart
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
            return LADM0Point.Gets(ADM0Code, RecordId, PartId).Value()?.ToList<IADMPoint>();
        }

        #endregion


        #region Static Methods

        public static NDbResult<LADM0Part> Get(string ADM0Code, int recordId, int partId)
        {
            NDbResult<LADM0Part> ret = new NDbResult<LADM0Part>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM0Part ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    var results = NQuery.Query<LADM0Part>(cmd, ADM0Code, recordId, partId).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }
        public static NDbResult<List<LADM0Part>> Gets(string ADM0Code)
        {
            NDbResult<List<LADM0Part>> ret = new NDbResult<List<LADM0Part>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM0Part ";
                    cmd += " WHERE ADM0Code = ? ";
                    var results = NQuery.Query<LADM0Part>(cmd, ADM0Code).ToList();
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

    #region LADM0Point

    public class LADM0Point : NTable<LADM0Point>, IADMPoint
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

        public static NDbResult<LADM0Point> Get(string ADM0Code, int recordId, int partId, int pointId)
        {
            NDbResult<LADM0Point> ret = new NDbResult<LADM0Point>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM0Point ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    cmd += "   AND PointId = ? ";
                    var results = NQuery.Query<LADM0Point>(cmd, ADM0Code, recordId, partId, pointId).FirstOrDefault();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }

        public static NDbResult<List<LADM0Point>> Gets(string ADM0Code, int recordId, int partId)
        {
            NDbResult<List<LADM0Point>> ret = new NDbResult<List<LADM0Point>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM0Point ";
                    cmd += " WHERE ADM0Code = ? ";
                    cmd += "   AND RecordId = ? ";
                    cmd += "   AND PartId = ? ";
                    var results = NQuery.Query<LADM0Point>(cmd, ADM0Code, recordId, partId).ToList();
                    ret.Success(results);
                }
                catch (Exception ex)
                {
                    med.Err(ex);
                }

                return ret;
            }
        }

        public static NDbResult<List<LADM0Point>> Gets(string ADM0Code)
        {
            NDbResult<List<LADM0Point>> ret = new NDbResult<List<LADM0Point>>();
            lock (sync)
            {
                SQLiteConnection db = Default;
                if (null == db) return ret;
                if (string.IsNullOrWhiteSpace(ADM0Code)) return ret;
                MethodBase med = MethodBase.GetCurrentMethod();
                try
                {
                    string cmd = string.Empty;
                    cmd += "SELECT * FROM LADM0Point ";
                    cmd += " WHERE ADM0Code = ? ";
                    var results = NQuery.Query<LADM0Point>(cmd, ADM0Code).ToList();
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
