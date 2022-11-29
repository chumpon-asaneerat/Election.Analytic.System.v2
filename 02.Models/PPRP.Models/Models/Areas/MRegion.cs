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
    #region MADMPak

    /// <summary>
    /// The MADMPak class
    /// </summary>
    public class MADMPak : NInpc
    {
        #region Internal Variables

        private string _RegionName = null;

        private string _ProvinceId = null;
        private string _ProvinceNameTH = null;

        private string _DistrictId = null;
        private string _DistrictNameTH = null;
        
        private string _SubDistrictId = null;
        private string _SubDistrictNameTH = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MADMPak() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MADMPak()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets Region Name.
        /// </summary>
        [ExcelColumn("ภาค")]
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
        /// Gets or sets Province Id.
        /// </summary>
        [ExcelColumn("รหัสจังหวัด")]
        public string ProvinceId
        {
            get { return _ProvinceId; }
            set
            {
                if (_ProvinceId != value)
                {
                    _ProvinceId = value;
                    Raise(() => ProvinceId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Province Name (TH).
        /// </summary>
        [ExcelColumn("จังหวัด")]
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

        /// <summary>
        /// Gets or sets Sub District Id.
        /// </summary>
        [ExcelColumn("รหัสอำเภอ")]
        public string DistrictId
        {
            get { return _DistrictId; }
            set
            {
                if (_DistrictId != value)
                {
                    _DistrictId = value;
                    Raise(() => DistrictId);
                }
            }
        }
        /// <summary>
        /// Gets or sets District Name (TH).
        /// </summary>
        [ExcelColumn("อำเภอ")]
        public string DistrictNameTH
        {
            get { return _DistrictNameTH; }
            set
            {
                if (_DistrictNameTH != value)
                {
                    _DistrictNameTH = value;
                    Raise(() => DistrictNameTH);
                }
            }
        }

        /// <summary>
        /// Gets or sets Sub District Id.
        /// </summary>
        [ExcelColumn("รหัสตำบล")]
        public string SubDistrictId
        {
            get { return _SubDistrictId; }
            set
            {
                if (_SubDistrictId != value)
                {
                    _SubDistrictId = value;
                    Raise(() => SubDistrictId);
                }
            }
        }
        /// <summary>
        /// Gets or sets Sub District Name (TH).
        /// </summary>
        [ExcelColumn("ตำบล")]
        public string SubDistrictNameTH
        {
            get { return _SubDistrictNameTH; }
            set
            {
                if (_SubDistrictNameTH != value)
                {
                    _SubDistrictNameTH = value;
                    Raise(() => SubDistrictNameTH);
                }
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Import ADM Pak.
        /// </summary>
        /// <param name="value">The MADMPak value.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult ImportADMPak(MADMPak value)
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
            p.Add("@RegionName", value.RegionName);
            p.Add("@ProvinceId", value.ProvinceId);
            p.Add("@DistrictId", value.DistrictId);
            p.Add("@SubDistrictId", value.SubDistrictId);
            p.Add("@ProvinceNameTH", value.ProvinceNameTH);
            p.Add("@DistrictNameTH", value.DistrictNameTH);
            p.Add("@SubDistrictNameTH", value.SubDistrictNameTH);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("ImportADMPak", p, commandType: CommandType.StoredProcedure);
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

        #endregion
    }

    #endregion

    #region MRegion

    /// <summary>
    /// The MRegion class.
    /// </summary>
    public class MRegion : NInpc
    {
        #region Internal Variables

        private int _RegionId = 0;
        private string _RegionName = null;
        private string _GeoGroup = null;
        private string _GeoSubGroup = null;

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MRegion() : base()
        {

        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~MRegion()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets RegionId.
        /// </summary>
        public int RegionId
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

        #region Static Methods

        /// <summary>
        /// Gets.
        /// </summary>
        /// <param name="regionId">The region id.</param>
        /// <param name="regionName">The region name.</param>
        /// <param name="geoGroup">The geo group.</param>
        /// <param name="geoSubGroup">The geo subgroup.</param>
        /// <returns>Returns list of MRegion instance.</returns>
        public static NDbResult<List<MRegion>> Gets(string regionId = null,
            string regionName = null, string geoGroup = null, string geoSubGroup = null)
        {
            MethodBase med = MethodBase.GetCurrentMethod();

            NDbResult<List<MRegion>> rets = new NDbResult<List<MRegion>>();

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
            p.Add("@RegionId", regionId);
            p.Add("@RegionName", regionName);
            p.Add("@GeoGroup", geoGroup);
            p.Add("@GeoSubGroup", geoSubGroup);

            try
            {
                rets.Value = cnn.Query<MRegion>("GetMRegions", p,
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
                rets.Value = new List<MRegion>();
            }

            return rets;
        }
        /// <summary>
        /// Save.
        /// </summary>
        /// <param name="value">The MRegion instance.</param>
        /// <returns>Returns NDbResult instance.</returns>
        public static NDbResult Save(MRegion value)
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

            var p = new DynamicParameters();
            p.Add("@RegionId", value.RegionId);
            p.Add("@RegionName", value.RegionName);
            p.Add("@GeoGroup", value.GeoGroup);
            p.Add("@GeoSubGroup", value.GeoSubGroup);

            p.Add("@errNum", dbType: DbType.Int32, direction: ParameterDirection.Output);
            p.Add("@errMsg", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

            try
            {
                cnn.Execute("SaveMRegion", p, commandType: CommandType.StoredProcedure);
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

        #endregion
    }

    #endregion
}
