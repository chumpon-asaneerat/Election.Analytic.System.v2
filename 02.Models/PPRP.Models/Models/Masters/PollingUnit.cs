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

namespace PPRP.Models
{
    #region PollingUnit

    /// <summary>
    /// The PollingUnit class.
    /// </summary>
    public class PollingUnit : NInpc
    {
        #region Internal Variables

        private int _ThaiYear = 0;
        private int _PollingUnitNo = 0;
        private int _PollingUnitCount = 0;
        private string _AreaRemark = null;

        private string _RegionId = null;
        private string _RegionName = null;
        private string _GeoGroup = null;
        private string _GeoSubGroup = null;

        private string _ADM1Code = null;
        private string _ProvinceId = null;
        private string _ProvinceNameEN = null;
        private string _ProvinceNameTH = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PollingUnit() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~PollingUnit()
        {

        }

        #endregion

        #region Public Properties

        #region PollingUnit

        /// <summary>
        /// Gets or sets ThaiYear.
        /// </summary>
        public int ThaiYear
        {
            get { return _ThaiYear; }
            set
            {
                if (_ThaiYear != value)
                {
                    _ThaiYear = value;
                    Raise(() => ThaiYear);
                }
            }
        }
        /// <summary>
        /// Gets or sets PollingUnit No.
        /// </summary>
        [ExcelColumn("เขตเลือกตั้ง", 2)]
        public int PollingUnitNo
        {
            get { return _PollingUnitNo; }
            set
            {
                if (_PollingUnitNo != value)
                {
                    _PollingUnitNo = value;
                    Raise(() => PollingUnitNo);
                }
            }
        }
        /// <summary>
        /// Gets or sets PollingUnit Count.
        /// </summary>
        [ExcelColumn("จำนวนหน่วยเลือกตั้ง", 3)]
        public int PollingUnitCount
        {
            get { return _PollingUnitCount; }
            set
            {
                if (_PollingUnitCount != value)
                {
                    _PollingUnitCount = value;
                    Raise(() => PollingUnitCount);
                }
            }
        }
        /// <summary>
        /// Gets or sets Area Remark.
        /// </summary>
        [ExcelColumn("ข้อมูลพื้นที่", 4)]
        public string AreaRemark
        {
            get { return _AreaRemark; }
            set
            {
                if (_AreaRemark != value)
                {
                    _AreaRemark = value;
                    Raise(() => AreaRemark);
                }
            }
        }

        #endregion

        #region Province

        /// <summary>
        /// Gets or sets ADM1 Code.
        /// </summary>
        public string ADM1Code
        {
            get { return _ADM1Code; }
            set
            {
                if (_ADM1Code != value)
                {
                    _ADM1Code = value;
                    Raise(() => ADM1Code);
                }
            }
        }
        /// <summary>
        /// Gets or sets ProvinceId.
        /// </summary>
        public string ProvinceId
        {
            get { return _ProvinceId; }
            set
            {
                if (_ProvinceId != value)
                {
                    _ProvinceId = value;
                    // Raise Event
                    Raise(() => ProvinceId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Province Name (EN).
        /// </summary>
        public string ProvinceNameEN
        {
            get { return _ProvinceNameEN; }
            set
            {
                if (_ProvinceNameEN != value)
                {
                    _ProvinceNameEN = value;
                    Raise(() => ProvinceNameEN);
                }
            }
        }
        /// <summary>
        /// Gets or sets Province Name (TH).
        /// </summary>
        [ExcelColumn("จังหวัด", 1)]
        public string ProvinceNameTH
        {
            get { return _ProvinceNameTH; }
            set
            {
                if (_ProvinceNameTH != value)
                {
                    _ProvinceNameTH = value;
                    Raise(() => ProvinceNameTH);
                }
            }
        }

        #endregion

        #region Region

        /// <summary>
        /// Gets or sets RegionId.
        /// </summary>
        public string RegionId
        {
            get { return _RegionId; }
            set
            {
                if (_RegionId != value)
                {
                    _RegionId = value;
                    // Raise Event
                    Raise(() => RegionId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Region Name.
        /// </summary>
        public string RegionName
        {
            get { return _RegionName; }
            set
            {
                if (_RegionName != value)
                {
                    _RegionName = value;
                    // Raise Event
                    Raise(() => RegionName);
                }
            }
        }
        /// <summary>
        /// Gets or sets Geo Group.
        /// </summary>
        public string GeoGroup
        {
            get { return _GeoGroup; }
            set
            {
                if (_GeoGroup != value)
                {
                    _GeoGroup = value;
                    // Raise Event
                    Raise(() => GeoGroup);
                }
            }
        }
        /// <summary>
        /// Gets or sets Geo SubGroup.
        /// </summary>
        public string GeoSubGroup
        {
            get { return _GeoSubGroup; }
            set
            {
                if (_GeoSubGroup != value)
                {
                    _GeoSubGroup = value;
                    // Raise Event
                    Raise(() => GeoSubGroup);
                }
            }
        }

        #endregion

        #endregion

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="thaiYear">The year in thai.</param>
        /// <param name="adm1code">The ADM1 Code.</param>
        /// <param name="provinceNameTH">The province name (th).</param>
        /// <param name="regionId">The region id.</param>
        /// <param name="regionName">The region name.</param>
        /// <param name="geoGroup">The geo group.</param>
        /// <param name="geoSubGroup">The geo subgroup.</param>
        /// <returns>Returns list of MProvince instance.</returns>
        public static NDbResult<List<PollingUnit>> Gets(
            int thaiYear = 0,
            string adm1code = null, string provinceNameTH = null,
            string regionId = null, string regionName = null,
            string geoGroup = null, string geoSubGroup = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<PollingUnit>> rets = new NDbResult<List<PollingUnit>>();

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
            p.Add("@ThaiYear", thaiYear);
            p.Add("@ADM1Code", adm1code);
            p.Add("@ProvinceNameTH", provinceNameTH);
            p.Add("@RegionId", regionId);
            p.Add("@RegionName", regionName);
            p.Add("@GeoGroup", geoGroup);
            p.Add("@GeoSubGroup", geoSubGroup);

            try
            {
                rets.Value = cnn.Query<PollingUnit>("GetPollingUnits", p,
                    commandType: CommandType.StoredProcedure).ToList();
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
                rets.Value = new List<PollingUnit>();
            }

            return rets;
        }

        /// <summary>
        /// Import PollingUnit.
        /// </summary>
        /// <param name="value">The PollingUnit value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Import(PollingUnit value)
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

            if (null == value)
            {
                string msg = "Value is null.";
                med.Err(msg);
                // Set error number/message
                ret.ErrNum = 8000;
                ret.ErrMsg = msg;

                return ret;
            }

            var p = new DynamicParameters();
            p.Add("@ThaiYear", value.ThaiYear);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@PollingUnitNo", value.PollingUnitNo);
            p.Add("@PollingUnitCount", value.PollingUnitCount);
            p.Add("@AreaRemark", value.AreaRemark);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportPollingUnit", p, commandType: CommandType.StoredProcedure);
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

        /*
        /// <summary>
        /// Gets Combine Polling Units.
        /// </summary>
        /// <param name="adm1code">The ADM1 Code.</param>
        /// <param name="provinceNameTH">The province name (th).</param>
        /// <param name="regionId">The region id.</param>
        /// <param name="regionName">The region name.</param>
        /// <param name="geoGroup">The geo group.</param>
        /// <param name="geoSubGroup">The geo subgroup.</param>
        /// <returns>Returns list of MProvince instance.</returns>
        public static NDbResult<List<PollingUnit>> GetCombines(
            string adm1code = null, string provinceNameTH = null,
            string regionId = null, string regionName = null,
            string geoGroup = null, string geoSubGroup = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<PollingUnit>> rets = new NDbResult<List<PollingUnit>>();

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
            p.Add("@ADM1Code", adm1code);
            p.Add("@ProvinceNameTH", provinceNameTH);
            p.Add("@RegionId", regionId);
            p.Add("@RegionName", regionName);
            p.Add("@GeoGroup", geoGroup);
            p.Add("@GeoSubGroup", geoSubGroup);

            try
            {
                rets.Value = cnn.Query<PollingUnit>("GetCombinePollingUnits", p,
                    commandType: CommandType.StoredProcedure).ToList();
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
                rets.Value = new List<PollingUnit>();
            }

            return rets;
        }
        */

        #endregion
    }

    #endregion
}
